<template>
  <div class="portfolio">
    <Sidebar />
    <div class="main-content">
      <header class="portfolio-header">
        <h2>Portefeuille</h2>
        <p>Gérez vos actifs en cryptomonnaies</p>
      </header>

      <!-- Solde total et fonds disponibles -->
      <div class="portfolio-stats">
        <div class="card">
          <h3>Solde total</h3>
          <p>${{ totalBalance.toLocaleString() }}</p>
        </div>
        <div class="card">
          <h3>Fonds disponibles</h3>
          <p>${{ availableFunds.toLocaleString() }}</p>
        </div>
      </div>

      <!-- Liste des actifs -->
      <div class="portfolio-assets">
        <h3>Vos actifs</h3>
        <table>
          <thead>
            <tr>
              <th>Cryptomonnaie</th>
              <th>Quantité</th>
              <th>Valeur actuelle</th>
              <th>24h Change</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="asset in assets" :key="asset.id">
              <td>
                <div class="asset-info">
                  <img :src="asset.icon" :alt="asset.name" class="crypto-icon" />
                  <span>{{ asset.name }} ({{ asset.symbol }})</span>
                </div>
              </td>
              <td>{{ asset.amount.toFixed(6) }}</td>
              <td>${{ (asset.amount * asset.price).toLocaleString() }}</td>
              <td :class="asset.change >= 0 ? 'positive' : 'negative'">
                {{ asset.change >= 0 ? '+' : '' }}{{ asset.change.toFixed(2) }}%
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Historique des transactions -->
      <div class="portfolio-history">
        <h3>Historique des transactions</h3>
        <table>
          <thead>
            <tr>
              <th>Date</th>
              <th>Type</th>
              <th>Cryptomonnaie</th>
              <th>Quantité</th>
              <th>Montant (USD)</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="transaction in transactions" :key="transaction.id">
              <td>{{ formatDate(transaction.date) }}</td>
              <td :class="transaction.type === 'Achat' ? 'positive' : 'negative'">
                {{ transaction.type }}
              </td>
              <td>{{ transaction.crypto }}</td>
              <td>{{ transaction.amount.toFixed(6) }}</td>
              <td>${{ transaction.value.toLocaleString() }}</td>
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

// Données du portefeuille
const totalBalance = ref(15420.50); // Solde total en USD
const availableFunds = ref(5230.40); // Fonds disponibles en USD

// Liste des actifs détenus
const assets = ref([
  {
    id: 1,
    name: 'Bitcoin',
    symbol: 'BTC',
    icon: 'https://cryptologos.cc/logos/bitcoin-btc-logo.png',
    amount: 0.5, // Quantité détenue
    price: 45000, // Prix actuel en USD
    change: 2.4, // Changement sur 24h en %
  },
  {
    id: 2,
    name: 'Ethereum',
    symbol: 'ETH',
    icon: 'https://cryptologos.cc/logos/ethereum-eth-logo.png',
    amount: 4.2,
    price: 3200,
    change: -1.2,
  },
  {
    id: 3,
    name: 'Binance Coin',
    symbol: 'BNB',
    icon: 'https://cryptologos.cc/logos/bnb-bnb-logo.png',
    amount: 10,
    price: 420,
    change: 0.8,
  },
]);

// Historique des transactions
const transactions = ref([
  {
    id: 1,
    date: new Date('2024-03-10'),
    type: 'Achat',
    crypto: 'Bitcoin',
    amount: 0.1,
    value: 4500,
  },
  {
    id: 2,
    date: new Date('2024-03-09'),
    type: 'Vente',
    crypto: 'Ethereum',
    amount: 2.5,
    value: 8000,
  },
  {
    id: 3,
    date: new Date('2024-03-08'),
    type: 'Dépôt',
    crypto: 'USD',
    amount: 1000,
    value: 1000,
  },
]);

// Fonction pour formater la date
const formatDate = (date: Date) => {
  return new Intl.DateTimeFormat('fr-FR', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
  }).format(date);
};
</script>

<style scoped>

.portfolio {
  display: flex;
  min-height: 100vh;
}

.main-content {
  margin-left: 280px;
  flex: 1;
  padding: 2rem;
}

.portfolio-header {
  text-align: left; /* Alignement à gauche */
  margin-bottom: 2rem;
}

.portfolio-header h2 {
  font-size: 2rem;
  color: #2563eb; /* Bleu pour le titre */
  animation: fadeIn 1s ease-in-out; /* Animation d'apparition */
}

.portfolio-header p {
  font-size: 1rem;
  color: #6b7280; /* Gris doux */
}

.portfolio-stats {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 1.5rem;
  margin-bottom: 2rem;
}

.card {
  background: white;
  padding: 1.5rem;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.card h3 {
  margin-bottom: 0.5rem;
  font-size: 1.25rem;
}

.card p {
  font-size: 1.5rem;
  font-weight: bold;
}

.portfolio-assets,
.portfolio-history {
  margin-bottom: 2rem;
}

h3 {
  margin-bottom: 1rem;
}

table {
  width: 100%;
  border-collapse: collapse;
}

th, td {
  padding: 1rem;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

.asset-info {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.crypto-icon {
  width: 24px;
  height: 24px;
  object-fit: contain;
}

.positive {
  color: green;
}

.negative {
  color: red;
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
  }}
</style>