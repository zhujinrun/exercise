import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home/Home.vue'

Vue.use(VueRouter)

  const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/home',
    name: 'home',
    component: Home
  },{
    path: '/detail/:id',
    name: 'detail',
    component: () => import( '../views/Detail.vue')
  },
  {
    path: '/find',
    name: 'find',
    component: () => import( '../views/About.vue')
  },
  {
    path: '/order',
    name: 'order',
    component: () => import( '../views/About.vue')
  },
  {
    path: '/my',
    name: 'my',
    component: () => import( '../views/About.vue')
  }
]

const router = new VueRouter({
  linkActiveClass:'active',
  routes
})

export default router
