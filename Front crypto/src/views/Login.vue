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
                      placeholder="Enter your email"
                      required
                    >
                  </div>
                </div>
                <div class="mb-4">
                  <label for="password" class="form-label">Password</label>
                  <div class="input-group">
                    <span class="input-group-text">
                      <i class="bi bi-lock"></i>
                    </span>
                    <input 
                      type="password" 
                      class="form-control" 
                      id="password" 
                      v-model="password"
                      placeholder="Enter your password"
                      required
                    >
                  </div>
                </div>
                <button type="submit" class="btn btn-primary w-100 login-btn">
                  <span>Login</span>
                  <i class="bi bi-arrow-right"></i>
                </button>
              </form>
              
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const email = ref('')
const password = ref('')

const handleLogin = () => {
  router.push('/dashboard')
}
</script>

<style scoped>
.login-page {
  background: linear-gradient(135deg, #0f172a 0%, #1e293b 100%);
  min-height: 100vh;
}

.login-card {
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

.login-btn {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  padding: 0.75rem;
  font-weight: 600;
  transition: all 0.3s ease;
}

.login-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}

.login-btn i {
  transition: transform 0.3s ease;
}

.login-btn:hover i {
  transform: translateX(4px);
}

.fade-in {
  animation: fadeIn 0.6s ease-out forwards;
}
</style>