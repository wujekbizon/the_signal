// Character creation UI logic
// Data is passed from server via window.classData

let selectedClassIndex = 0;

function init() {
    // Class selection handlers
    document.querySelectorAll('.class-item').forEach(btn => {
        btn.addEventListener('click', function() {
            const index = parseInt(this.dataset.classIndex);
            selectClass(index);
        });
    });

    // Start mission button
    document.getElementById('start-mission-btn').addEventListener('click', startMission);

    // Keyboard navigation
    document.addEventListener('keydown', handleKeyboard);

    // Timestamp
    setInterval(updateTimestamp, 1000);
    updateTimestamp();
}

function selectClass(index) {
    selectedClassIndex = index;
    const classInfo = window.classData[index];

    // Update active class button
    document.querySelectorAll('.class-item').forEach(b => b.classList.remove('active'));
    document.querySelector(`[data-class-index="${index}"]`).classList.add('active');

    // Update display
    document.getElementById('class-title').textContent = classInfo.Name + ' PROFILE';
    document.getElementById('class-spec').textContent = classInfo.Specialization;
    document.getElementById('class-desc').textContent = classInfo.Description;

    // Update bio
    const bioElement = document.getElementById('class-bio');
    const bioLines = classInfo.Bio.split('\n');
    bioElement.innerHTML = bioLines.map(line => `<p>${line}</p>`).join('');

    // Update portrait
    const portraitImg = document.querySelector('#char-portrait img');
    if (portraitImg && classInfo.ImagePath) {
        portraitImg.src = classInfo.ImagePath;
        portraitImg.alt = classInfo.Name;
    }

    // Update stats (base 10 + bonus)
    const baseValue = 10;
    document.getElementById('stat-technical').textContent = generateStatBar(baseValue + classInfo.TechnicalBonus);
    document.getElementById('stat-technical-val').textContent = baseValue + classInfo.TechnicalBonus;
    document.getElementById('stat-combat').textContent = generateStatBar(baseValue + classInfo.CombatBonus);
    document.getElementById('stat-combat-val').textContent = baseValue + classInfo.CombatBonus;
    document.getElementById('stat-medical').textContent = generateStatBar(baseValue + classInfo.MedicalBonus);
    document.getElementById('stat-medical-val').textContent = baseValue + classInfo.MedicalBonus;
    document.getElementById('stat-science').textContent = generateStatBar(baseValue + classInfo.ScienceBonus);
    document.getElementById('stat-science-val').textContent = baseValue + classInfo.ScienceBonus;
    document.getElementById('stat-stealth').textContent = generateStatBar(baseValue + classInfo.StealthBonus);
    document.getElementById('stat-stealth-val').textContent = baseValue + classInfo.StealthBonus;
    document.getElementById('stat-leadership').textContent = generateStatBar(baseValue + classInfo.LeadershipBonus);
    document.getElementById('stat-leadership-val').textContent = baseValue + classInfo.LeadershipBonus;
}

function generateStatBar(value) {
    const filled = Math.floor(value / 10);
    const empty = 10 - filled;
    return '█'.repeat(filled) + '░'.repeat(empty);
}

function startMission() {
    const nickname = document.getElementById('character-nickname').value.trim();

    if (!nickname) {
        alert('Enter callsign to proceed.');
        return;
    }

    const selectedClass = window.classData[selectedClassIndex];
    
    // Show loading overlay
    const loader = new LoadingOverlay();
    loader.show('WAKING UP...', `Initializing cryo-pod revival protocols for ${nickname}...`);
    
    // Create a form and submit it normally (preserves TempData)
    const form = document.createElement('form');
    form.method = 'POST';
    form.action = '/Game/CreateCharacter';
    
    const nameInput = document.createElement('input');
    nameInput.type = 'hidden';
    nameInput.name = 'name';
    nameInput.value = nickname;
    form.appendChild(nameInput);
    
    const classInput = document.createElement('input');
    classInput.type = 'hidden';
    classInput.name = 'characterClass';
    classInput.value = selectedClass.Type;
    form.appendChild(classInput);
    
    document.body.appendChild(form);
    
    // Small delay to show loading animation
    setTimeout(() => {
        loader.update('REVIVING...', 'Restoring consciousness from emergency stasis...');
        setTimeout(() => {
            form.submit();
        }, 600);
    }, 400);
}

function handleKeyboard(e) {
    if (e.key === 'ArrowDown') {
        e.preventDefault();
        const nextIndex = (selectedClassIndex + 1) % window.classData.length;
        selectClass(nextIndex);
    } else if (e.key === 'ArrowUp') {
        e.preventDefault();
        const prevIndex = (selectedClassIndex - 1 + window.classData.length) % window.classData.length;
        selectClass(prevIndex);
    } else if (e.key === 'Enter') {
        e.preventDefault();
        document.getElementById('start-mission-btn').click();
    } else if (e.key === 'Escape') {
        window.location.href = '/';
    }
}

function updateTimestamp() {
    const now = new Date();
    const timeString = now.toLocaleTimeString('en-US', { hour12: false });
    document.getElementById('header-timestamp').textContent = timeString;
}

// Initialize on DOM ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', init);
} else {
    init();
}
