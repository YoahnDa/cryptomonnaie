<template>
  <div class="login-page">
    <div class="container">
      <div class="row justify-content-center align-items-center min-vh-100">
        <div class="col-md-6 col-lg-4 fade-in">
          <div class="login-card">
            <div class="card-body p-5">
              <div class="text-center mb-4">
                <h1 class="brand-name">e-CRYPTO</h1>
                <p class="text-muted">Welcome back! Please login to your account.</p>
              </div>
              <form @submit.prevent="handleLogin" class="login-form">
                <!-- Champ Username -->
                <div class="mb-4">
                  <label for="username" class="form-label">Nom d'utilisateur</label>
                  <div class="input-group">
                    <span class="input-group-text">
                      <i class="bi bi-person"></i>
                    </span>
                    <input 
                      type="text" 
                      class="form-control" 
                      id="username" 
                      v-model="username"
                      placeholder="Entrez votre nom d'utilisateur"
                      required
                    >
                  </div>
                </div>

                <!-- Champ Password -->
                <div class="mb-4">
                  <label for="password" class="form-label">Mot de passe</label>
                  <div class="input-group">
                    <span class="input-group-text">
                      <i class="bi bi-lock"></i>
                    </span>
                    <input 
                      type="password" 
                      class="form-control" 
                      id="password" 
                      v-model="password"
                      placeholder="Entrez votre mot de passe"
                      required
                    >
                  </div>
                </div>

                <!-- Bouton Login -->
                <button type="submit" class="btn btn-primary w-100 login-btn">
                  <span>Se connecter</span>
                  <i class="bi bi-arrow-right"></i>
                </button>
              </form>

              <!-- Lien vers la page d'inscription -->
              <div class="text-center mt-4">
                <p class="text-muted">
                  Vous n'avez pas de compte ? 
                  <router-link to="/signup" class="link-primary text-decoration-none">Créer un nouveau compte</router-link>
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal pour la vérification PIN -->
    <div v-if="showPinModal" class="pin-modal-overlay">
      <div class="pin-modal">
        <h3>Vérification PIN</h3>
        <p>Un PIN temporaire a été envoyé à votre email. Veuillez l'entrer ci-dessous :</p>
        <form @submit.prevent="verifyPin">
          <div class="mb-3">
            <input 
              type="text" 
              class="form-control" 
              v-model="enteredPin" 
              placeholder="Entrez le PIN" 
              required
            />
          </div>
          <div class="d-flex justify-content-between">
            <button type="button" class="btn btn-secondary" @click="resendPin">Renvoyer le PIN</button>
            <button type="submit" class="btn btn-primary">Vérifier</button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import apiClient from '../plugins/axios'; // Importez Axios
import axios, { AxiosError } from 'axios'; // Importer Axios et son type

const router = useRouter();
const username = ref('');
const password = ref('');
const showPinModal = ref(false);
const enteredPin = ref('');
const tempPin = ref('');

// Gestion de la connexion
const handleLogin = async () => {
    try {
        const response = await apiClient.post('/auth/login', {
            username: username.value,
            password: password.value,
        });

        console.log('Réponse du backend:', response.data);
        tempPin.value = response.data.tempPin; // Supposons que le backend renvoie un PIN temporaire
        showPinModal.value = true;
    } catch (error: unknown) {
        if (axios.isAxiosError(error)) {
            console.error('Erreur Axios lors de la connexion:', error.response?.data || error.message);
            alert(error.response?.data?.message || 'Identifiants incorrects. Veuillez réessayer.');
        } else if (error instanceof Error) {
            console.error('Erreur générale lors de la connexion:', error.message);
            alert('Une erreur est survenue. Veuillez réessayer.');
        } else {
            console.error('Erreur inconnue lors de la connexion:', error);
        }
    }
};

// Vérification du PIN
const verifyPin = async () => {
    try {
        const response = await apiClient.post('/auth/verify-pin', {
            username: username.value,
            pin: enteredPin.value,
        });

        if (response.status === 200) {
            alert('Authentification réussie ! Redirection...');
            showPinModal.value = false;
            router.push('/dashboard');
        } else {
            alert('PIN incorrect. Veuillez réessayer.');
        }
    } catch (error) {
        if (axios.isAxiosError(error)) {
            console.error('Erreur Axios lors de la vérification du PIN:', error.response?.data || error.message);
            alert(error.response?.data?.message || 'Une erreur s\'est produite. Veuillez réessayer.');
        } else {
            console.error('Erreur inconnue lors de la vérification du PIN:', error);
            alert('Une erreur s\'est produite. Veuillez réessayer.');
        }
    }
};

// Renvoyer le PIN
const resendPin = async () => {
    try {
        const response = await apiClient.post('/auth/resend-pin', {
            username: username.value,
        });

        if (response.status === 200) {
            tempPin.value = response.data.tempPin;
            alert('Un nouveau PIN a été envoyé à votre email.');
        }
    } catch (error: unknown) {
        if (axios.isAxiosError(error)) {
            console.error('Erreur Axios lors de la demande de renvoi du PIN:', error.response?.data || error.message);
            alert(error.response?.data?.message || 'Une erreur s\'est produite. Veuillez réessayer.');
        } else if (error instanceof Error) {
            console.error('Erreur générale lors de la demande de renvoi du PIN:', error.message);
            alert('Une erreur est survenue. Veuillez réessayer.');
        } else {
            console.error('Erreur inconnue lors de la demande de renvoi du PIN:', error);
        }
    }
};
</script>
<style scoped>
/* Modal Overlay */
.pin-modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

/* Modal Content */
.pin-modal {
  background: #fff;
  padding: 2rem;
  border-radius: 1rem;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  width: 100%;
  max-width: 400px;
  text-align: center;
}

.pin-modal h3 {
  margin-bottom: 1rem;
  color: #333;
}

.pin-modal p {
  margin-bottom: 1.5rem;
  color: #666;
}

.pin-modal .form-control {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #ddd;
  border-radius: 6px;
  margin-bottom: 1rem;
}

.pin-modal .btn {
  padding: 0.5rem 1rem;
  border-radius: 6px;
}

.pin-modal .btn-secondary {
  background-color: #6c757d;
  color: #fff;
}

.pin-modal .btn-primary {
  background-color: #2563eb;
  color: #fff;
}
</style>
