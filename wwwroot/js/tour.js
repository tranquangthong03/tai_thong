// Tour page JavaScript

document.addEventListener('DOMContentLoaded', function() {
    // Add animation order to tour cards
    document.querySelectorAll('.tour-card').forEach(function(card, index) {
        card.style.setProperty('--animation-order', index + 1);
    });

    // Initialize counters for stats
    animateCounters();
    
    // Initialize filters and sorting
    initializeFilters();
    
    // Add parallax effect to banner
    const tourBanner = document.querySelector('.tour-banner');
    if (tourBanner) {
        window.addEventListener('scroll', function() {
            const scrollPosition = window.scrollY;
            if (scrollPosition < 600) {
                tourBanner.style.backgroundPosition = `center ${scrollPosition * 0.5}px`;
            }
        });
    }
});

// Animate number counters
function animateCounters() {
    const counters = document.querySelectorAll('.counter-value');
    
    counters.forEach(counter => {
        const target = parseInt(counter.getAttribute('data-target'));
        const duration = 1500; // ms
        const stepTime = 50; // ms
        const totalSteps = duration / stepTime;
        const stepValue = target / totalSteps;
        let current = 0;
        
        const updateCounter = () => {
            current += stepValue;
            if (current < target) {
                counter.textContent = Math.ceil(current).toLocaleString();
                setTimeout(updateCounter, stepTime);
            } else {
                counter.textContent = target.toLocaleString();
            }
        };
        
        // Start animation when element is in viewport
        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    updateCounter();
                    observer.unobserve(entry.target);
                }
            });
        }, { threshold: 0.5 });
        
        observer.observe(counter);
    });
}

// Initialize filters and sorting
function initializeFilters() {
    const sortSelect = document.getElementById('tourSort');
    const filterPrice = document.getElementById('filterPrice');
    const filterDuration = document.getElementById('filterDuration');
    
    // Handle filter changes
    if (sortSelect) {
        sortSelect.addEventListener('change', applyFilters);
    }
    
    if (filterPrice) {
        filterPrice.addEventListener('change', applyFilters);
    }
    
    if (filterDuration) {
        filterDuration.addEventListener('change', applyFilters);
    }
    
    // Reset filters button
    const resetButton = document.querySelector('.reset-filters');
    if (resetButton) {
        resetButton.addEventListener('click', function(e) {
            e.preventDefault();
            if (sortSelect) sortSelect.value = 'default';
            if (filterPrice) filterPrice.value = 'all';
            if (filterDuration) filterDuration.value = 'all';
            document.getElementById('searchName').value = '';
            applyFilters();
        });
    }
    
    // Search form submit
    const searchForm = document.querySelector('.tour-search-form');
    if (searchForm) {
        searchForm.addEventListener('submit', function(e) {
            e.preventDefault();
            applyFilters();
        });
    }
}

// Apply filters and sorting
function applyFilters() {
    const searchTerm = document.getElementById('searchName').value.toLowerCase();
    const sortMethod = document.getElementById('tourSort')?.value || 'default';
    const priceFilter = document.getElementById('filterPrice')?.value || 'all';
    const durationFilter = document.getElementById('filterDuration')?.value || 'all';
    
    const tourCards = document.querySelectorAll('.tour-card-wrapper');
    let visibleCount = 0;
    
    // Reset animation order
    let animationOrder = 0;
    
    // Filter and sort tours
    tourCards.forEach(cardWrapper => {
        const card = cardWrapper.querySelector('.tour-card');
        const name = cardWrapper.dataset.name.toLowerCase();
        const price = parseInt(cardWrapper.dataset.price);
        const duration = parseInt(cardWrapper.dataset.duration || 0);
        
        // Apply filters
        let isVisible = name.includes(searchTerm);
        
        // Price filter
        if (isVisible && priceFilter !== 'all') {
            if (priceFilter === 'under1m' && price >= 1000000) isVisible = false;
            else if (priceFilter === '1m-3m' && (price < 1000000 || price > 3000000)) isVisible = false;
            else if (priceFilter === '3m-5m' && (price < 3000000 || price > 5000000)) isVisible = false;
            else if (priceFilter === '5m-10m' && (price < 5000000 || price > 10000000)) isVisible = false;
            else if (priceFilter === 'over10m' && price < 10000000) isVisible = false;
        }
        
        // Duration filter
        if (isVisible && durationFilter !== 'all') {
            if (durationFilter === '1-3days' && (duration < 1 || duration > 3)) isVisible = false;
            else if (durationFilter === '4-7days' && (duration < 4 || duration > 7)) isVisible = false;
            else if (durationFilter === '8-14days' && (duration < 8 || duration > 14)) isVisible = false;
            else if (durationFilter === 'over14days' && duration < 15) isVisible = false;
        }
        
        // Apply visibility
        if (isVisible) {
            cardWrapper.style.display = '';
            card.style.setProperty('--animation-order', ++animationOrder);
            visibleCount++;
        } else {
            cardWrapper.style.display = 'none';
        }
    });
    
    // Show empty state if no results
    const emptyState = document.querySelector('.tour-empty-state');
    const tourGrid = document.querySelector('.tour-grid');
    
    if (emptyState && tourGrid) {
        if (visibleCount === 0) {
            emptyState.style.display = '';
            tourGrid.style.display = 'none';
        } else {
            emptyState.style.display = 'none';
            tourGrid.style.display = '';
        }
    }
    
    // Apply sorting (after filtering)
    if (sortMethod !== 'default' && visibleCount > 0) {
        const tourContainer = document.querySelector('.tour-grid');
        const visibleCards = Array.from(tourCards).filter(card => card.style.display !== 'none');
        
        visibleCards.sort((a, b) => {
            const aPrice = parseInt(a.dataset.price);
            const bPrice = parseInt(b.dataset.price);
            const aName = a.dataset.name.toLowerCase();
            const bName = b.dataset.name.toLowerCase();
            const aPopular = a.dataset.popular === 'true';
            const bPopular = b.dataset.popular === 'true';
            
            if (sortMethod === 'price-asc') return aPrice - bPrice;
            else if (sortMethod === 'price-desc') return bPrice - aPrice;
            else if (sortMethod === 'name-asc') return aName.localeCompare(bName);
            else if (sortMethod === 'name-desc') return bName.localeCompare(aName);
            else if (sortMethod === 'popular') return bPopular - aPopular;
            return 0;
        });
        
        // Reappend elements in sorted order
        visibleCards.forEach(card => tourContainer.appendChild(card));
    }
}
