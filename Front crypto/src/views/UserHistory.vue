<template>
    <div class="user-history">
      <Sidebar />
      <div class="main-content">
        <header class="user-history-header">
          <h2>Histoire de {{ username }}</h2>
          <p>Liste des transactions effectuées par cet utilisateur</p>
        </header>
        <!-- Liste des transactions sous forme de cartes -->
        <div class="transaction-cards">
          <div v-if="userTransactions.length === 0" class="no-transactions">
            Aucune transaction disponible pour cet utilisateur.
          </div>
          <div v-for="transaction in userTransactions" :key="transaction.id" class="transaction-card">
            <div class="card-header">
              <span class="date">{{ transaction.date }}</span>
              <span :class="transaction.type === 'Achat' ? 'positive' : 'negative'">
                {{ transaction.type }}
              </span>
            </div>
            <div class="card-body">
              <p><strong>Cryptomonnaie :</strong> {{ transaction.crypto }}</p>
              <p><strong>Montant :</strong> ${{ transaction.amount.toLocaleString() }}</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </template>


<script setup lang="ts">
import { ref, computed } from 'vue';
import { useRoute } from 'vue-router';
import Sidebar from '../components/Sidebar.vue';


const route = useRoute();
const username = ref(route.query.username);

// Données de test
const transactions = ref([
  { id: 1, date: '2024-03-10', user: 'Esther Howard', crypto: 'Bitcoin', type: 'Achat', amount: 500 },
  { id: 2, date: '2024-03-09', user: 'John Doe', crypto: 'Ethereum', type: 'Vente', amount: 300 },
  { id: 3, date: '2024-03-08', user: 'Jane Smith', crypto: 'Binance Coin', type: 'Achat', amount: 1000 },
]);

// Filtrer les transactions pour l'utilisateur sélectionné
const userTransactions = computed(() => {
  return transactions.value.filter((transaction) => transaction.user === username.value);
});
</script>
<style scoped>
.user-history {
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

.user-history-header {
  text-align: center;
  margin-bottom: 2rem;
}

.transaction-cards {
  width: 100%;
  max-width: 1200px; /* Largeur maximale pour les grands écrans */
  margin: 0 auto; /* Centrer la liste */
  display: flex;
  flex-direction: column;
  gap: 1rem; /* Espacement entre les cartes */
}

.transaction-card {
  background: white;
  border-radius: 8px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1); /* Ombre légère */
  padding: 1rem;
  transition: transform 0.3s ease, box-shadow 0.3s ease; /* Transition fluide */
}

.transaction-card:hover {
  transform: translateY(-5px); /* Effet de levitation au survol */
  box-shadow: 0 6px 12px rgba(0, 0, 0, 0.15); /* Ombre plus prononcée */
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.5rem;
}

.card-header .date {
  font-size: 0.875rem;
  color: #6b7280; /* Gris doux */
}

.card-header .positive {
  color: #22c55e; /* Vert pour les achats */
  font-weight: bold;
}

.card-header .negative {
  color: #dc2626; /* Rouge pour les ventes */
  font-weight: bold;
}

.card-body p {
  margin: 0.25rem 0;
  font-size: 0.875rem;
  color: #374151; /* Bleu-gris foncé */
}

.no-transactions {
  text-align: center;
  color: #6b7280; /* Gris doux */
  font-size: 1rem;
}
</style>