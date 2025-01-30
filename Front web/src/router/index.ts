import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/Home.vue'
import Login from '../views/Login.vue'
import Register from '../views/Register.vue'
import Wallet from '../views/Wallet.vue'
import Trade from '../views/Trade.vue'
import Market from '../views/Market.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      name: 'home',
      component: Home,
      meta: { requiresAuth: true }
    },
    {
      path: '/login',
      name: 'login',
      component: Login
    },
    {
      path: '/register',
      name: 'register',
      component: Register
    },
    {
      path: '/wallet',
      name: 'wallet',
      component: Wallet,
      meta: { requiresAuth: true }
    },
    {
      path: '/trade',
      name: 'trade',
      component: Trade,
      meta: { requiresAuth: true }
    },
    {
      path: '/market',
      name: 'market',
      component: Market,
      meta: { requiresAuth: true }
    }
  ]
})

router.beforeEach((to, from, next) => {
  const isAuthenticated = localStorage.getItem('token')
  
  if (to.meta.requiresAuth && !isAuthenticated) {
    next('/login')
  } else {
    next()
  }
})

export default router