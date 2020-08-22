<template>
  <div id="app">
    <!-- 上面展示 -->
    <router-view />
    <!-- 底部导航 -->
    <NavBottom  v-show="showNav"/>
  </div>
</template>

<script>
import NavBottom from './views/NavBottom.vue'
import {reactive,toRefs,computed} from "@vue/composition-api"

export default {
  setup(props,{root}){
    const state = reactive({
      showNav:computed(()=>root.$store.getters.showNav)
    });
    return {
      ...toRefs(state)
    }
  },
  components:{
    NavBottom
  }
}
</script>
<style>
@import './assets/css/index.css';
#app .nav-bottom .active {
  color:yellow;
}
</style>
