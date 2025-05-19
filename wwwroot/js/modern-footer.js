// Modern Footer JS

document.addEventListener('DOMContentLoaded', function() {
    // Back to top button functionality
    const backToTopButton = document.querySelector('.back-to-top');
    if (backToTopButton) {
        window.addEventListener('scroll', () => {
            if (window.scrollY > 300) {
                backToTopButton.classList.add('show');
            } else {
                backToTopButton.classList.remove('show');
            }
        });

        backToTopButton.addEventListener('click', () => {
            window.scrollTo({
                top: 0,
                behavior: 'smooth'
            });
        });
    }
    
    // Add animation delays to footer widgets
    document.querySelectorAll('.footer-widget').forEach((widget, index) => {
        widget.style.setProperty('--widget-order', index + 1);
    });
    
    // Create floating particles for the footer
    const footer = document.querySelector('.modern-footer');
    if (footer) {
        createFooterParticles(footer);
    }
});

function createFooterParticles(footer) {
    // Create 10 random particles
    for (let i = 0; i < 10; i++) {
        const particle = document.createElement('div');
        particle.classList.add('footer-particle');
        
        // Random size between 5px and 15px
        const size = Math.random() * 10 + 5;
        particle.style.width = `${size}px`;
        particle.style.height = `${size}px`;
        
        // Random position
        const posX = Math.random() * 100;
        const posY = Math.random() * 100;
        particle.style.left = `${posX}%`;
        particle.style.top = `${posY}%`;
        
        // Random animation delay
        particle.style.animationDelay = `${Math.random() * 5}s`;
        
        footer.appendChild(particle);
    }
}
