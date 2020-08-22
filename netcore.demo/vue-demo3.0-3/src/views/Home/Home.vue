<template>
  <div>
    <!-- 轮播图 -->
    <Banner />
    <!-- 导航 -->
    <div class="index-category">
      <div class="category" v-for="(v,i) in lists" :key="i">
        <i class="iconfont" :class="v.icon" :style="{background:v.color}"></i>
        <label>{{v.title}}</label>
      </div>
    </div>
    <!-- 列表展示 -->
    <List />
  </div>
</template>

<script>
import Banner from './Banner.vue'
import List from './List.vue'
import {reactive,toRefs,provide} from "@vue/composition-api"

export default {
  name: 'Home',
  setup(){
    const state = reactive({
      lists:[
        {title:'美食1',icon:'icontongxunlu1',color:'#f60'},
        {title:'美食2',icon:'iconfaxian2',color:'#92d85c'},
        {title:'美食3',icon:'icontongxunlu1',color:'#f60'},
        {title:'美食4',icon:'iconfaxian2',color:'#92d85c'},
        {title:'美食5',icon:'icontongxunlu1',color:'#f60'},
        {title:'美食6',icon:'iconfaxian2',color:'#92d85c'},
        {title:'美食7',icon:'icontongxunlu1',color:'#f60'}
      ],
       imgArray:[
        require("@/assets/img/1.jpg"),
        require("@/assets/img/2.jpg"),
        require("@/assets/img/3.jpg"),
        require("@/assets/img/4.jpg"),
      ]

    });
    provide('imgArray',state.imgArray);

    return {
      ...toRefs(state),
    }
  },
  components:{
    Banner,
    List
  }

}
</script>


