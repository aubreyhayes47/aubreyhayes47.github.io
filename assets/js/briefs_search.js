// Simple client-side filter for the .brief-list created by the generator.
// It hides list items that don't match the search input (title or summary).
(function () {
  function init() {
    const input = document.getElementById('briefs-search-input');
    const clearBtn = document.getElementById('briefs-search-clear');
    if (!input) return;

    const list = document.querySelectorAll('.brief-list li');
    if (!list || list.length === 0) return;

    function normalize(s) {
      return (s || '').toLowerCase().trim();
    }

    function update() {
      const q = normalize(input.value);
      let anyVisible = false;
      list.forEach(li => {
        const text = normalize(li.textContent || li.innerText || '');
        const visible = q === '' || text.indexOf(q) !== -1;
        li.style.display = visible ? '' : 'none';
        if (visible) anyVisible = true;
      });
      clearBtn.style.display = input.value ? '' : 'none';
      // Optionally show a no-results message
      let noResults = document.getElementById('briefs-no-results');
      if (!anyVisible) {
        if (!noResults) {
          noResults = document.createElement('p');
          noResults.id = 'briefs-no-results';
          noResults.textContent = 'No briefs match your search.';
          noResults.style.color = '#666';
          const container = document.querySelector('.briefs-search');
          if (container) container.appendChild(noResults);
        }
      } else {
        if (noResults) noResults.parentNode.removeChild(noResults);
      }
    }

    input.addEventListener('input', update);
    clearBtn.addEventListener('click', function () {
      input.value = '';
      update();
      input.focus();
    });

    // keyboard shortcut: press "/" to focus search when not focused in an input
    document.addEventListener('keydown', function (e) {
      if (e.key === '/' && document.activeElement.tagName.toLowerCase() !== 'input' && document.activeElement.tagName.toLowerCase() !== 'textarea') {
        e.preventDefault();
        input.focus();
      }
    });
  }

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', init);
  } else {
    init();
  }
})();