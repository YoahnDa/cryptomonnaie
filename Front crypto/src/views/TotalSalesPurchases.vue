<template>
  <div class="total-sales-purchases">
    <Sidebar />
    <div class="main-content">
      <header class="total-sales-purchases-header">
        <h2>Somme Totale d'Achat et de Vente</h2>
        <p>Récapitulatif des transactions</p>
      </header>

      <!-- Filtre par date -->
      <div class="filters">
        <div class="form-group">
          <label>Date</label>
          <input type="date" v-model="selectedDate" />
        </div>
      </div>

      <!-- Tableau récapitulatif -->
      <div class="summary-table">
        <table>
          <thead>
            <tr>
              <th>Utilisateur</th>
              <th>Total Achats</th>
              <th>Total Ventes</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="user in filteredUsers" :key="user.id">
              <td>{{ user.name }}</td>
              <td>${{ user.totalPurchases.toLocaleString() }}</td>
              <td>${{ user.totalSales.toLocaleString() }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import Sidebar from '../components/Sidebar.vue';

// Données de test
const users = ref([
  { id: 1, name: 'Esther Howard', totalPurchases: 5000, totalSales: 3000 },
  { id: 2, name: 'John Doe', totalPurchases: 3000, totalSales: 2000 },
  { id: 3, name: 'Jane Smith', totalPurchases: 10000, totalSales: 5000 },
]);

const selectedDate = ref('');

// Utilisateurs filtrés
const filteredUsers = computed(() => {
  return users.value.filter((user) => {
    return !selectedDate.value || true; // Ajouter une logique de filtrage par date si nécessaire
  });
});
</script>

<style scoped>.total-sales-purchases {
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

.total-sales-purchases-header {
  text-align: center;
  margin-bottom: 2rem;
}

.total-sales-purchases-header h2 {
  font-size: 2rem;
  color: #2563eb; /* Bleu pour le titre */
  animation: fadeIn 1s ease-in-out; /* Animation d'apparition */
}

.total-sales-purchases-header p {
  font-size: 1rem;
  color: #6b7280; /* Gris doux */
}

.filters {
  margin-bottom: 2rem;
  max-width: 600px; /* Largeur maximale pour les grands écrans */
  width: 100%;
  margin: 0 auto; /* Centrer les filtres */
}

.form-group {
  margin-bottom: 1rem;
}

label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: bold;
  color: #374151; /* Bleu-gris foncé */
}

input {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 1rem;
  transition: border-color 0.3s ease, box-shadow 0.3s ease; /* Transition fluide */
}

input:focus {
  border-color: #2563eb; /* Bordure bleue au focus */
  box-shadow: 0 0 5px rgba(37, 99, 235, 0.3); /* Effet de focus */
  outline: none;
}

.summary-table {
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