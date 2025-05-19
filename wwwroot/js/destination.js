// Destination Page JavaScript
document.addEventListener('DOMContentLoaded', function() {
    // Thêm hiệu ứng cho các destination-card khi scroll
    const destinationCards = document.querySelectorAll('.destination-card');
    
    // Thêm hiệu ứng hover cho destination-card
    destinationCards.forEach(card => {
        // Lấy hình ảnh trong card
        const image = card.querySelector('.card-img-top');
        const badge = card.querySelector('.badge');
        const adminButtons = card.querySelector('.admin-buttons');
        
        // Hiệu ứng khi hover
        card.addEventListener('mouseenter', function() {
            if (image) {
                image.style.transform = 'scale(1.1)';
            }
            if (badge) {
                badge.classList.add('bg-white');
                badge.classList.add('text-primary');
                badge.classList.remove('bg-light');
            }
            if (adminButtons) {
                adminButtons.style.opacity = '1';
            }
        });
        
        // Hiệu ứng khi bỏ hover
        card.addEventListener('mouseleave', function() {
            if (image) {
                image.style.transform = 'scale(1)';
            }
            if (badge) {
                badge.classList.remove('bg-white');
                badge.classList.remove('text-primary');
                badge.classList.add('bg-light');
            }
            if (adminButtons) {
                adminButtons.style.opacity = '0';
            }
        });
    });

    // Hiệu ứng đếm số
    const statValues = document.querySelectorAll('.stat-value');
    statValues.forEach(stat => {
        const finalValue = parseInt(stat.textContent);
        let startValue = 0;
        const duration = 2000; // 2 giây
        const frameDuration = 1000 / 60; // 60fps
        const totalFrames = Math.round(duration / frameDuration);
        const valueIncrement = finalValue / totalFrames;
        
        const counter = setInterval(() => {
            startValue += valueIncrement;
            
            if (startValue >= finalValue) {
                stat.textContent = finalValue;
                clearInterval(counter);
                return;
            }
            
            stat.textContent = Math.floor(startValue);
        }, frameDuration);
    });
    
    // Hiệu ứng lọc destination card
    const filterInputs = document.querySelectorAll('#countryFilter, #ratingFilter');
    const filterCheckbox = document.querySelector('#popularFilter');
    const searchInput = document.querySelector('#searchDestination');
    
    if (filterInputs.length && filterCheckbox && searchInput) {
        // Xử lý filter đã được triển khai trong phần jQuery của view
    }

    // Hiệu ứng cho banner
    const banner = document.querySelector('.destination-banner');
    if (banner) {
        window.addEventListener('scroll', function() {
            const scrollPosition = window.scrollY;
            const bannerHeight = banner.offsetHeight;
            
            if (scrollPosition < bannerHeight) {
                const yPos = scrollPosition * 0.5;
                banner.style.backgroundPosition = `center -${yPos}px`;
            }
        });
    }
});
