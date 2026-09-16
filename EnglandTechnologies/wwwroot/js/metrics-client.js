(function () {
    "use strict";

    const API_BASE = "https://portfolioapi.englandtechnologies.net";
    const OFFLINE_THRESHOLD_MS = 20_000;

    const connection = new signalR.HubConnectionBuilder()
        .withUrl(`${API_BASE}/metricsHub`, {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets
        })
        .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
        .configureLogging(signalR.LogLevel.Warning)
        .build();

    /**
     * Updates text content of an element by ID
     * Logs a warning if the element is missing from the DOM
     */
    function set(id, value) {
        const el = document.getElementById(id);
        if (el) {
            el.textContent = value;
        } else {
            console.warn(`[Metrics] Element lookup failed: ID "${id}" not found in HTML.`);
        }
    }

    /**
     * Toggles the online/offline CSS class for a host's status dot
     */
    function setStatus(hostname, online) {
        const dot = document.getElementById(`${hostname}-status`);
        if (dot) {
            dot.classList.toggle("online", online);
            dot.classList.toggle("offline", !online);
        } else {
            console.warn(`[Metrics] Status dot not found for host: "${hostname}-status"`);
        }
    }

    const lastSeen = {};

    /**
     * Watchdog timer: Marks hosts offline if no data is received for OFFLINE_THRESHOLD_MS
     */
    setInterval(() => {
        const now = Date.now();
        Object.entries(lastSeen).forEach(([host, ts]) => {
            if (now - ts > OFFLINE_THRESHOLD_MS) {
                setStatus(host, false);
            }
        });
    }, 15_000);

    /**
     * Renders a metrics payload into the DOM for its host. Shared by the live
     * SignalR feed and the initial REST snapshot, so a host that's currently
     * offline still shows the real last-seen time instead of "--".
     */
    function applyMetrics(data) {
        if (!data || !data.hostname) {
            console.error("[Metrics] Received malformed data packet (missing hostname).");
            return;
        }

        const h = data.hostname;
        const collectedAtMs = new Date(data.collectedAt).getTime();

        // Online state is derived from how stale the timestamp actually is,
        // not from the mere fact that a metrics record exists for the host.
        lastSeen[h] = collectedAtMs;
        setStatus(h, (Date.now() - collectedAtMs) <= OFFLINE_THRESHOLD_MS);

        // Core System Stats
        if (data.cpuUsage != null) set(`${h}-cpuUsage`, `${data.cpuUsage.toFixed(1)}%`);
        if (data.cpuTemp  != null) set(`${h}-cpuTemp`,  `${data.cpuTemp.toFixed(1)}°C`);
        if (data.ramUsagePct  != null) set(`${h}-ramPct`,   `${data.ramUsagePct.toFixed(1)}%`);
        if (data.uptime != null) set(`${h}-uptime`, data.uptime);

        if (data.ramUsedMiB != null && data.ramTotalMiB != null) {
            set(`${h}-ramDetail`,
                `${(data.ramUsedMiB / 1024).toFixed(1)} / ${(data.ramTotalMiB / 1024).toFixed(1)} GiB`);
        }

        // --- DYNAMIC DISK POPULATION ---
        const diskContainer = document.getElementById(`${h}-disks-container`);
        if (diskContainer && Array.isArray(data.disks)) {
            let diskHtml = "";

            data.disks.forEach((disk) => {
                // Label handling: Specific swap for Root, otherwise use the mount path
                let label = disk.device || "Unknown Drive";
                if (label === "/") {
                    label = "Root";
                }

                diskHtml += `
                <div class="metric-row">
                    <span class="metric-label">${label}</span>
                    <span class="metric-value">
                        ${disk.usagePct.toFixed(1)}%
                        <span style="color:#888; font-size:0.8em;">
                            (${disk.usedGiB.toFixed(1)} / ${disk.totalGiB.toFixed(1)} GiB)
                        </span>
                    </span>
                </div>`;
            });

            diskContainer.innerHTML = diskHtml;
        } else if (!diskContainer) {
            console.warn(`[Metrics] Disk container ID "${h}-disks-container" is missing from the page.`);
        }

        // Update Last Seen Timestamp — includes the date so a stale value
        // (host offline for hours/days) doesn't read as "seen today".
        set(`${h}-lastSeen`, Number.isFinite(collectedAtMs) ? new Date(collectedAtMs).toLocaleString() : "--");
    }

    /**
     * Primary data handler for incoming SignalR messages
     */
    connection.on("ReceiveMetrics", (data) => {
        // --- DEBUG LOG ---
        console.log("[Metrics] Data Received:", data);
        applyMetrics(data);
    });

    // Lifecycle Events
    connection.onreconnecting(() => {
        console.warn("[Metrics] SignalR connection lost. Attempting to reconnect...");
    });

    connection.onreconnected(() => {
        console.info("[Metrics] SignalR reconnected successfully.");
    });

    connection.onclose(() => {
        console.error("[Metrics] SignalR connection closed permanently.");
    });

    // Seed the page with the last-known state for every host. Without this,
    // a host that's currently offline never gets pushed anything over
    // SignalR, so "Last Seen" would sit at its placeholder forever instead
    // of showing when it actually dropped off.
    fetch(`${API_BASE}/api/metrics/latest`)
        .then(res => res.ok ? res.json() : Promise.reject(res.status))
        .then(snapshot => snapshot.forEach(applyMetrics))
        .catch(err => console.error("[Metrics] Failed to load last-known metrics snapshot:", err));

    connection.start()
        .then(() => console.info("[Metrics] SignalR connected. Waiting for server metrics..."))
        .catch(err => console.error("[Metrics] SignalR start failure:", err));
})();
