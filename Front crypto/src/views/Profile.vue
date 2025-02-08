<template>
    <div class="profile">
      <Sidebar />
      <div class="main-content">
        <header class="profile-header">
          <h2>Modifier le Profil</h2>
          <p>Mettez à jour vos informations personnelles</p>
        </header>
        <div class="profile-form">
          <form @submit.prevent="handleUpdateProfile">
            <div class="form-group">
              <label>Nom</label>
              <input type="text" v-model="name" required />
            </div>
            <div class="form-group">
              <label>Email</label>
              <input type="email" v-model="email" required />
            </div>
            <div class="form-group">
              <label>Nouveau mot de passe</label>
              <input type="password" v-model="newPassword" />
            </div>
            <button type="submit">Enregistrer</button>
          </form>
        </div>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref } from 'vue';
  import Sidebar from '../components/Sidebar.vue';
  
  const name = ref('Esther Howard');
  const email = ref('esther@example.com');
  const newPassword = ref('');
  
  const handleUpdateProfile = () => {
    alert('Profil mis à jour avec succès !');
  };
  </script>
  
  <style scoped>
  .profile {
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

.profile-header {
  text-align: center;
  margin-bottom: 2rem;
}

.profile-header h2 {
  font-size: 2rem;
  color: #2563eb; /* Bleu pour "Modifier le Profil" */
  animation: fadeIn 1s ease-in-out; /* Animation d'apparition */
}

.profile-header p {
  font-size: 1rem;
  color: #6b7280; /* Gris doux */
}

.profile-form {
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
  border-color: #2563eb; /* Bordure bleue au focus */
  box-shadow: 0 0 5px rgba(37, 99, 235, 0.3); /* Effet de focus */
  outline: none;
}

button {
  width: 100%;
  padding: 0.75rem;
  background-color: #2563eb; /* Bleu pour le bouton "Enregistrer" */
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