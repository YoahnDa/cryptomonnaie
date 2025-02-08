<template>
    <div class="withdraw">
      <Sidebar />
      <div class="main-content">
        <header class="withdraw-header">
          <h2>Retrait</h2>
          <p>Retirez des fonds de votre compte</p>
        </header>
        <div class="withdraw-form">
          <form @submit.prevent="handleWithdraw">
            <div class="form-group">
              <label>Montant (USD)</label>
              <input type="number" v-model="amount" min="0" step="0.01" required />
            </div>
            <button type="submit">Retirer</button>
          </form>
        </div>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref } from 'vue';
  import Sidebar from '../components/Sidebar.vue';
  
  const amount = ref(0);
  
  const handleWithdraw = () => {
    alert(`Retrait de ${amount.value} USD effectué`);
  };
  </script>
  
  <style scoped>
  .withdraw {
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
  justify-content: center; /* Centrer verticalement */
}

.withdraw-header {
  text-align: center;
  margin-bottom: 2rem;
}

.withdraw-header h2 {
  font-size: 2rem;
  color: #dc2626; /* Rouge pour "Retrait" */
  animation: fadeIn 1s ease-in-out; /* Animation d'apparition */
}

.withdraw-header p {
  font-size: 1rem;
  color: #6b7280; /* Gris doux */
}

.withdraw-form {
  max-width: 400px;
  width: 100%;
  background: white;
  padding: 2rem;
  border-radius: 10px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1); /* Ombre légère */
  animation: slideUp 0.8s ease-in-out; /* Animation de montée */
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
  border-color: #dc2626; /* Bordure rouge au focus */
  box-shadow: 0 0 5px rgba(220, 38, 38, 0.3); /* Effet de focus */
  outline: none;
}

button {
  width: 100%;
  padding: 0.75rem;
  background-color: #dc2626; /* Rouge pour le bouton "Retirer" */
  color: white;
  border: none;
  border-radius: 6px;
  font-size: 1rem;
  font-weight: bold;
  cursor: pointer;
  transition: transform 0.3s ease, background-color 0.3s ease; /* Animation de clic */
}

button:hover {
  background-color: #b91c1c; /* Rouge plus foncé au survol */
  transform: scale(1.05); /* Légère augmentation de taille */
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