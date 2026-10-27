/* Theme selection. Loaded synchronously in <head> so the right theme is set before first paint.
   Order: 1) the browser's preference (prefers-color-scheme), which browsers derive from the
   OS setting when set to "auto"; 2) a direct light/dark query as a second check;
   3) dark if neither can be determined. */
(function () {
  var root = document.documentElement;

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
    var theme = detect();
    root.setAttribute('data-theme', theme);
    root.setAttribute('data-bs-theme', theme);
    root.style.colorScheme = theme;
  }

  apply();

  try {
    var mq = window.matchMedia('(prefers-color-scheme: light)');
    if (mq.addEventListener) mq.addEventListener('change', apply);
    else if (mq.addListener) mq.addListener(apply);
  } catch (e) { /* ignore */ }
})();
