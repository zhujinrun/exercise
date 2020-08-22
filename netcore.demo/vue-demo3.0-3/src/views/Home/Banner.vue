<template>
  <!-- 轮播图 -->
  <div class="banner">
    <img v-for="(v,i) in imgArray" :key="i" :src="v" v-show="n==i"/>
    <div class="banner-circle">
      <ul>
        <li v-for="(v,i) in imgArray" :key="i" :class="n==i?'selected':''"></li>
      </ul>
    </div>
  </div>
</template>

<script>
import {reactive,toRefs,onMounted,inject} from "@vue/composition-api"

export default {
  setup(props){   
    const state = reactive({
      n:0,
      imgArray:inject('imgArray')
    });
    const autoPlay=()=>{
      setInterval(play,2000)
    }
    const play =() =>{
      state.n++;
      if(state.n>=state.imgArray.length){
        state.n = 0;
      }
    }
    onMounted(()=>{
      autoPlay()
    })
    return {
      ...toRefs(state)
    }
  }

}
</script>


