<template>
  <div class="admin-validation">
    <SidebarAdmin />
    <div class="main-content fade-in">
      <header class="admin-validation-header">
        <h2>Validation des Dépôts et Retraits</h2>
        <p>Validez ou rejetez les demandes des utilisateurs</p>
      </header>

      <!-- Liste des demandes en attente -->
      <div class="requests-list">
        <table>
          <thead>
            <tr>
              <th>Utilisateur</th>
              <th>Type</th>
              <th>Montant</th>
              <th>Date</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="request in pendingRequests" :key="request.id" class="request-row">
              <td>{{ request.user }}</td>
              <td>{{ request.type }}</td>
              <td>${{ request.amount.toLocaleString() }}</td>
              <td>{{ request.date }}</td>
              <td>
                <button @click="approveRequest(request.id)" class="btn-approve">Approuver</button>
                <button @click="rejectRequest(request.id)" class="btn-reject">Rejeter</button>
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
import SidebarAdmin from '../../components/SidebarAdmin.vue';

// Données de test
const pendingRequests = ref([
  { id: 1, user: 'Esther Howard', type: 'Dépôt', amount: 1000, date: '2024-03-10' },
  { id: 2, user: 'John Doe', type: 'Retrait', amount: 500, date: '2024-03-09' },
  { id: 3, user: 'Jane Smith', type: 'Dépôt', amount: 2000, date: '2024-03-08' },
]);

// Approuver une demande
const approveRequest = (id: number) => {
  const request = pendingRequests.value.find((req) => req.id === id);
  if (request) {
    alert(`Demande ${request.type} de ${request.user} approuvée. Notification envoyée.`);
    pendingRequests.value = pendingRequests.value.filter((req) => req.id !== id);
  }
};

// Rejeter une demande
const rejectRequest = (id: number) => {
  const request = pendingRequests.value.find((req) => req.id === id);
  if (request) {
    alert(`Demande ${request.type} de ${request.user} rejetée.`);
    pendingRequests.value = pendingRequests.value.filter((req) => req.id !== id);
  }
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

.admin-validation {
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

.admin-validation-header {
  margin-bottom: 2rem;
  text-align: center;
}

.admin-validation-header h2 {
  font-size: 2rem;
  color: #1e40af; /* Bleu profond */
  margin-bottom: 0.5rem;
  transition: color 0.3s ease-in-out; /* Changement de couleur au survol */
}

.admin-validation-header h2:hover {
  color: #1e3a8a; /* Bleu plus foncé */
}

.admin-validation-header p {
  font-size: 1rem;
  color: #6b7280; /* Gris neutre */
}

.requests-list {
  overflow-x: auto;
  border-radius: 8px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1); /* Ombre subtile */
  background-color: #fff;
  transition: box-shadow 0.3s ease-in-out; /* Animation pour l'ombre */
}

.requests-list:hover {
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

.request-row:hover td {
  background-color: #f9fafb; /* Fond légèrement plus clair */
}

.btn-approve,
.btn-reject {
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  transition: background-color 0.3s ease-in-out, transform 0.2s ease-in-out;
  font-weight: bold;
}

.btn-approve {
  background-color: #10b981; /* Vert vibrant */
  color: #fff;
}

.btn-approve:hover {
  background-color: #059669; /* Vert plus foncé */
  transform: scale(1.05);
}

.btn-reject {
  background-color: #ef4444; /* Rouge vibrant */
  color: #fff;
}

.btn-reject:hover {
  background-color: #dc2626; /* Rouge plus foncé */
  transform: scale(1.05);
}
</style>