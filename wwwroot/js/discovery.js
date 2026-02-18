// Discovery Manager - Handles interactive discovery of items in the narrative
class DiscoveryManager {
    constructor(characterId, locationId) {
        this.characterId = characterId;
        this.locationId = locationId;
        this.discovered = new Set();
        this.initializing = false;
        this.storageKey = `signal_discovered_${characterId}_${locationId}`;
    }

    init() {
        if (this.initializing) return;
        this.initializing = true;

        // Load discovered interactables from localStorage first
        this.loadDiscoveredFromStorage();

        // Load already discovered interactables from the page (in case backend already knows)
        this.loadDiscoveredFromPage();

        // Apply discovered state to UI
        this.applyDiscoveredState();

        // Attach click handlers to all discovery-hud elements
        this.attachDiscoveryHandlers();

        this.initializing = false;
    }

    loadDiscoveredFromStorage() {
        try {
            const stored = localStorage.getItem(this.storageKey);
            if (stored) {
                const storedIds = JSON.parse(stored);
                storedIds.forEach(id => this.discovered.add(id));
            }
        } catch (e) {
            console.error('Failed to load discovered interactables from storage:', e);
        }
    }

    saveDiscoveredToStorage() {
        try {
            const ids = Array.from(this.discovered);
            localStorage.setItem(this.storageKey, JSON.stringify(ids));
        } catch (e) {
            console.error('Failed to save discovered interactables to storage:', e);
        }
    }

    applyDiscoveredState() {
        // Mark discovery links as discovered based on localStorage
        document.querySelectorAll('.discovery-hud').forEach(link => {
            const interactableId = link.dataset.id;
            if (this.discovered.has(interactableId)) {
                link.classList.add('discovered');
                // Update aria-label
                const originalLabel = link.getAttribute('aria-label') || '';
                if (!originalLabel.includes('(discovered)')) {
                    link.setAttribute('aria-label', originalLabel + ' (discovered)');
                }
            }
        });
    }

    loadDiscoveredFromPage() {
        // Get all already discovered interactables from the buttons in the DOM
        const buttons = document.querySelectorAll('#discovered-interactables .interactable-btn');
        buttons.forEach(btn => {
            const onclick = btn.getAttribute('onclick');
            if (onclick) {
                const match = onclick.match(/interact\('([^']+)'\)/);
                if (match) {
                    this.discovered.add(match[1]);
                }
            }
        });
    }

    attachDiscoveryHandlers() {
        document.querySelectorAll('.discovery-hud').forEach(link => {
            // Remove existing handlers to prevent duplicates
            link.removeEventListener('click', this.handleDiscovery);
            link.addEventListener('click', (e) => this.handleDiscovery(e));
        });
    }

