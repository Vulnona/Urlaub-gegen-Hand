// Add this script to show errors directly on page
(function() {
    // Create error display div
    const errorDiv = document.createElement('div');
    errorDiv.id = 'error-display';
    errorDiv.style.cssText = `
        position: fixed;
        top: 10px;
        right: 10px;
        background: #ff4444;
        color: white;
        padding: 15px;
        border-radius: 5px;
        z-index: 10000;
        max-width: 400px;
        font-family: monospace;
        font-size: 12px;
        display: none;
    `;
    
    function showError(message) {
        errorDiv.innerHTML += '<div style="border-bottom: 1px solid #fff; padding: 5px 0;">' + message + '</div>';
        errorDiv.style.display = 'block';
        
        // Auto-hide after 10 seconds
        setTimeout(() => {
            errorDiv.style.display = 'none';
            errorDiv.innerHTML = '';
        }, 10000);
    }
    
    // Override console.error
    const originalError = console.error;
    console.error = function(...args) {
        const message = args.map(arg => 
            typeof arg === 'object' ? JSON.stringify(arg, null, 2) : String(arg)
        ).join(' ');
        
        showError(message);
        originalError.apply(console, args);
    };
    
    // Catch unhandled promise rejections
    window.addEventListener('unhandledrejection', function(event) {
        showError('Promise Rejection: ' + String(event.reason));
    });
    
    // Catch JavaScript errors
    window.addEventListener('error', function(event) {
        showError('JS Error: ' + (event.error?.message || event.message));
    });
    
    // Add to page when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', () => document.body.appendChild(errorDiv));
    } else {
        document.body.appendChild(errorDiv);
    }
})();
