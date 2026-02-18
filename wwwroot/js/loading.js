/**
 * LoadingOverlay - Reusable loading state component
 * Usage:
 *   const loader = new LoadingOverlay();
 *   loader.show('Initializing...');
 *   // Do async work
 *   loader.hide();
 */
class LoadingOverlay {
    constructor(options = {}) {
        this.options = {
            spinner: true,
            progressBar: true,
            defaultText: 'LOADING...',
            ...options
        };
        
        this.overlay = null;
        this.spinner = null;
        this.text = null;
        this.status = null;
        this.progressBar = null;
        
        this.init();
    }
    
    init() {
        // Check if overlay already exists
        this.overlay = document.getElementById('loading-overlay');
        
        if (!this.overlay) {
            this.createOverlay();
        }
        
        this.cacheElements();
    }
    
    createOverlay() {
        this.overlay = document.createElement('div');
        this.overlay.id = 'loading-overlay';
        this.overlay.className = 'loading-overlay';
        
        let html = '';
        
        if (this.options.spinner) {
            html += '<div class="loading-spinner"></div>';
        }
        
        html += '<div class="loading-text" id="loading-text">' + this.options.defaultText + '</div>';
        html += '<div class="loading-status" id="loading-status"></div>';
        
        if (this.options.progressBar) {
            html += '<div class="loading-progress"><div class="loading-progress-bar"></div></div>';
        }
        
        this.overlay.innerHTML = html;
        document.body.appendChild(this.overlay);
    }
    
    cacheElements() {
        this.text = document.getElementById('loading-text');
        this.status = document.getElementById('loading-status');
    }
    
    /**
     * Show the loading overlay
     * @param {string} text - Main loading text
     * @param {string} status - Secondary status/info text
     */
    show(text = null, status = null) {
        if (text && this.text) {
            this.text.textContent = text;
        }
        
        if (status && this.status) {
            this.status.textContent = status;
        }
        
        this.overlay.classList.add('active');
        document.body.style.overflow = 'hidden';
    }
    
    /**
     * Update the loading text/status
     * @param {string} text - Main loading text
     * @param {string} status - Secondary status/info text
     */
    update(text = null, status = null) {
        if (text && this.text) {
            this.text.textContent = text;
        }
        
        if (status !== undefined && this.status) {
            this.status.textContent = status || '';
        }
    }
    
    /**
     * Hide the loading overlay
     */
    hide() {
        this.overlay.classList.remove('active');
        document.body.style.overflow = '';
        
        // Reset text after transition
        setTimeout(() => {
            if (this.text) {
                this.text.textContent = this.options.defaultText;
            }
            if (this.status) {
                this.status.textContent = '';
            }
        }, 300);
    }
    
    /**
     * Show loading while promise resolves
     * @param {Promise} promise - Promise to wait for
     * @param {string} text - Loading text
     * @returns {Promise} - Resolves with original promise result
     */
    async wrap(promise, text = null) {
        this.show(text);
        try {
            const result = await promise;
            return result;
        } finally {
            this.hide();
        }
    }
}

/**
 * Button Loading State
 * Usage:
 *   const btnLoader = new ButtonLoader(buttonElement);
 *   btnLoader.start('Processing...');
 *   // Do work
 *   btnLoader.stop();
 */
class ButtonLoader {
    constructor(button) {
        this.button = button;
        this.originalText = button.innerHTML;
        this.isLoading = false;
    }
    
    start(text = 'LOADING...') {
        if (this.isLoading) return;
        
        this.isLoading = true;
        this.button.classList.add('btn-loading');
        this.button.innerHTML = `<span>${text}</span>`;
        this.button.disabled = true;
    }
    
    stop() {
        if (!this.isLoading) return;
        
        this.isLoading = false;
        this.button.classList.remove('btn-loading');
        this.button.innerHTML = this.originalText;
        this.button.disabled = false;
    }
}

// Export for module usage
if (typeof module !== 'undefined' && module.exports) {
    module.exports = { LoadingOverlay, ButtonLoader };
}
