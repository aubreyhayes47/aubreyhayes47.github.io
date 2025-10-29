#!/usr/bin/env node

const fs = require('fs');
const path = require('path');

const briefsDir = path.join(process.cwd(), 'briefs');
if (!fs.existsSync(briefsDir)) {
  console.error('Directory "briefs" not found in repository root. Create a /briefs folder with course subfolders first.');
  process.exit(1);
}

const courseDirs = fs.readdirSync(briefsDir).filter(name => {
  try { return fs.statSync(path.join(briefsDir, name)).isDirectory(); } catch(e){ return false; }
});

courseDirs.forEach(course => {
  const coursePath = path.join(briefsDir, course);
  const files = fs.readdirSync(coursePath).filter(f => f.endsWith('.md') && f.toLowerCase() !== 'index.md');

  const entries = files.map(file => {
    const full = path.join(coursePath, file);
    const content = fs.readFileSync(full, 'utf8');

    // Parse YAML front matter (simple) if present
    const fmMatch = content.match(/^---\n([\s\S]*?)\n---/);
    let title = file.replace(/\.md$/i, '').replace(/_/g, ' ');
    title = title.replace(/\b\w/g, c => c.toUpperCase());
    let permalink = `/briefs/${course}/${file.replace(/\.md$/i, '/')}`;
    let summary = '';

    if (fmMatch) {
      const fm = fmMatch[1];
      const titleMatch = fm.match(/title:\s*["']?(.+?)["']?$/m);
      if (titleMatch) title = titleMatch[1].trim();
      const permalinkMatch = fm.match(/permalink:\s*(.+)$/m);
      if (permalinkMatch) permalink = permalinkMatch[1].trim();
    }

    const after = fmMatch ? content.slice(fmMatch[0].length).trim() : content.trim();
    const firstPara = after.split(/\n\s*\n/).find(p => p && p.trim().length > 0);
    if (firstPara) summary = firstPara.replace(/\n/g, ' ').trim();
    if (summary.length > 200) summary = summary.slice(0, 197) + '...';

    return { title, permalink, summary };
  });

  // Build index content
  const prettyCourse = course.charAt(0).toUpperCase() + course.slice(1).replace(/[-_]/g, ' ');
  let indexContent = `---\ntitle: "${prettyCourse} Briefs"\npermalink: /briefs/${course}/\n---\n\n`;
  indexContent += `# ${prettyCourse} — Case Briefs\n\n`;
  indexContent += `{% include briefs_search.html %}\n\n`;
  indexContent += '<ul class="brief-list">\n';
  entries.forEach(e => {
    indexContent += `  <li><a href="${e.permalink}">${e.title}</a>`;
    if (e.summary) indexContent += ` — ${e.summary}`;
    indexContent += '</li>\n';
  });
  indexContent += '</ul>\n';

  fs.writeFileSync(path.join(coursePath, 'index.md'), indexContent, 'utf8');
  console.log(`Wrote index for ${course}`);
});

console.log('Done.');
