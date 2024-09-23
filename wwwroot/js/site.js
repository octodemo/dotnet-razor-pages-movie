document.addEventListener('DOMContentLoaded', () => {
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
      setTimeout(() => tooltip.style.opacity = 1, 0); // Fade-in effect
    });
    elem.addEventListener('mouseleave', function () {
      const tooltip = document.querySelector('.tooltip');
      tooltip.style.opacity = 0;
      setTimeout(() => tooltip.remove(), 300); // Wait for fade-out
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
    if (window.scrollY > 200) {
      backToTop.classList.add('show');
    } else {
      backToTop.classList.remove('show');
    }
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
    setTimeout(() => modal.style.display = 'flex', 5000); // Show after 5 seconds
  }

  // Add scroll progress bar
  const scrollProgress = document.createElement('div');
  scrollProgress.id = 'scrollProgress';
  document.body.appendChild(scrollProgress);
  window.addEventListener('scroll', () => {
    const scrollTop = window.scrollY;
    const docHeight = document.documentElement.scrollHeight - window.innerHeight;
    const scrollPercent = (scrollTop / docHeight) * 100;
    scrollProgress.style.width = `${scrollPercent}%`;
  });

  // Add flip effect to movie cards
  document.querySelectorAll('.movie-card').forEach(card => {
    card.addEventListener('click', () => {
      card.classList.toggle('flipped');
    });
  });
});