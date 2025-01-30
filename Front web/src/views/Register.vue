<template>
  <div class="min-h-screen bg-gray-100 flex flex-col justify-center py-12 sm:px-6 lg:px-8">
    <div class="sm:mx-auto sm:w-full sm:max-w-md">
      <h2 class="text-center text-3xl font-extrabold text-gray-900">Create your account</h2>
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
                rules="required|min:8"
                v-model="password"
                class="mt-1 block w-full border border-gray-300 rounded-md shadow-sm"
              />
              <span class="text-red-500 text-sm">{{ errors.password }}</span>
            </div>

            <div>
              <label for="confirmPassword" class="block text-sm font-medium text-gray-700">
                Confirm Password
              </label>
              <Field
                id="confirmPassword"
                name="confirmPassword"
                type="password"
                rules="required|confirmed:@password"
                v-model="confirmPassword"
                class="mt-1 block w-full border border-gray-300 rounded-md shadow-sm"
              />
              <span class="text-red-500 text-sm">{{ errors.confirmPassword }}</span>
            </div>

            <div>
              <button
                type="submit"
                class="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
              >
                Register
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
const confirmPassword = ref('')

const handleSubmit = async () => {
  try {
    await authService.register(email.value, password.value)
    router.push('/login')
  } catch (error) {
    console.error('Registration failed:', error)
  }
}
</script>