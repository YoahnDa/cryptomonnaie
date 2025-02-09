<template>
  <div class="real-time">
    <Sidebar />
    <div class="main-content">
      <!-- Market Table -->
      <div class="card fade-in" style="animation-delay: 0.5s">
        <div class="card-body">
          <div class="table-responsive">
            <table class="table">
              <thead>
                <tr>
                  <th>
                    <button class="btn btn-link text-dark p-0" @click="sortBy('rank')">#</button>
                  </th>
                  <th>
                    <button class="btn btn-link text-dark p-0" @click="sortBy('name')">Name</button>
                  </th>
                  <th>
                    <button class="btn btn-link text-dark p-0" @click="sortBy('price')">Price</button>
                  </th>
                  <th>
                    <button class="btn btn-link text-dark p-0" @click="sortBy('change')">24h Change</button>
                  </th>
                  <th>
                    <button class="btn btn-link text-dark p-0" @click="sortBy('volume')">24h Volume</button>
                  </th>
                  <th>
                    <button class="btn btn-link text-dark p-0" @click="sortBy('marketCap')">Market Cap</button>
                  </th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="crypto in filteredCryptos" :key="crypto.id">
                  <td>{{ crypto.rank }}</td>
                  <td>
                    <div class="d-flex align-items-center gap-2">
                      <img :src="crypto.icon" :alt="crypto.name" class="crypto-icon">
                      <div>
                        <div class="fw-medium">{{ crypto.name }}</div>
                        <div class="text-muted small">{{ crypto.symbol }}</div>
                      </div>
                    </div>
                  </td>
                  <td>${{ crypto.price.toFixed(2) }}</td>
                  <td :class="crypto.change >= 0 ? 'trend-up' : 'trend-down'">
                    {{ crypto.change >= 0 ? '+' : '' }}{{ crypto.change.toFixed(2) }}%
                  </td>
                  <td>${{ formatNumber(crypto.volume) }}</td>
                  <td>${{ formatNumber(crypto.marketCap) }}</td>
                  <td>
                    <button 
                      class="btn btn-sm"
                      :class="crypto.favorite ? 'btn-warning' : 'btn-outline-warning'"
                      @click="toggleFavorite(crypto)"
                    >
                      <i class="bi" :class="crypto.favorite ? 'bi-star-fill' : 'bi-star'"></i>
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { ref, computed } from 'vue';
import Sidebar from '../components/Sidebar.vue';

// Type pour un objet crypto
interface Crypto {
  id: number;
  rank: number;
  name: string;
  symbol: string;
  icon: string;
  price: number;
  change: number;
  volume: number;
  marketCap: number;
  favorite: boolean;
}

// Données des cryptos
const cryptos = ref<Crypto[]>([
  {
    id: 1,
    rank: 1,
    name: 'Bitcoin',
    symbol: 'BTC',
    icon: 'https://assets.coingecko.com/coins/images/1/large/bitcoin.png',
    price: 45000,
    change: 2.4,
    volume: 30000000,
    marketCap: 850000000000,
    favorite: false,
  },
  {
    id: 2,
    rank: 2,
    name: 'Ethereum',
    symbol: 'ETH',
    icon: 'https://assets.coingecko.com/coins/images/279/large/ethereum.png',
    price: 3200,
    change: -1.2,
    volume: 15000000,
    marketCap: 380000000000,
    favorite: false,
  },
  {
    id: 3,
    rank: 3,
    name: 'Binance Coin',
    symbol: 'BNB',
    icon: 'https://assets.coingecko.com/coins/images/825/large/binance-coin-logo.png',
    price: 420,
    change: 0.8,
    volume: 5000000,
    marketCap: 65000000000,
    favorite: false,
  },
]);

// État de tri
const sortKey = ref<string>('');
const sortOrder = ref<number>(1);

// Tri des cryptos
const filteredCryptos = computed(() => {
  return [...cryptos.value].sort((a, b) => {
    if (a[sortKey.value as keyof Crypto] < b[sortKey.value as keyof Crypto]) return -1 * sortOrder.value;
    if (a[sortKey.value as keyof Crypto] > b[sortKey.value as keyof Crypto]) return 1 * sortOrder.value;
    return 0;
  });
});

// Fonction de tri
const sortBy = (key: string): void => {
  if (sortKey.value === key) {
    sortOrder.value *= -1; // Inverser l'ordre si la même clé est cliquée
  } else {
    sortKey.value = key;
    sortOrder.value = 1; // Réinitialiser l'ordre
  }
};

// Ajouter/Retirer des favoris
const toggleFavorite = (crypto: Crypto): void => {
  crypto.favorite = !crypto.favorite;
};

// Formatage des grands nombres
const formatNumber = (value: number): string => {
  return new Intl.NumberFormat('en-US', { notation: 'compact' }).format(value);
};
</script>
<style scoped>
/* Styles principaux */
.real-time {
  display: flex;
  min-height: 100vh;
  background-color: #f9fafb; /* Fond clair */
}

.main-content {
  margin-left: 280px; /* Espace pour la sidebar */
  flex: 1;
  padding: 2rem;
  display: flex;
  flex-direction: column;
  align-items: center; /* Centrer horizontalement */
  justify-content: flex-start; /* Aligner le contenu en haut */
}

.card {
  max-width: 1200px;
  width: 100%;
  background: white;
  padding: 2rem;
  border-radius: 10px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1); /* Ombre légère */
  animation: slideUp 0.8s ease-in-out; /* Animation de montée */
}

.table-responsive {
  overflow-x: auto;
}

.table {
  width: 100%;
  border-collapse: collapse;
}

.table th,
.table td {
  padding: 1rem;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

.table th {
  cursor: pointer;
  user-select: none;
  font-weight: bold;
  color: #374151; /* Bleu-gris foncé */
}

.table tbody tr:hover {
  background-color: #f3f4f6; /* Fond clair au survol */
  transform: scale(1.01); /* Légère augmentation de taille */
  transition: background-color 0.3s ease, transform 0.3s ease; /* Transition fluide */
}

.crypto-icon {
  width: 24px;
  height: 24px;
  border-radius: 50%;
}

.trend-up {
  color: #22c55e; /* Vert pour les variations positives */
}

.trend-down {
  color: #dc2626; /* Rouge pour les variations négatives */
}

/* Boutons Favoris */
.btn-warning {
  background-color: #ffc107;
  border-color: #ffc107;
}

.btn-outline-warning {
  color: #ffc107;
  border-color: #ffc107;
}

.btn-outline-warning:hover {
  background-color: #ffc107;
  color: #fff;
}

/* Animations */
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