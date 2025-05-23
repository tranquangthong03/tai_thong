document.addEventListener('DOMContentLoaded', function() {
  // Navbar scroll effect
  const navbar = document.querySelector('.navbar');
  const navbarBrand = document.querySelector('.navbar-brand');
  
  if (navbar) {
    window.addEventListener('scroll', function() {
      if (window.scrollY > 50) {
        navbar.classList.add('scrolled');
      } else {
        navbar.classList.remove('scrolled');
      }
    });
    
    // Check scroll position on page load
    if (window.scrollY > 50) {
      navbar.classList.add('scrolled');
    }
  }

  // Add the collapsed class to the toggle button by default
  const navbarToggler = document.querySelector('.navbar-toggler');
  if (navbarToggler) {
    navbarToggler.classList.add('collapsed');
  }
  
  // Fix for dropdown menus on touch devices
  const dropdownMenus = document.querySelectorAll('.dropdown-menu');
  if (dropdownMenus) {
    dropdownMenus.forEach(menu => {
      menu.addEventListener('click', function(e) {
        e.stopPropagation();
      });
    });
  }
}); 