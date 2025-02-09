<template>
    <div class="sidebar" :class="{ 'collapsed': isCollapsed }">
      <!-- Header avec le logo et le bouton Administrateur -->
      <header class="sidebar-header">
        <div class="sidebar-brand">
          <h3>e-CRYPTO</h3>
        </div>
      </header>
  
      <!-- Profil utilisateur -->
      <div class="user-profile">
        <div class="profile-image">
          <img src="../assets/user.jpeg" alt="Profile">
        </div>
        <h6 v-if="!isCollapsed">Esther Howard</h6>
        <router-link to="/profile" class="badge" v-if="!isCollapsed">Modifier le profil</router-link>
      </div>
  
      <!-- Navigation -->
      <nav class="sidebar-nav">
        <router-link
          v-for="link in links"
          :key="link.path"
          :to="link.path"
          class="nav-link"
          :class="{ active: isActive(link.path) }"
        >
          <i :class="link.icon"></i>
          <span v-if="!isCollapsed">{{ link.label }}</span>
        </router-link>
      </nav>
      <!-- Bouton Administrateur -->
    <div class="admin-section">
      <router-link to="/admin/login" class="admin-button">
        Administrateur
      </router-link>
    </div>
    </div>

  </template>
<script setup lang="ts">
import { ref, computed } from 'vue';
import { useRoute } from 'vue-router';

const links = ref([
  { path: '/dashboard', label: 'Tableau de bord', icon: 'bi bi-grid' },
  { path: '/buy', label: 'Achat', icon: 'bi bi-cart-plus' },
  { path: '/sell', label: 'Vente', icon: 'bi bi-cart-dash' },
  { path: '/deposit', label: 'Dépôt', icon: 'bi bi-cash-coin' },
  { path: '/withdraw', label: 'Retrait', icon: 'bi bi-bank' },
  { path: '/real-time', label: 'Cours en temps réel', icon: 'bi bi-graph-up' },
  { path: '/analysis', label: 'Analyse crypto', icon: 'bi bi-bar-chart' },
  { path: '/commission-analysis', label: 'Analyse commissions', icon: 'bi bi-percent' },
  { path: '/total-sales-purchases', label: 'Somme totale d’achat et de vente', icon: 'bi bi-bank' },
]);

const route = useRoute();
const isCollapsed = ref(false);

const isActive = (path: string) => {
  return route.path.startsWith(path);
};

const toggleSidebar = () => {
  isCollapsed.value = !isCollapsed.value;
};
</script>


<style scoped>
/* ==============================
   1. CONTENEUR PRINCIPAL
   ============================== */
   .sidebar {
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

.sidebar.collapsed {
  width: 80px;
}

/* ==============================
   2. EN-TÊTE (HEADER)
   ============================== */
.sidebar-header {
  display: flex;
  align-items: center;
  padding-bottom: 1rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

.sidebar-brand h3 {
  background: linear-gradient(135deg, #2563eb, #1e40af);
  -webkit-background-clip: text;
  color: transparent;
  font-weight: 700;
  letter-spacing: -1px;
  margin: 0;
}

/* ==============================
   3. PROFIL UTILISATEUR
   ============================== */
.user-profile {
  text-align: center;
  padding: 1.5rem 0;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

.profile-image {
  width: 80px;
  height: 80px;
  margin: 0 auto 1rem;
  border-radius: 50%;
  padding: 3px;
  background: linear-gradient(135deg, #2563eb, #1e40af);
}

.profile-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 50%;
  border: 3px solid var(--sidebar-bg);
}

.badge {
  background: linear-gradient(135deg, #2563eb, #1e40af);
  padding: 0.25rem 1rem;
  border-radius: 1rem;
  font-size: 0.75rem;
  color: white;
  text-decoration: none;
  display: inline-block;
  margin-top: 0.5rem;
}

/* ==============================
   4. NAVIGATION
   ============================== */
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

.sidebar.collapsed .nav-link span {
  display: none;
}

.sidebar.collapsed .nav-link i {
  margin: 0 auto;
}

/* ==============================
   5. SECTION SOMME TOTALE
   ============================== */
.total-section {
  margin-top: auto; /* Pousse cette section vers le bas */
  padding: 1rem 0;
  border-top: 1px solid rgba(255, 255, 255, 0.1);
}

.total-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.5rem;
  font-size: 0.9rem;
}

.total-item strong {
  font-weight: bold;
  color: #2563eb;
}

/* ==============================
   6. BOUTON ADMINISTRATEUR
   ============================== */
.admin-section {
  text-align: center;
  margin-top: 1rem;
}

.admin-button {
  background: linear-gradient(135deg, #ff4d4d, #c62828); /* Rouge vibrant */
  color: white;
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 2rem;
  font-size: 0.9rem;
  font-weight: bold;
  text-decoration: none;
  cursor: pointer;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
  display: inline-block;
}

.admin-button:hover {
  transform: scale(1.1); /* Animation d'agrandissement */
  box-shadow: 0 0 10px rgba(255, 77, 77, 0.5); /* Ombre rouge */
}

.sidebar.collapsed .admin-button {
  display: none; /* Masquer le bouton en mode réduit */
}
</style>