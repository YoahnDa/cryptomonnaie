<template>
  <div class="min-h-screen bg-gray-100 flex flex-col justify-center py-12 sm:px-6 lg:px-8">
    <div class="sm:mx-auto sm:w-full sm:max-w-md">
      <h2 class="text-center text-3xl font-extrabold text-gray-900">Sign in to your account</h2>
    </div>

    <div class="mt-8 sm:mx-auto sm:w-full sm:max-w-md">
      <div class="bg-white py-8 px-4 shadow sm:rounded-lg sm:px-10">
        <Form @submit="handleSubmit" v-slot="{ errors }">
          <div class="space-y-6">
            <div>
              <label for="email" class="block text-sm font-medium text-gray-700">Email</label>
              <Field
                id="email"
                name="email"
                type="email"
                rules="required|email"
                v-model="email"
                class="mt-1 block w-full border border-gray-300 rounded-md shadow-sm"
              />
              <span class="text-red-500 text-sm">{{ errors.email }}</span>
            </div>

            <div>
              <label for="password" class="block text-sm font-medium text-gray-700">Password</label>
              <Field
                id="password"
                name="password"
                type="password"
                rules="required"
                v-model="password"
                class="mt-1 block w-full border border-gray-300 rounded-md shadow-sm"
              />
              <span class="text-red-500 text-sm">{{ errors.password }}</span>
            </div>

            <div v-if="showPinInput">
              <label for="pin" class="block text-sm font-medium text-gray-700">PIN Code</label>
              <Field
                id="pin"
                name="pin"
                type="text"
                rules="required|length:6"
                v-model="pin"
                class="mt-1 block w-full border border-gray-300 rounded-md shadow-sm"
              />
              <span class="text-red-500 text-sm">{{ errors.pin }}</span>
            </div>

            <div>
              <button
                type="submit"
                class="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
              >
                {{ showPinInput ? 'Verify PIN' : 'Sign in' }}
              </button>
            </div>
          </div>
        </Form>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { Form, Field } from 'vee-validate'
import { useRouter } from 'vue-router'
import { authService } from '../services/authService'

const router = useRouter()
const email = ref('')
const password = ref('')
const pin = ref('')
const showPinInput = ref(false)

const handleSubmit = async () => {
  try {
    if (!showPinInput.value) {
      await authService.login(email.value, password.value)
      showPinInput.value = true
    } else {
      await authService.validatePin(pin.value)
      router.push('/')
    }
  } catch (error) {
    console.error('Login failed:', error)
  }
}
</script>