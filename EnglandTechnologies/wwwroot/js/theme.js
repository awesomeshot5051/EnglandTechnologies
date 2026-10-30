/* Theme selection. Loaded synchronously in <head> so the right theme is set before first paint.
   The visitor's choice (auto | dark | light) is saved in localStorage under "et-theme-mode".
   "auto" follows the browser's prefers-color-scheme (derived from the OS setting), and
   falls back to dark if that cannot be determined. window.etTheme exposes the mode to the toggle button. */
(function () {
  var root = document.documentElement;
  var KEY = 'et-theme-mode';
  var MODES = ['auto', 'dark', 'light'];

  function getMode() {
    try {
      var m = localStorage.getItem(KEY);
      if (MODES.indexOf(m) !== -1) return m;
    } catch (e) { /* storage blocked */ }
    return 'auto';
  }

  function detect() {
    try {
      if (window.matchMedia) {
        if (window.matchMedia('(prefers-color-scheme: light)').matches) return 'light';
        if (window.matchMedia('(prefers-color-scheme: dark)').matches) return 'dark';
      }
    } catch (e) { /* fall through */ }
    return 'dark';
  }

  function apply() {
    var mode = getMode();
    var theme = mode === 'auto' ? detect() : mode;
    root.setAttribute('data-theme-mode', mode);
    root.setAttribute('data-theme', theme);
    root.setAttribute('data-bs-theme', theme);
    root.style.colorScheme = theme;
  }

  function setMode(mode) {
    try { localStorage.setItem(KEY, mode); } catch (e) { /* ignore */ }
    apply();
  }

  function cycle() {
    var next = MODES[(MODES.indexOf(getMode()) + 1) % MODES.length];
    setMode(next);
    return next;
  }

  window.etTheme = { getMode: getMode, setMode: setMode, cycle: cycle };

  apply();

  try {
    var mq = window.matchMedia('(prefers-color-scheme: light)');
    if (mq.addEventListener) mq.addEventListener('change', apply);
    else if (mq.addListener) mq.addListener(apply);
  } catch (e) { /* ignore */ }
})();
