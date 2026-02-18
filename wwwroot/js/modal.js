/**
 * GameModal - Themed modal system for The Signal
 * Usage:
 *   const modal = new GameModal();
 *   modal.show({
 *       title: 'Terminal Access',
 *       content: 'System diagnostics...',
 *       type: 'terminal',
 *       icon: '🖥'
 *   });
 */
class GameModal {
    constructor() {
        this.overlay = null;
        this.container = null;
        this.header = null;
        this.body = null;
        this.closeBtn = null;
        this.onCloseCallback = null;
        
        this.init();
    }
    
    init() {
        // Check if modal already exists
        if (document.getElementById('game-modal')) {
            this.overlay = document.getElementById('game-modal');
            this.cacheElements();
            return;
        }
        
        this.createModal();
    }
    
    createModal() {
        this.overlay = document.createElement('div');
        this.overlay.id = 'game-modal';
        this.overlay.className = 'modal-overlay';
        
        this.overlay.innerHTML = `
            <div class="modal-container" id="modal-container">
                <div class="modal-header" id="modal-header">
                    <div class="modal-title">
                        <span class="modal-icon" id="modal-icon">📄</span>
                        <span id="modal-title-text">Modal Title</span>
                    </div>
                    <button class="modal-close" id="modal-close">×</button>
                </div>
                <div class="modal-body" id="modal-body">
                    <div class="modal-content" id="modal-content">
                        Modal content goes here...
                    </div>
                </div>
            </div>
        `;
        
        document.body.appendChild(this.overlay);
        this.cacheElements();
        this.bindEvents();
    }
    
    cacheElements() {
        this.container = document.getElementById('modal-container');
        this.header = document.getElementById('modal-header');
        this.body = document.getElementById('modal-body');
        this.closeBtn = document.getElementById('modal-close');
        this.titleText = document.getElementById('modal-title-text');
        this.content = document.getElementById('modal-content');
        this.icon = document.getElementById('modal-icon');
    }
    
    bindEvents() {
        this.closeBtn.addEventListener('click', () => this.hide());
        this.overlay.addEventListener('click', (e) => {
            if (e.target === this.overlay) {
                this.hide();
            }
        });
        
        document.addEventListener('keydown', (e) => {
            if (e.key === 'Escape' && this.overlay.classList.contains('active')) {
                this.hide();
            }
        });
    }
    
    /**
     * Show the modal
     * @param {Object} options - Modal options
     * @param {string} options.title - Modal title
     * @param {string} options.content - Modal content (HTML or text)
     * @param {string} options.type - Modal type: 'terminal', 'warning', 'danger'
     * @param {string} options.icon - Icon to display
     * @param {Function} options.onClose - Callback when modal closes
     */
    show(options = {}) {
        const {
            title = 'Modal',
            content = '',
            type = 'terminal',
            icon = '📄',
            onClose = null
        } = options;
        
        // Set content
        this.titleText.textContent = title;
        this.content.innerHTML = content;
        this.icon.textContent = icon;
        
        // Set type class
        this.container.className = 'modal-container';
        this.container.classList.add(`modal-${type}`);
        
        // Store callback
        this.onCloseCallback = onClose;
        
        // Show modal
        this.overlay.classList.add('active');
        document.body.style.overflow = 'hidden';
    }
    
    /**
     * Hide the modal
     */
    hide() {
        this.overlay.classList.remove('active');
        document.body.style.overflow = '';
        
        if (this.onCloseCallback) {
            this.onCloseCallback();
            this.onCloseCallback = null;
        }
    }
    
    /**
     * Show discovery notification
     * @param {string} title - Discovery title
     * @param {string} description - Discovery description
     */
    showDiscovery(title, description) {
        let discovery = document.getElementById('modal-discovery');
        
        if (!discovery) {
            discovery = document.createElement('div');
            discovery.id = 'modal-discovery';
            discovery.className = 'modal-discovery';
            discovery.innerHTML = `
                <div class="discovery-title">NEW DISCOVERY</div>
                <div class="discovery-content" id="discovery-content"></div>
            `;
            document.body.appendChild(discovery);
        }
        
        const content = discovery.querySelector('#discovery-content');
        content.innerHTML = `<strong>${title}</strong><br>${description}`;
        
        discovery.classList.add('active');
        
        // Auto-hide after 4 seconds
        setTimeout(() => {
            discovery.classList.remove('active');
        }, 4000);
    }
}

// Export for module usage
if (typeof module !== 'undefined' && module.exports) {
    module.exports = { GameModal };
}
