/**
 * Settings Modal JavaScript
 * Handles settings UI interactions and audio controls
 */

function openSettings() {
    const modal = document.getElementById('settingsModal');
    modal.classList.add('active');
    loadSettingsIntoUI();
}

function closeSettings() {
    const modal = document.getElementById('settingsModal');
    modal.classList.remove('active');
}

function loadSettingsIntoUI() {
    if (!window.audioManager) return;
    
    const settings = window.audioManager.getSettings();
    document.getElementById('masterVolume').value = Math.round(settings.masterVolume * 100);
    document.getElementById('musicVolume').value = Math.round(settings.musicVolume * 100);
    document.getElementById('sfxVolume').value = Math.round(settings.sfxVolume * 100);
    
    document.getElementById('masterValue').textContent = Math.round(settings.masterVolume * 100) + '%';
    document.getElementById('musicValue').textContent = Math.round(settings.musicVolume * 100) + '%';
    document.getElementById('sfxValue').textContent = Math.round(settings.sfxVolume * 100) + '%';
    
    const muteBtn = document.getElementById('muteBtn');
    if (settings.muted) {
        muteBtn.classList.add('muted');
        muteBtn.textContent = 'ON';
    } else {
        muteBtn.classList.remove('muted');
        muteBtn.textContent = 'OFF';
    }
}

function toggleMute() {
    if (!window.audioManager) return;
    
    const isMuted = window.audioManager.toggleMute();
    const muteBtn = document.getElementById('muteBtn');
    
    if (isMuted) {
        muteBtn.classList.add('muted');
        muteBtn.textContent = 'ON';
    } else {
        muteBtn.classList.remove('muted');
        muteBtn.textContent = 'OFF';
    }
}

// Initialize event listeners when DOM is ready
document.addEventListener('DOMContentLoaded', function() {
    // Volume slider event listeners
    document.getElementById('masterVolume')?.addEventListener('input', function(e) {
        const value = e.target.value / 100;
        document.getElementById('masterValue').textContent = e.target.value + '%';
        if (window.audioManager) window.audioManager.setMasterVolume(value);
    });

    document.getElementById('musicVolume')?.addEventListener('input', function(e) {
        const value = e.target.value / 100;
        document.getElementById('musicValue').textContent = e.target.value + '%';
        if (window.audioManager) window.audioManager.setMusicVolume(value);
    });

    document.getElementById('sfxVolume')?.addEventListener('input', function(e) {
        const value = e.target.value / 100;
        document.getElementById('sfxValue').textContent = e.target.value + '%';
        if (window.audioManager) window.audioManager.setSfxVolume(value);
    });

    // Close modal on backdrop click
    document.getElementById('settingsModal')?.addEventListener('click', function(e) {
        if (e.target === this) closeSettings();
    });

    // Escape key to close
    document.addEventListener('keydown', function(e) {
        if (e.key === 'Escape') closeSettings();
    });
});
