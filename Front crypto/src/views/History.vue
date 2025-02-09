<template>
  <div class="history">
    <Sidebar />
    <div class="main-content">
      <header class="history-header">
        <h2>Historique des Transactions</h2>
        <p>Liste des achats et ventes passés</p>
      </header>

      <!-- Filtres multicritères -->
      <div class="filters">
        <div class="form-group">
          <label>Date</label>
          <input type="date" v-model="filters.date" />
        </div>
        <div class="form-group">
          <label>Utilisateur</label>
          <input type="text" v-model="filters.user" placeholder="Rechercher par utilisateur" />
        </div>
        <div class="form-group">
          <label>Cryptomonnaie</label>
          <select v-model="filters.crypto">
            <option value="">Toutes</option>
            <option v-for="crypto in cryptos" :key="crypto" :value="crypto">{{ crypto }}</option>
          </select>
        </div>
        <button class="btn-validate">Valider</button>
      </div>

      <!-- Liste des transactions -->
      <div class="transaction-list">
        <table>
          <thead>
            <tr>
              <th>Date</th>
              <th>Utilisateur</th>
              <th>Cryptomonnaie</th>
              <th>Type</th>
              <th>Montant</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="transaction in filteredTransactions" :key="transaction.id">
              <td>{{ transaction.date }}</td>
              <td>
                <div class="user-info">
                  <img :src="transaction.userImage" :alt="transaction.user" class="user-image" />
                  <span>{{ transaction.user }}</span>
                </div>
              </td>
              <td>{{ transaction.crypto }}</td>
              <td :class="transaction.type === 'Achat' ? 'positive' : 'negative'">
                {{ transaction.type }}
              </td>
              <td>${{ transaction.amount.toLocaleString() }}</td>
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
const transactions = ref([
  { id: 1, date: '2024-03-10', user: 'Esther Howard', userImage: '../assets/user.jpeg', crypto: 'Bitcoin', type: 'Achat', amount: 500 },
  { id: 2, date: '2024-03-09', user: 'John Doe', userImage: 'https://via.placeholder.com/40', crypto: 'Ethereum', type: 'Vente', amount: 300 },
  { id: 3, date: '2024-03-08', user: 'Jane Smith', userImage: 'https://via.placeholder.com/40', crypto: 'Binance Coin', type: 'Achat', amount: 1000 },
]);

const cryptos = ref(['Bitcoin', 'Ethereum', 'Binance Coin']);

// Filtres
const filters = ref({
  date: '',
  user: '',
  crypto: '',
});

// Transactions filtrées
const filteredTransactions = computed(() => {
  return transactions.value.filter((transaction) => {
    return (
      (!filters.value.date || transaction.date === filters.value.date) &&
      (!filters.value.user || transaction.user.toLowerCase().includes(filters.value.user.toLowerCase())) &&
      (!filters.value.crypto || transaction.crypto === filters.value.crypto)
    );
  });
});
</script>

<style scoped>
.history {
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

.history-header {
  text-align: center;
  margin-bottom: 2rem;
}

button {
  padding: 0.75rem 1.5rem;
  background-color: #2563eb;
  color: white;
  border: none;
  border-radius: 6px;
  font-size: 1rem;
  font-weight: bold;
  cursor: pointer;
  transition: transform 0.3s ease, background-color 0.3s ease; /* Animation de clic */
}

button:hover {
  background-color: #1e40af; /* Bleu plus foncé au survol */
  transform: scale(1.05); /* Légère augmentation de taille */
}

.history-header h2 {
  font-size: 2rem;
  color: #2563eb; /* Bleu pour le titre */
  animation: fadeIn 1s ease-in-out; /* Animation d'apparition */
}

.history-header p {
  font-size: 1rem;
  color: #6b7280; /* Gris doux */
}

.filters {
  display: flex;
  gap: 1rem;
  width: 100%;
  max-width: 1200px; /* Largeur maximale pour les grands écrans */
  margin: 0 auto; /* Centrer les filtres */
  margin-bottom: 2rem;
  animation: slideUp 0.8s ease-in-out; /* Animation de montée */
  flex-wrap: wrap; /* Permettre le retour à la ligne sur petits écrans */
}

.form-group {
  flex: 1;
  min-width: 200px; /* Largeur minimale pour éviter le chevauchement */
}

label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: bold;
  color: #374151; /* Bleu-gris foncé */
}

input,
select {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 1rem;
  transition: border-color 0.3s ease, box-shadow 0.3s ease; /* Transition fluide */
}

input:focus,
select:focus {
  border-color: #2563eb; /* Bordure bleue au focus */
  box-shadow: 0 0 5px rgba(37, 99, 235, 0.3); /* Effet de focus */
  outline: none;
}

.transaction-list {
  width: 100%;
  max-width: 1200px; /* Largeur maximale pour les grands écrans */
  margin: 0 auto; /* Centrer la liste */
  background: white;
  padding: 2rem;
  border-radius: 8px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1); /* Ombre légère */
  overflow-x: auto;
  animation: slideUp 0.8s ease-in-out; /* Animation de montée */
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

.user-info {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.user-image {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  object-fit: cover;
  transition: transform 0.3s ease; /* Transition fluide */
}

.user-image:hover {
  transform: scale(1.1); /* Effet de zoom au survol */
}

.positive {
  color: #22c55e; /* Vert pour les achats */
}

.negative {
  color: #dc2626; /* Rouge pour les ventes */
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