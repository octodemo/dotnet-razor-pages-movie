// Smooth scroll for anchor links
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
  anchor.addEventListener('click', function (e) {
    e.preventDefault();
    document.querySelector(this.getAttribute('href')).scrollIntoView({
      behavior: 'smooth'
    });
  });
});

// Add a fade-in effect to the body
document.addEventListener('DOMContentLoaded', () => {
  document.body.style.opacity = 0;
  setTimeout(() => {
    document.body.style.transition = 'opacity 1s';
    document.body.style.opacity = 1;
  }, 100);

  // Initialize tooltips
  document.querySelectorAll('[data-tooltip]').forEach(elem => {
    elem.addEventListener('mouseenter', function () {
      const tooltip = document.createElement('div');
      tooltip.className = 'tooltip';
      tooltip.innerText = this.getAttribute('data-tooltip');
      document.body.appendChild(tooltip);
      const rect = this.getBoundingClientRect();
      tooltip.style.left = `${rect.left + window.scrollX}px`;
      tooltip.style.top = `${rect.top + window.scrollY - tooltip.offsetHeight}px`;
    });
    elem.addEventListener('mouseleave', function () {
      document.querySelector('.tooltip').remove();
    });
  });

  // Add back-to-top button
  const backToTop = document.createElement('button');
  backToTop.innerText = '↑';
  backToTop.className = 'back-to-top';
  document.body.appendChild(backToTop);
  backToTop.addEventListener('click', () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  });
  window.addEventListener('scroll', () => {
    backToTop.style.display = window.scrollY > 200 ? 'block' : 'none';
  });

  // Add modal popup with less intrusive behavior
  if (!localStorage.getItem('hideWelcomePopup')) {
    const modal = document.createElement('div');
    modal.className = 'modal';
    modal.innerHTML = `
      <div class="modal-content">
        <span class="close">&times;</span>
        <p>Welcome to the site!</p>
        <button id="dontShowAgain">Don't show again</button>
      </div>
    `;
    document.body.appendChild(modal);
    const closeModal = () => modal.style.display = 'none';
    document.querySelector('.modal .close').addEventListener('click', closeModal);
    document.getElementById('dontShowAgain').addEventListener('click', () => {
      localStorage.setItem('hideWelcomePopup', 'true');
      closeModal();
    });
    window.addEventListener('click', (e) => {
      if (e.target === modal) closeModal();
    });
    setTimeout(() => modal.style.display = 'block', 5000); // Show after 5 seconds
  }
});