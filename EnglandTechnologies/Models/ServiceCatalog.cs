namespace EnglandTechnologies.Models;

public record Service(string Slug, string Title, string Time, string Summary, string[] Bullets, int? Price, string? Note = null, string? PriceLabel = null);

public record ServiceCategory(string Id, string Title, string[] Intro, Service[] Items);

public static class ServiceCatalog
{
    // Slugs must match the keys under Booking:ServiceIds in appsettings.json.
    // Times are estimates based on experience.
    public static readonly ServiceCategory[] Categories =
    [
        new("repair", "Diagnostics & Repair",
            ["Find out why your computer is slow, freezing, or crashing, and fix it", "Virus removal", "Reinstalling Windows or macOS"],
            [
                new Service("computer-health-check", "Computer Health Check", "45 minutes",
                    "Test your computer's parts and software to find the cause of crashes, freezes, or overheating.",
                    ["Your computer crashes, freezes, or restarts on its own", "It runs hot or the fans are loud all the time", "You want to know what is wrong before spending money", "Parts and software are tested to find the cause"], 40),
                new Service("speed-up-and-cleanup", "Speed-Up and Cleanup", "60 minutes",
                    "Remove unneeded programs and clutter so your computer starts and runs faster.",
                    ["Your computer takes a long time to start or open programs", "Unneeded programs and startup clutter are removed", "Your files stay where they are"], 55),
                new Service("virus-and-malware-removal", "Virus and Malware Removal", "90 minutes",
                    "Remove pop-ups, browser hijackers, and tracking software without losing your files.",
                    ["Pop-ups or ads keep appearing", "Your browser home page or search keeps changing", "You think something is tracking or slowing your computer", "Malicious software is removed without losing your files"], 75),
                new Service("fresh-windows-or-macos-install", "Fresh Windows or macOS Install", "90 minutes",
                    "Start over with a clean copy of Windows or macOS, with drivers and updates installed.",
                    ["Your system is too broken or cluttered to repair", "A clean copy of Windows or macOS is installed", "Drivers and updates are installed for you"], 75)
            ]),
        new("data", "Files & Data",
            ["Move to a new computer", "Get files back from a broken drive", "Set up backups", "Safely erase old drives"],
            [
                new Service("move-files-to-a-new-computer", "Move Files to a New Computer", "90 minutes",
                    "Move your files, photos, bookmarks, passwords, and settings from your old computer to your new one.",
                    ["You just bought a new computer", "Files, photos, bookmarks, and passwords are moved over", "Your settings are carried across so it feels familiar"], 75),
                new Service("secure-drive-wipe", "Secure Drive Wipe", "90 minutes",
                    "Permanently erase old computers or drives before you recycle, donate, or sell them.",
                    ["You are recycling, donating, or selling a computer or drive", "Your data is permanently erased so it can't be recovered"], 55, "Each additional drive is $15."),
                new Service("data-recovery", "Data Recovery", "180 minutes",
                    "Get your files back from a failing drive, a computer that won't start, or storage that was erased by mistake.",
                    ["A drive is failing or making odd noises", "Your computer will not start and the files matter", "Storage was erased by mistake", "Starts with a 30-minute evaluation of what can be recovered"], 125,
                    "Starts with a 30-minute evaluation of what can be recovered. If you decide not to continue, you pay only a $40 evaluation fee. If you continue, the evaluation fee is included in the $125 recovery price. Recovery results depend on the condition of your drive.", "From $40"),
                new Service("automatic-backup-setup", "Automatic Backup Setup", "60 minutes",
                    "Set up backups to an external drive or the cloud so your files are protected without you thinking about it.",
                    ["You do not have backups, or are not sure you do", "Backups go to an external drive or the cloud", "They run on their own so you do not have to remember"], 55),
                new Service("switch-to-linux", "Switch to Linux", "120 minutes",
                    "Back up your files, replace Windows or macOS with beginner-friendly Linux, and put everything back.",
                    ["Your computer is too old or slow for its current system", "Your files are backed up first", "Beginner-friendly Linux replaces Windows or macOS", "Your files are put back when it is done"], 120)
            ]),
        new("hardware", "Setup, Upgrades & Networking",
            ["Set up a new computer", "Upgrade to a faster drive or more memory", "Fix weak Wi-Fi", "Connect printers, smart TVs, cameras, and other devices"],
            [
                new Service("new-computer-setup", "New Computer Setup", "60 minutes",
                    "Unbox your new computer, set it up, remove junk software, install updates, and connect your accessories.",
                    ["You have a new computer still in the box", "It is unboxed and set up for you", "Junk software is removed and updates are installed", "Your accessories are connected"], 55),
                new Service("faster-drive-ssd-upgrade", "Faster Drive (SSD) Upgrade", "90 minutes",
                    "Copy everything to a new, faster drive and install it. No reinstalling needed.",
                    ["Your computer is slow because of an old drive", "Everything is copied to the new drive", "No reinstalling or starting over"], 75, "Labor only. You supply the drive."),
                new Service("memory-and-parts-installation", "Memory and Parts Installation", "45 minutes",
                    "Install memory, a graphics card, or a power supply in a desktop, then check that it runs reliably.",
                    ["You bought memory, a graphics card, or a power supply", "The part is installed in your desktop", "It is tested to make sure it runs reliably"], 40, "Labor only. You supply the parts."),
                new Service("desk-and-cable-setup", "Desk and Cable Setup", "60 minutes",
                    "Tidy cables, mount monitor arms, and set up a docking station for a clean workspace.",
                    ["Your desk is a tangle of cables", "Cables are tidied", "Monitor arms are mounted and a docking station is set up"], 55),
                new Service("better-wi-fi-setup", "Better Wi-Fi Setup", "90 minutes",
                    "Get a strong Wi-Fi signal in every room, protected with a password and a separate network for guests.",
                    ["Wi-Fi is weak or drops in parts of your home", "Signal is improved in every room", "Your network is protected with a password", "A separate network is set up for guests"], 80, "Labor only. You supply any Wi-Fi equipment."),
                new Service("printer-and-external-drive-setup", "Printer and External Drive Setup", "45 minutes",
                    "Connect printers, scanners, or external drives so every device in the home can use them.",
                    ["A printer, scanner, or external drive will not connect", "It is set up so every device in the home can use it"], 40),
                new Service("smart-device-setup", "Smart Device Setup", "60 minutes",
                    "Connect streaming devices, smart TVs, cameras, and home hubs to your Wi-Fi.",
                    ["You have a streaming device, smart TV, camera, or home hub", "It is connected to your Wi-Fi and working"], 55)
            ]),
        new("other", "Other",
            ["Anything else you need help with that is not listed in the services above"],
            [
                new Service("other-remote", "Other: Remote", "Varies",
                    "This would be for any other questions or concerns that you might have that is not listed in the services above.",
                    ["Any other question or concern not listed above", "Done by video call or remote session"], null, "Billed at the hourly rates below.", "From $55/hr"),
                new Service("other-in-person", "Other: In Person", "Varies",
                    "This would be for any other questions or concerns that you might have that is not listed in the services above.",
                    ["Any other question or concern not listed above", "Done at your location"], null, "Billed at the hourly rates below.", "From $65/hr")
            ])
    ];

    public static Service? Find(string slug) =>
        Categories.SelectMany(c => c.Items).FirstOrDefault(s => s.Slug == slug);
}
