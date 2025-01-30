<template>
  <div class="container mx-auto px-4 py-8">
    <h1 class="text-3xl font-bold mb-8">Welcome to Your Crypto Dashboard</h1>
    
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <!-- Portfolio Summary Card -->
      <div class="bg-white rounded-lg shadow p-6">
        <h2 class="text-xl font-semibold mb-4">Portfolio Summary</h2>
        <div class="space-y-2">
          <p class="flex justify-between">
            <span>Total Balance:</span>
            <span class="font-bold">$0.00</span>
          </p>
          <p class="flex justify-between">
            <span>24h Change:</span>
            <span class="text-green-500">+0.00%</span>
          </p>
        </div>
      </div>

      <!-- Quick Actions Card -->
      <div class="bg-white rounded-lg shadow p-6">
        <h2 class="text-xl font-semibold mb-4">Quick Actions</h2>
        <div class="space-y-3">
          <router-link
            to="/trade"
            class="block w-full text-center bg-indigo-600 text-white py-2 rounded-md hover:bg-indigo-700"
          >
            Trade Now
          </router-link>
          <router-link
            to="/wallet"
            class="block w-full text-center bg-gray-600 text-white py-2 rounded-md hover:bg-gray-700"
          >
            Manage Wallet
          </router-link>
        </div>
      </div>

      <!-- Market Overview Card -->
      <div class="bg-white rounded-lg shadow p-6">
        <h2 class="text-xl font-semibold mb-4">Market Overview</h2>
        <div class="space-y-2">
          <p class="flex justify-between">
            <span>BTC/USD:</span>
            <span>$0.00</span>
          </p>
          <p class="flex justify-between">
            <span>ETH/USD:</span>
            <span>$0.00</span>
          </p>
        </div>
        <router-link
          to="/market"
          class="block w-full text-center mt-4 text-indigo-600 hover:text-indigo-800"
        >
          View All Markets →
        </router-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
// Component logic will be added in the next iteration
</script>