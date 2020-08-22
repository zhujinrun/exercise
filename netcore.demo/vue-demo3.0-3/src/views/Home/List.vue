<template>
    <!-- 列表展示 -->
    <div class="index-main">
      <ul>
        <li class="lists" v-for="v in items" :key="v.product_id">
          <router-link :to="'/detail/'+v.product_id">
            <img :src="v.product_img_url" />
          </router-link>
          <label>
            <b class="discount">折扣价:{{v.product_uprice}}</b>
            <span class="price-text">原价:{{v.product_price}}</span>
          </label>
        </li>
        
      </ul>
    </div>
</template>

<script>
import {reactive,toRefs,computed,onMounted} from "@vue/composition-api"

export default {
  name: 'Home',
  setup(props,{root}){
    const state = reactive({
     items:[]
    });
    let getData=()=>{
      root.$http.get('/home/page/1/20').then(res=>{
        console.log(res);
        if(res.status =='200'){
          state.items = res.data.data;
        }
      },error=>{
        console.log(error)
      })
    }
    onMounted(()=>{
      getData();
    })
    return {
      ...toRefs(state),
    }
  }

}
</script>


