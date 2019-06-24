<template>
  <div>
    <div class="row">
      <div v-if="datacollection" class="col-md-6 card p-4">
        <box-plot-chart :chart-data="datacollection"/>
      </div>
    </div>
  </div>
</template>

<script>
import BoxPlotChart from '@/components/Charts/BoxPlotChart.js';
import * as chartConfigs from '@/components/Charts/config';

export default {
  components: { BoxPlotChart },
  data() {
    return {
      datacollection: null
    };
  },
  mounted() {
    this.fillData();
  },
  methods: {
    fillData() {
      this.datacollection = {
        labels: [this.getRandomInt(), this.getRandomInt()],
        datasets: [
          {
            label: 'Data One',
            backgroundColor: '#f87979',
            data: [[1, 2, 3, 4, 5]]
          }
        ]
      };
    },
    getRandomInt() {
      return Math.floor(Math.random() * (50 - 5 + 1)) + 5;
    },
    getRandomValues(min = 0, max = 100) {
      return Array.from({ length: 100 }).map(
        () => Math.random() * (max - min) + min
      );
    }
  }
};
</script>

<style lang="scss" scoped>
</style>