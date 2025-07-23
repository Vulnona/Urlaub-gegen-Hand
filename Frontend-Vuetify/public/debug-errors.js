// Console error catcher for debugging
(function() {
    // Override console.error to capture all errors
    const originalError = console.error;
    console.error = function(...args) {
        // Also send errors to a div for visibility
        const errorDiv = document.getElementById('js-errors') || (() => {
            const div = document.createElement('div');
            div.id = 'js-errors';
            div.style.cssText = 'position:fixed;top:0;right:0;background:red;color:white;padding:10px;z-index:9999;max-width:300px;';
            document.body.appendChild(div);
            return div;
        })();
        
        errorDiv.innerHTML += '<div>' + args.map(arg => 
            typeof arg === 'object' ? JSON.stringify(arg) : String(arg)
        ).join(' ') + '</div>';
        
        originalError.apply(console, args);
    };

    // Catch unhandled promise rejections
    window.addEventListener('unhandledrejection', function(event) {
        console.error('Unhandled Promise Rejection:', event.reason);
    });

    // Catch all JavaScript errors
    window.addEventListener('error', function(event) {
        console.error('JavaScript Error:', event.error || event.message);
    });
})();
