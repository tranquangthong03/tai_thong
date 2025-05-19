// Destination filtering logic
document.addEventListener('DOMContentLoaded', function() {
    // Get filter elements
    const countryFilter = document.getElementById('countryFilter');
    const ratingFilter = document.getElementById('ratingFilter');
    const popularFilter = document.getElementById('popularFilter');
    
    if (!countryFilter || !ratingFilter || !popularFilter) return;
    
    // Add event listeners
    countryFilter.addEventListener('change', applyFilters);
    ratingFilter.addEventListener('change', applyFilters);
    popularFilter.addEventListener('change', applyFilters);
    
    function applyFilters() {
        // Get current filter values
        const selectedCountry = countryFilter.value.toLowerCase();
        const selectedRating = parseInt(ratingFilter.value) || 0;
        const showPopularOnly = popularFilter.checked;
        
        // Get all destination cards
        const destinationCards = document.querySelectorAll('.destination-card');
        
        // Loop through each card and determine visibility
        destinationCards.forEach(card => {
            const country = card.dataset.country.toLowerCase();
            const rating = parseFloat(card.dataset.rating) || 0;
            const popularity = parseInt(card.dataset.popularity) || 0;
            
            // Default visible
            let isVisible = true;
            
            // Apply country filter if selected
            if (selectedCountry && country !== selectedCountry) {
                isVisible = false;
            }
            
            // Apply rating filter if selected
            if (selectedRating > 0 && rating < selectedRating) {
                isVisible = false;
            }
            
            // Apply popularity filter if checked
            if (showPopularOnly && popularity < 8) { // Assuming 8+ is popular on a scale of 10
                isVisible = false;
            }
            
            // Show/hide card
            card.closest('.col-md-4').style.display = isVisible ? 'block' : 'none';
        });
    }
});
