<template>
  <div class="detail">
    <a class="goback" @click="goBack()">返回</a>
    <h1>{{id}}</h1>
  </div>
</template>

<script>
import {reactive,toRefs,onMounted,onUnmounted} from "@vue/composition-api"

export default {
  setup(props,{root}){
    const state = reactive({
      id:''

    });
    onMounted(()=>{  //挂载完成
      state.id = root.$route.params.id;
      root.$store.dispatch('HIDENAV');  //隐藏
    })
    onUnmounted(()=>{  //销毁
      root.$store.dispatch('SHOWNAV');  //显示
    })
    let goBack =()=>{
      root.$router.push('/home');
    }
    return {
      ...toRefs(state),
      goBack
    }
  }
}
</script>
