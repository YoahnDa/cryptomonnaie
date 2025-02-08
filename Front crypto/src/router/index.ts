import { createRouter, createWebHistory } from 'vue-router';
import Dashboard from '../views/Dashboard.vue';
import Buy from '../views/Buy.vue';
import Sell from '../views/Sell.vue';
import History from '../views/History.vue';
import Deposit from '../views/Deposit.vue';
import Withdraw from '../views/Withdraw.vue';
import RealTime from '../views/RealTime.vue';
import Analysis from '../views/Analysis.vue';
import CommissionAnalysis from '../views/CommissionAnalysis.vue';
import TotalSalesPurchases from '../views/TotalSalesPurchases.vue';
import AdminValidation from '../views/admin/AdminValidation.vue';
import AdminUserHistory from '../views/admin/AdminUserHistory.vue';
import AdminLogin from '../views/admin/AdminLogin.vue';
import Login from '../views/Login.vue';
import Profile from '../views/Profile.vue';
import Sidebar from '../views/Sidebar.vue';
import Signup from '../views/Signup.vue';
import UserHistory from '../views/UserHistory.vue';

const routes = [
  { path: '/', redirect: '/login' },
  { path: '/dashboard', component: Dashboard },
  { path: '/login', component: Login },
  { path: '/profile', component: Profile},
  { path: '/buy', component: Buy },
  { path: '/sell', component: Sell },
  { path: '/history', component: History },
  { path: '/deposit', component: Deposit },
  { path: '/withdraw', component: Withdraw },
  { path: '/real-time', component: RealTime },
  { path: '/analysis', component: Analysis },
  { path: '/commission-analysis', component: CommissionAnalysis },
  { path: '/total-sales-purchases', component: TotalSalesPurchases },
  { path: '/admin/adminValidation', component: AdminValidation },
  { path: '/admin/adminUserHistory', component: AdminUserHistory },
  { path: '/admin/adminLogin', component: AdminLogin },
  { path: '/sidebar', component: Sidebar },
  { path: '/signup', component: Signup },
  { path: '/userHistory', component: UserHistory },

];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;