    async handleDiscovery(event) {
        event.preventDefault();
        const link = event.target.closest('.discovery-hud');
        if (!link) return;

        const interactableId = link.dataset.id;

        if (this.discovered.has(interactableId)) return;

        // Call API to discover
        try {
            const response = await fetch('/api/discovery/interactable', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    characterId: this.characterId,
                    interactableId: interactableId,
                    locationId: this.locationId
                })
            });

            if (response.ok) {
                const data = await response.json();
                if (data.success && !data.alreadyDiscovered && data.interactable) {
                    this.addToInteractables(data.interactable);
                    this.markDiscovered(link);
                    this.showDiscoveryToast(data.interactable.name);
                    this.addToActionLog(`Discovered: ${data.interactable.name}`);
                } else if (data.alreadyDiscovered) {
                    this.markDiscovered(link);
                }
            }
        } catch (error) {
            console.error('Discovery failed:', error);
        }
    }

    addToInteractables(interactable) {
        const grid = document.getElementById('discovered-interactables');
        const noDiscoveriesHint = document.querySelector('.no-discoveries-hint');

        // Remove the "no discoveries" hint if present
        if (noDiscoveriesHint) {
            noDiscoveriesHint.remove();
        }

        // Check if button already exists
        const existingBtn = grid.querySelector(`button[onclick="interact('${interactable.id}')"]`);
        if (existingBtn) return;

        const button = document.createElement('button');
        button.className = `interactable-btn ${interactable.isKeyItem ? 'key-item' : ''}`;
        button.onclick = () => interact(interactable.id);
        button.innerHTML = `
            <span class="interactable-name">${interactable.name}</span>
            <span class="interactable-action">${interactable.action}</span>
        `;
        grid.appendChild(button);

        // Animate in
        button.style.opacity = '0';
        button.style.transform = 'translateY(10px)';
        requestAnimationFrame(() => {
            button.style.transition = 'all 0.3s ease';
            button.style.opacity = '1';
            button.style.transform = 'translateY(0)';
        });

        this.discovered.add(interactable.id);

        // Save to localStorage
        this.saveDiscoveredToStorage();
    }

    markDiscovered(link) {
        link.classList.add('discovered');
        const interactableId = link.dataset.id;
        this.discovered.add(interactableId);

        // Save to localStorage
        this.saveDiscoveredToStorage();

        // Update aria-label to indicate discovered state
        const originalLabel = link.getAttribute('aria-label') || '';
        if (!originalLabel.includes('(discovered)')) {
            link.setAttribute('aria-label', originalLabel + ' (discovered)');
        }
    }

    showDiscoveryToast(name) {
        // Create toast notification
        const toast = document.createElement('div');
        toast.className = 'discovery-toast';
        toast.innerHTML = `
            <span class="toast-icon">✓</span>
            <span class="toast-text">Discovered: ${name}</span>
        `;
        document.body.appendChild(toast);

        // Animate in
        requestAnimationFrame(() => {
            toast.classList.add('show');
        });

        // Remove after 3 seconds
        setTimeout(() => {
            toast.classList.remove('show');
            setTimeout(() => toast.remove(), 300);
        }, 3000);
    }

    addToActionLog(message) {
        // Find or create the action log section
        let actionLogSection = document.querySelector('.right-panel .panel-section:has(.action-log)');
        let actionLog = document.querySelector('.action-log');
        
        // If no action log section exists, create it
        if (!actionLogSection) {
            const rightPanel = document.querySelector('.right-panel');
            if (!rightPanel) return;
            
            actionLogSection = document.createElement('div');
            actionLogSection.className = 'panel-section';
            actionLogSection.innerHTML = `
                <h3 class="panel-title">LOG</h3>
                <div class="action-log"></div>
            `;
            rightPanel.appendChild(actionLogSection);
            actionLog = actionLogSection.querySelector('.action-log');
        }
        
        // Create new log entry
        const entry = document.createElement('div');
        entry.className = 'log-entry';
        entry.textContent = `> ${message}`;
        
        // Add to log
        actionLog.insertBefore(entry, actionLog.firstChild);
        
        // Keep only last 10 entries
        while (actionLog.children.length > 10) {
            actionLog.removeChild(actionLog.lastChild);
        }
        
        // Scroll to top
        actionLog.scrollTop = 0;
    }

    // Static method to clear storage for a character/location (call when moving locations)
    static clearStorage(characterId, locationId) {
        try {
            const storageKey = `signal_discovered_${characterId}_${locationId}`;
            localStorage.removeItem(storageKey);
        } catch (e) {
            console.error('Failed to clear discovered interactables storage:', e);
        }
    }

    // Static method to clear all discovery storage for a character
    static clearAllForCharacter(characterId) {
        try {
            const prefix = `signal_discovered_${characterId}_`;
            for (let i = localStorage.length - 1; i >= 0; i--) {
                const key = localStorage.key(i);
                if (key && key.startsWith(prefix)) {
                    localStorage.removeItem(key);
                }
            }
        } catch (e) {
            console.error('Failed to clear all discovery storage:', e);
        }
    }
}
