<template>
  <div class="signup-page">
    <div class="container">
      <div class="row justify-content-center align-items-center min-vh-100">
        <div class="col-md-6 col-lg-4 fade-in">
          <div class="signup-card">
            <div class="card-body p-5">
              <div class="text-center mb-4">
                <h1 class="brand-name">e-CRYPTO</h1>
                <p class="text-muted">Créez un compte pour commencer.</p>
              </div>
              <form @submit.prevent="handleSignup" class="signup-form">
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

                <!-- Champ Email -->
                <div class="mb-4">
                  <label for="email" class="form-label">Email</label>
                  <div class="input-group">
                    <span class="input-group-text">
                      <i class="bi bi-envelope"></i>
                    </span>
                    <input 
                      type="email" 
                      class="form-control" 
                      id="email" 
                      v-model="email"
                      placeholder="Entrez votre email"
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

                <!-- Bouton S'inscrire -->
                <button type="submit" class="btn btn-primary w-100 signup-btn">
                  <span>S'inscrire</span>
                  <i class="bi bi-arrow-right"></i>
                </button>
              </form>

              <!-- Lien vers la page de connexion -->
              <div class="text-center mt-4">
                <p class="text-muted">
                  Déjà inscrit ? 
                  <router-link to="/login" class="link-primary text-decoration-none">Se connecter</router-link>
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import apiClient from '../plugins/axios'; // Import d'Axios

const router = useRouter();
const username = ref('');
const email = ref('');
const password = ref('');

// ✅ Fonction d'inscription
const handleSignup = async () => {
  if (!username.value || !email.value || !password.value) {
    alert('Veuillez remplir tous les champs.');
    return;
  }

  try {
    const response = await apiClient.post('/auth/register', {
      username: username.value,
      email: email.value,
      password: password.value,
    });

    console.log('Inscription réussie:', response.data);
    alert('Inscription réussie ! Vérifiez votre email pour confirmation.');
    router.push('/login');
  } catch (err: unknown) {
    const error = err as { response?: { data?: { message?: string } } };
    alert(error.response?.data?.message || "Erreur lors de l'inscription");
  }
};
</script>
<style scoped>
/* Animation d'entrée */
.fade-in {
  animation: fadeIn 0.6s ease-out forwards;
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

.signup-page {
  background: linear-gradient(135deg, #0f172a 0%, #1e293b 100%);
  min-height: 100vh;
}

.signup-card {
  background: rgba(255, 255, 255, 0.95);
  border-radius: 1rem;
  box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.25);
  backdrop-filter: blur(10px);
}

.brand-name {
  background: linear-gradient(135deg, #2563eb, #1e40af);
  -webkit-background-clip: text;
  color: transparent;
  font-size: 2.5rem;
  font-weight: 700;
  letter-spacing: -1px;
  margin-bottom: 1rem;
}

.input-group-text {
  background: transparent;
  border-right: none;
}

.input-group .form-control {
  border-left: none;
}

.input-group:focus-within {
  box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.1);
}

.signup-btn {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  padding: 0.75rem;
  font-weight: 600;
  transition: all 0.3s ease;
}

.signup-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}

.signup-btn i {
  transition: transform 0.3s ease;
}

.signup-btn:hover i {
  transform: translateX(4px);
}

/* Lien vers la page de connexion */
.link-primary {
  color: #2563eb; /* Bleu vibrant */
  font-weight: 600;
  transition: color 0.3s ease-in-out;
}

.link-primary:hover {
  color: #1e40af; /* Bleu plus foncé */
  text-decoration: underline;
}
</style>