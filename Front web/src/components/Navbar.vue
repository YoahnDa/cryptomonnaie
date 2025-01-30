<template>
  <nav class="bg-gray-800 text-white p-4">
    <div class="container mx-auto flex justify-between items-center">
      <router-link to="/" class="text-xl font-bold">Crypto Platform</router-link>
      <div class="space-x-4">
        <template v-if="isAuthenticated">
          <router-link to="/" class="hover:text-gray-300">Home</router-link>
          <router-link to="/wallet" class="hover:text-gray-300">Wallet</router-link>
          <router-link to="/trade" class="hover:text-gray-300">Trade</router-link>
          <router-link to="/market" class="hover:text-gray-300">Market</router-link>
          <button @click="logout" class="hover:text-gray-300">Logout</button>
        </template>
        <template v-else>
          <router-link to="/login" class="hover:text-gray-300">Login</router-link>
          <router-link to="/register" class="hover:text-gray-300">Register</router-link>
        </template>
      </div>
    </div>
  </nav>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRouter } from 'vue-router'
import { authService } from '../services/authService'

const router = useRouter()
const isAuthenticated = computed(() => !!localStorage.getItem('token'))

const logout = () => {
  authService.logout()
  router.push('/login')
}
</script>