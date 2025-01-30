<template>
  <footer class="bg-gray-800 text-white p-4 mt-auto">
    <div class="container mx-auto text-center">
      <p>&copy; {{ currentYear }} Crypto Trading Platform. All rights reserved.</p>
    </div>
  </footer>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const currentYear = ref(new Date().getFullYear())
</script>