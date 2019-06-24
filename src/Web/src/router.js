import Vue from 'vue';
import Router from 'vue-router';
import BenchMarkLayout from '@/layout/BenchMarkLayout';
import BenchMarkView from './views/BenchMark.vue';
import BoxView from './views/BoxPlot.vue';
Vue.use(Router);

export default new Router({
  mode: 'history',
  linkExactActiveClass: 'active',
  routes: [
    {
      path: '/',
      redirect: 'benchmark',
      component: BenchMarkLayout,
      children: [
        {
          path: '/',
          name: 'benchmark',
          component: BenchMarkView
        },
        {
          path: '/box',
          name: 'box',
          component: BoxView
        }
      ]
    }
  ]
});
