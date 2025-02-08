<template>
    <div class="real-time">
      <Sidebar />
      <div class="main-content">
        <header class="real-time-header">
          <h2>Cours en temps réel</h2>
          <p>Suivez les prix des cryptomonnaies</p>
        </header>
        <div class="real-time-table">
          <table>
            <thead>
              <tr>
                <th>Cryptomonnaie</th>
                <th>Prix (USD)</th>
                <th>24h Change</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="crypto in cryptos" :key="crypto.id">
                <td>{{ crypto.name }}</td>
                <td>${{ crypto.price }}</td>
                <td :class="crypto.change >= 0 ? 'positive' : 'negative'">
                  {{ crypto.change }}%
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref } from 'vue';
  import Sidebar from '../components/Sidebar.vue';
  
  const cryptos = ref([
    { id: 1, name: 'Bitcoin', price: 45000, change: 2.4 },
    { id: 2, name: 'Ethereum', price: 3200, change: -1.2 },
    { id: 3, name: 'Binance Coin', price: 420, change: 0.8 },
  ]);
  </script>
  
  <style scoped>
  .real-time {
  display: flex;
  min-height: 100vh;
  background-color: #f9fafb; /* Fond clair */
}

.main-content {
  margin-left: 0; /* Plein écran : suppression de l'espace pour la sidebar */
  flex: 1;
  padding: 2rem;
  display: flex;
  flex-direction: column;
  align-items: center; /* Centrer horizontalement */
  justify-content: flex-start; /* Aligner le contenu en haut */
}

.real-time-header {
  text-align: center;
  margin-bottom: 2rem;
}

.real-time-header h2 {
  font-size: 2rem;
  color: #2563eb; /* Bleu pour le titre */
  animation: fadeIn 1s ease-in-out; /* Animation d'apparition */
}

.real-time-header p {
  font-size: 1rem;
  color: #6b7280; /* Gris doux */
}

.real-time-table {
  max-width: 1200px; /* Largeur maximale pour les grands écrans */
  width: 100%;
  margin: 0 auto; /* Centrer la table */
  background: white;
  padding: 2rem;
  border-radius: 10px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1); /* Ombre légère */
  animation: slideUp 0.8s ease-in-out; /* Animation de montée */
  overflow-x: auto; /* Permettre le défilement horizontal si nécessaire */
}

table {
  width: 100%;
  border-collapse: collapse;
}

th,
td {
  padding: 1rem;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

/* Hover effect for rows */
tbody tr:hover {
  background-color: #f3f4f6; /* Fond clair au survol */
  transform: scale(1.01); /* Légère augmentation de taille */
  transition: background-color 0.3s ease, transform 0.3s ease; /* Transition fluide */
}

.positive {
  color: #22c55e; /* Vert pour les variations positives */
}

.negative {
  color: #dc2626; /* Rouge pour les variations négatives */
}

/* ==============================
   ANIMATIONS
   ============================== */
@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

@keyframes slideUp {
  from {
    transform: translateY(20px);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}
  </style>