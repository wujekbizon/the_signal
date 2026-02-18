/**
 * AudioManager - Handles game audio, music, and sound effects
 * Persists settings to localStorage
 */
class AudioManager {
    constructor() {
        this.audioContext = null;
        this.masterGain = null;
        this.musicGain = null;
        this.sfxGain = null;
        this.currentMusic = null;
        this.isMuted = false;
        this.settings = this.loadSettings();
        this.initialized = false;
    }

    loadSettings() {
        const defaults = {
            masterVolume: 0.3,
            musicVolume: 0.7,
            sfxVolume: 0.8,
            muted: false
        };
        try {
            const saved = localStorage.getItem('signal_audio_settings');
            return saved ? { ...defaults, ...JSON.parse(saved) } : defaults;
        } catch {
            return defaults;
        }
    }

    saveSettings() {
        localStorage.setItem('signal_audio_settings', JSON.stringify({
            masterVolume: this.settings.masterVolume,
            musicVolume: this.settings.musicVolume,
            sfxVolume: this.settings.sfxVolume,
            muted: this.isMuted
        }));
    }

    async init() {
        if (this.initialized) return;
        
        try {
            this.audioContext = new (window.AudioContext || window.webkitAudioContext)();
            
            this.masterGain = this.audioContext.createGain();
            this.musicGain = this.audioContext.createGain();
            this.sfxGain = this.audioContext.createGain();
            
            this.musicGain.connect(this.masterGain);
            this.sfxGain.connect(this.masterGain);
            this.masterGain.connect(this.audioContext.destination);
            
            this.applySettings();
            this.initialized = true;
            
            console.log('AudioManager initialized');
        } catch (error) {
            console.error('Failed to initialize AudioManager:', error);
        }
    }

    applySettings() {
        if (!this.masterGain) return;
        
        const masterVol = this.isMuted ? 0 : this.settings.masterVolume;
        this.masterGain.gain.value = masterVol;
        this.musicGain.gain.value = this.settings.musicVolume;
        this.sfxGain.gain.value = this.settings.sfxVolume;
    }

    async playMusic(url, loop = true, fadeIn = true) {
        if (!this.initialized) await this.init();
        if (this.audioContext.state === 'suspended') {
            await this.audioContext.resume();
        }

        // Stop current music
        if (this.currentMusic) {
            await this.stopMusic(true);
        }

        try {
            const response = await fetch(url);
            const arrayBuffer = await response.arrayBuffer();
            const audioBuffer = await this.audioContext.decodeAudioData(arrayBuffer);
            
            const source = this.audioContext.createBufferSource();
            source.buffer = audioBuffer;
            source.loop = loop;
            
            const gainNode = this.audioContext.createGain();
            gainNode.connect(this.musicGain);
            source.connect(gainNode);
            
            if (fadeIn) {
                gainNode.gain.setValueAtTime(0, this.audioContext.currentTime);
                gainNode.gain.linearRampToValueAtTime(1, this.audioContext.currentTime + 2);
            } else {
                gainNode.gain.value = 1;
            }
            
            source.start(0);
            this.currentMusic = { source, gainNode };
            
            console.log('Playing music:', url);
        } catch (error) {
            console.error('Failed to play music:', error);
        }
    }

    async stopMusic(fadeOut = true) {
        if (!this.currentMusic) return;
        
        const { source, gainNode } = this.currentMusic;
        
        if (fadeOut && this.audioContext) {
            gainNode.gain.linearRampToValueAtTime(0, this.audioContext.currentTime + 1);
            await new Promise(resolve => setTimeout(resolve, 1000));
        }
        
        try {
            source.stop();
        } catch {}
        
        this.currentMusic = null;
    }

    setMasterVolume(value) {
        this.settings.masterVolume = Math.max(0, Math.min(1, value));
        this.applySettings();
        this.saveSettings();
    }

    setMusicVolume(value) {
        this.settings.musicVolume = Math.max(0, Math.min(1, value));
        this.applySettings();
        this.saveSettings();
    }

    setSfxVolume(value) {
        this.settings.sfxVolume = Math.max(0, Math.min(1, value));
        this.applySettings();
        this.saveSettings();
    }

    toggleMute() {
        this.isMuted = !this.isMuted;
        this.applySettings();
        this.saveSettings();
        return this.isMuted;
    }

    getSettings() {
        return {
            masterVolume: this.settings.masterVolume,
            musicVolume: this.settings.musicVolume,
            sfxVolume: this.settings.sfxVolume,
            muted: this.isMuted
        };
    }
}

// Global instance
window.audioManager = new AudioManager();

// Initialize on first user interaction (browser autoplay policy)
let audioInitialized = false;
function initAudioOnInteraction() {
    if (!audioInitialized) {
        window.audioManager.init();
        audioInitialized = true;
        document.removeEventListener('click', initAudioOnInteraction);
        document.removeEventListener('keydown', initAudioOnInteraction);
    }
}

document.addEventListener('click', initAudioOnInteraction);
document.addEventListener('keydown', initAudioOnInteraction);
