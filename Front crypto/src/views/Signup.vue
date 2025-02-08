<template>
    <div class="signup">
      <h2>Inscription</h2>
      <form @submit.prevent="handleSignup">
        <div class="form-group">
          <label>Email</label>
          <input type="email" v-model="email" required />
        </div>
        <div class="form-group">
          <label>Mot de passe</label>
          <input type="password" v-model="password" required />
        </div>
        <button type="submit">S'inscrire</button>
      </form>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref } from 'vue';
  import { useRouter } from 'vue-router';
  
  const email = ref('');
  const password = ref('');
  const router = useRouter();
  
  const handleSignup = () => {
    // Logique d'inscription
    router.push('/dashboard');
  };
  </script>
  
  <style scoped>
  .signup {
    max-width: 400px;
    margin: 0 auto;
    padding: 2rem;
  }
  
  .form-group {
    margin-bottom: 1rem;
  }
  
  label {
    display: block;
    margin-bottom: 0.5rem;
  }
  
  input {
    width: 100%;
    padding: 0.5rem;
    border: 1px solid #ddd;
    border-radius: 4px;
  }
  
  button {
    padding: 0.5rem 1rem;
    background-color: #2563eb;
    color: white;
    border: none;
    border-radius: 4px;
    cursor: pointer;
  }
  
  button:hover {
    background-color: #1e40af;
  }
  </style>