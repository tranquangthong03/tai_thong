// This script ensures the testimonials section doesn't get overlapped by the footer
document.addEventListener('DOMContentLoaded', function() {
    // Function to adjust spacing if needed
    function adjustFooterSpacing() {
        const testimonialsSection = document.querySelector('.testimonials-section');
        const footer = document.getElementById('site-footer');
        const footerSpacing = document.querySelector('.footer-spacing');
        
        if (testimonialsSection && footer && footerSpacing) {
            // Calculate the distance between the bottom of testimonials and top of footer content
            const testimonialRect = testimonialsSection.getBoundingClientRect();
            const testimonialBottom = testimonialRect.bottom;
            
            // Add more spacing if needed
            if (window.innerHeight - testimonialBottom < 100) {
                footerSpacing.style.height = '150px';
            }
        }
    }
    
    // Run on page load
    adjustFooterSpacing();
    
    // Run on window resize
    window.addEventListener('resize', adjustFooterSpacing);
});
