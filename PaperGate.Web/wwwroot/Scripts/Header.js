function toggleMenu() {
  const menu = document.getElementById('mobileMenu');
  if (menu.classList.contains('translate-x-full')) {
    menu.classList.remove('translate-x-full');
    menu.classList.add('translate-x-0');
  } else {
    menu.classList.add('translate-x-full');
    menu.classList.remove('translate-x-0');
  }
}
