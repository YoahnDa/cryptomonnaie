<template>
  <div class="admin-user-history">
    <SidebarAdmin />
    <div class="main-content fade-in">
      <header class="admin-user-history-header">
        <h2>Historique des Opérations</h2>
        <p>Consultez les transactions des utilisateurs</p>
      </header>

      <!-- Filtres -->
      <div class="filters">
        <div class="form-group">
          <label>Date</label>
          <input type="date" v-model="selectedDate" />
        </div>
        <div class="form-group">
          <label>Cryptomonnaie</label>
          <select v-model="selectedCrypto">
            <option value="">Toutes</option>
            <option v-for="crypto in cryptos" :key="crypto" :value="crypto">{{ crypto }}</option>
          </select>
        </div>
        <button class="validate-button" @click="applyFilters">Valider</button>
      </div>

      <!-- Liste des transactions -->
      <div class="transactions-list">
        <table>
          <thead>
            <tr>
              <th>Utilisateur</th>
              <th>Type</th>
              <th>Cryptomonnaie</th>
              <th>Montant</th>
              <th>Date</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="transaction in filteredTransactions" :key="transaction.id">
              <td>{{ transaction.user }}</td>
              <td>{{ transaction.type }}</td>
              <td>{{ transaction.crypto }}</td>
              <td>${{ transaction.amount.toLocaleString() }}</td>
              <td>{{ transaction.date }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>


<script setup lang="ts">
import { ref, computed } from 'vue';
import SidebarAdmin from '../../components/SidebarAdmin.vue';

// Données de test
const transactions = ref([
  { id: 1, user: 'Esther Howard', type: 'Achat', crypto: 'Bitcoin', amount: 500, date: '2024-03-10' },
  { id: 2, user: 'John Doe', type: 'Vente', crypto: 'Ethereum', amount: 300, date: '2024-03-09' },
  { id: 3, user: 'Jane Smith', type: 'Dépôt', crypto: 'USD', amount: 1000, date: '2024-03-08' },
]);

const cryptos = ref(['Bitcoin', 'Ethereum', 'Binance Coin']);

const selectedDate = ref('');
const selectedCrypto = ref('');

// Transactions filtrées
const filteredTransactions = computed(() => {
  return transactions.value.filter(
    (transaction) =>
      (!selectedDate.value || transaction.date === selectedDate.value) &&
      (!selectedCrypto.value || transaction.crypto === selectedCrypto.value)
  );
});

// Méthode pour appliquer les filtres
const applyFilters = () => {
  console.log('Filtres appliqués:', { selectedDate: selectedDate.value, selectedCrypto: selectedCrypto.value });
};
</script>
<style scoped>
/* Animation d'entrée */
.fade-in {
  animation: fadeIn 0.5s ease-in-out;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(-10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.admin-user-history {
  display: flex;
  min-height: 100vh;
  font-family: 'Arial', sans-serif;
  background-color: #f9fafb; /* Couleur de fond douce */
}

.main-content {
  margin-left: 280px;
  flex: 1;
  padding: 2rem;
  transition: margin-left 0.3s ease-in-out; /* Animation pour le décalage latéral */
}

.admin-user-history-header {
  margin-bottom: 2rem;
  text-align: center;
}

.admin-user-history-header h2 {
  font-size: 2rem;
  color: #1e40af; /* Bleu profond */
  margin-bottom: 0.5rem;
  transition: color 0.3s ease-in-out; /* Changement de couleur au survol */
}

.admin-user-history-header h2:hover {
  color: #1e3a8a; /* Bleu plus foncé */
}

.admin-user-history-header p {
  font-size: 1rem;
  color: #6b7280; /* Gris neutre */
}

.filters {
  display: flex;
  gap: 1rem;
  margin-bottom: 2rem;
  align-items: flex-end;
}

.form-group {
  flex: 1;
}

label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: bold;
  color: #374151; /* Gris foncé */
}

input,
select {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #d1d5db; /* Gris clair */
  border-radius: 6px;
  transition: border-color 0.3s ease-in-out, box-shadow 0.3s ease-in-out; /* Effet de focus */
}

input:focus,
select:focus {
  border-color: #3b82f6; /* Bleu vibrant */
  box-shadow: 0 0 8px rgba(59, 130, 246, 0.2); /* Ombre légère */
  outline: none;
}

.validate-button {
  padding: 0.75rem 1.5rem;
  background-color: #3b82f6; /* Bleu vibrant */
  color: #fff;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  transition: background-color 0.3s ease-in-out, transform 0.2s ease-in-out;
}

.validate-button:hover {
  background-color: #2563eb; /* Bleu plus foncé */
  transform: scale(1.05);
}

.transactions-list {
  overflow-x: auto;
  border-radius: 8px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1); /* Ombre subtile */
  background-color: #fff;
  transition: box-shadow 0.3s ease-in-out; /* Animation pour l'ombre */
}

.transactions-list:hover {
  box-shadow: 0 6px 12px rgba(0, 0, 0, 0.15);
}

table {
  width: 100%;
  border-collapse: collapse;
}

th,
td {
  padding: 1rem;
  text-align: left;
  border-bottom: 1px solid #e5e7eb; /* Gris très clair */
  transition: background-color 0.3s ease-in-out; /* Changement de couleur au survol */
}

th {
  background-color: #f3f4f6; /* Gris très clair */
  color: #374151; /* Gris foncé */
  font-weight: bold;
}

tr:hover td {
  background-color: #f9fafb; /* Fond légèrement plus clair */
}
</style>