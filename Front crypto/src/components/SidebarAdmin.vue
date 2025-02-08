<template>
    <div class="sidebar-admin">
      <!-- Brand -->
      <div class="sidebar-brand">
        <h3>ADMIN</h3>
      </div>
  
      <!-- Navigation Links -->
      <nav class="sidebar-nav">
        <router-link to="/admin/adminValidation" class="nav-link" :class="{ active: isActive('/admin/validation') }">
          <i class="bi bi-check-circle"></i>
          <span>Validation</span>
        </router-link>
        <router-link to="/admin/adminUserHistory" class="nav-link" :class="{ active: isActive('/admin/user-history') }">
          <i class="bi bi-clock-history"></i>
          <span>Historique</span>
        </router-link>
      </nav>
  
      <!-- Bouton Déconnexion -->
      <div class="logout-section">
        <button @click="logout" class="logout-button">
          <i class="bi bi-box-arrow-right"></i>
          <span>Déconnexion</span>
        </button>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { useRouter } from 'vue-router';
  
  const router = useRouter();
  
  // Vérifie si le lien est actif
  const isActive = (path: string) => {
    return window.location.pathname === path;
  };
  
  // Déconnexion
  const logout = () => {
    // Effacer les données d'authentification (exemple : token)
    localStorage.removeItem('adminToken');
    // Rediriger vers la page de connexion
    router.push('/dashboard');
  };
  </script>
  
  <style scoped>
  .sidebar-admin {
    width: 280px;
    min-height: 100vh;
    position: fixed;
    background: var(--sidebar-bg);
    color: white;
    padding: 2rem;
    display: flex;
    flex-direction: column;
    justify-content: space-between; /* Espacer le contenu entre le haut et le bas */
  }
  
  .sidebar-brand h3 {
    background: linear-gradient(135deg, #ff4d4d, #c62828);
    -webkit-background-clip: text;
    color: transparent;
    font-weight: 700;
    letter-spacing: -1px;
    margin: 0;
  }
  
  .sidebar-nav {
    display: flex;
    flex-direction: column;
    gap: 0.5rem;
  }
  
  .nav-link {
    display: flex;
    align-items: center;
    gap: 1rem;
    padding: 1rem;
    color: rgba(255, 255, 255, 0.7);
    border-radius: 0.5rem;
    transition: all 0.3s ease;
    text-decoration: none;
  }
  
  .nav-link:hover,
  .nav-link.active {
    color: white;
    background: rgba(255, 255, 255, 0.1);
  }
  
  .nav-link i {
    font-size: 1.25rem;
  }
  
  .logout-section {
    text-align: center;
    margin-top: auto; /* Pousse le bouton de déconnexion en bas */
  }
  
  .logout-button {
    background: linear-gradient(135deg, #ff4d4d, #c62828);
    color: white;
    padding: 0.75rem 1.5rem;
    border: none;
    border-radius: 2rem;
    font-size: 0.9rem;
    font-weight: bold;
    cursor: pointer;
    transition: transform 0.3s ease, box-shadow 0.3s ease;
    display: inline-flex;
    align-items: center;
    gap: 0.5rem;
  }
  
  .logout-button:hover {
    transform: scale(1.1);
    box-shadow: 0 0 10px rgba(255, 77, 77, 0.5);
  }
  </style>