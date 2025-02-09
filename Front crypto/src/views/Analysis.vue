<template>
  <div class="analysis">
    <Sidebar />
    <div class="main-content">
      <header class="analysis-header">
        <h2>Analyse Crypto</h2>
        <p>Analysez les performances des cryptomonnaies</p>
      </header>

      <!-- Filtres -->
      <div class="filters">
        <div class="form-group">
          <label>Type d'analyse</label>
          <select v-model="selectedAnalysis">
            <option value="quartile">1er Quartile</option>
            <option value="max">Max</option>
            <option value="min">Min</option>
            <option value="average">Moyenne</option>
            <option value="stdDev">Écart-type</option>
          </select>
        </div>
        <div class="form-group">
          <label>Cryptomonnaie</label>
          <div class="crypto-checkboxes">
            <label>
              <input type="checkbox" v-model="selectAllCryptos" @change="toggleAllCryptos" /> Toutes
            </label>
            <label v-for="crypto in cryptos" :key="crypto">
              <input type="checkbox" v-model="selectedCryptos" :value="crypto" /> {{ crypto }}
            </label>
          </div>
        </div>
        <div class="form-group">
          <label>Date et heure min</label>
          <input type="datetime-local" v-model="minDateTime" />
        </div>
        <div class="form-group">
          <label>Date et heure max</label>
          <input type="datetime-local" v-model="maxDateTime" />
        </div>
        <button @click="applyFilters">Valider</button>
      </div>

      <!-- Résultats de l'analyse sous forme de tableau -->
      <div class="analysis-results">
        <h3>Résultats</h3>
        <table v-if="filteredData.length > 0">
          <thead>
            <tr>
              <th>Cryptomonnaie</th>
              <th>1er Quartile</th>
              <th>Max</th>
              <th>Min</th>
              <th>Moyenne</th>
              <th>Écart-type</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in filteredData" :key="item.crypto">
              <td>{{ item.crypto }}</td>
              <td>{{ item.quartile }}</td>
              <td>{{ item.max }}</td>
              <td>{{ item.min }}</td>
              <td>{{ item.average }}</td>
              <td>{{ item.stdDev }}</td>
            </tr>
          </tbody>
        </table>
        <p v-else>Aucune donnée disponible pour les filtres sélectionnés.</p>
      </div>

      <!-- Graphique en courbe -->
      <div class="chart-container">
        <LineChart :data="chartData" :options="chartOptions" />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { Line as LineChart } from 'vue-chartjs';
import { Chart as ChartJS, CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend } from 'chart.js';
import Sidebar from '../components/Sidebar.vue';

// Enregistrer les composants nécessaires pour le graphique en courbe
ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend);

const cryptos = ref(['Bitcoin', 'Ethereum', 'Binance Coin']);
const selectedAnalysis = ref<'quartile' | 'max' | 'min' | 'average' | 'stdDev'>('quartile');
const selectedCryptos = ref<string[]>([]);
const selectAllCryptos = ref(false);
const minDateTime = ref('');
const maxDateTime = ref('');

// Données de test
const cryptoData = ref([
  { crypto: 'Bitcoin', date: '2024-03-10T10:00:00', quartile: 44000, max: 48000, min: 42000, average: 45200, stdDev: 1200 },
  { crypto: 'Ethereum', date: '2024-03-10T12:00:00', quartile: 3100, max: 3300, min: 3000, average: 3200, stdDev: 100 },
  { crypto: 'Binance Coin', date: '2024-03-10T15:00:00', quartile: 400, max: 450, min: 380, average: 420, stdDev: 20 },
]);

// Sélectionner/désélectionner toutes les cryptos
const toggleAllCryptos = () => {
  if (selectAllCryptos.value) {
    selectedCryptos.value = [...cryptos.value];
  } else {
    selectedCryptos.value = [];
  }
};

// Appliquer les filtres
const applyFilters = () => {
  filteredData.value = cryptoData.value.filter((item) => {
    const dateMatch =
      (!minDateTime.value || new Date(item.date) >= new Date(minDateTime.value)) &&
      (!maxDateTime.value || new Date(item.date) <= new Date(maxDateTime.value));
    const cryptoMatch = selectedCryptos.value.length === 0 || selectedCryptos.value.includes(item.crypto);
    return dateMatch && cryptoMatch;
  });
};

// Données filtrées
const filteredData = ref(cryptoData.value);

// Données pour le graphique en courbe
const chartData = computed(() => {
  return {
    labels: filteredData.value.map((item) => item.crypto), // Cryptomonnaies en abscisse
    datasets: [
      {
        label: selectedAnalysis.value,
        data: filteredData.value.map((item) => item[selectedAnalysis.value]), // Valeurs en ordonnée
        borderColor: '#2563eb', // Couleur de la courbe
        backgroundColor: 'rgba(37, 99, 235, 0.1)', // Fond sous la courbe
        fill: true, // Remplir l'espace sous la courbe
        tension: 0.4, // Lissage de la courbe
      },
    ],
  };
});
const chartOptions = {
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: {
      position: 'bottom' as const, // Ajout de "as const" pour corriger l'erreur TypeScript
    },
  },
  scales: {
    y: {
      beginAtZero: false,
    },
  },
};

</script>


<style scoped>
.analysis {
  display: flex;
  min-height: 100vh;
  background-color: #f9fafb; /* Fond clair */
}

.main-content {
  margin-left: 280px; /* Espace pour la sidebar */
  flex: 1;
  padding: 2rem;
}

.analysis-header {
  margin-bottom: 2rem;
}

.analysis-header h2 {
  font-size: 2rem;
  color: #2563eb; /* Bleu pour le titre */
  animation: fadeIn 1s ease-in-out; /* Animation d'apparition */
}

.analysis-header p {
  font-size: 1rem;
  color: #6b7280; /* Gris doux */
}

.filters {
  display: flex;
  gap: 1rem;
  margin-bottom: 2rem;
  flex-wrap: wrap; /* Permettre le retour à la ligne sur petits écrans */
  animation: slideUp 0.8s ease-in-out; /* Animation de montée */
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

/* Styles des cases à cocher */
.crypto-checkboxes {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.crypto-checkboxes label {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  cursor: pointer;
  transition: transform 0.3s ease; /* Transition fluide */
}

.crypto-checkboxes label:hover {
  transform: scale(1.05); /* Légère augmentation de taille au survol */
}

.crypto-checkboxes input[type="checkbox"] {
  appearance: none;
  width: 18px;
  height: 18px;
  border: 2px solid var(--primary-color);
  border-radius: 4px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: background-color 0.2s ease, border-color 0.2s ease;
}

.crypto-checkboxes input[type="checkbox"]:checked {
  background-color: var(--primary-color);
  border-color: var(--primary-color);
}

.crypto-checkboxes input[type="checkbox"]::after {
  content: '✔';
  color: white;
  font-size: 14px;
  font-weight: bold;
  display: none;
}

.crypto-checkboxes input[type="checkbox"]:checked::after {
  display: block;
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

.analysis-results {
  background: white;
  padding: 2rem;
  border-radius: 8px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1); /* Ombre légère */
  margin-bottom: 2rem;
  animation: slideUp 0.8s ease-in-out; /* Animation de montée */
}

table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 1rem;
}

th,
td {
  padding: 1rem;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

.chart-container {
  background: white;
  padding: 2rem;
  border-radius: 8px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1); /* Ombre légère */
  margin-top: 2rem;
  height: 400px;
  animation: slideUp 0.8s ease-in-out; /* Animation de montée */
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
