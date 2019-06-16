<template>
  <div>
    <card type="white" header-classes="bg-transparent">
      <div slot="header" class="row align-items-center">
        <div class="col">
          <h6 class="text-light text-uppercase ls-1 mb-1">Performance</h6>
          <div class="d-flex align-items-center">
            <h5 class="h3 text-dark mb-0">Hotstart</h5>
            <img class="ml-1" style="max-width:20px;" src="img/icons/common/energy.svg">
          </div>
        </div>
      </div>
      <line-chart :height="350" ref="hotChart" :chart-data="chartData" :extra-options="options"></line-chart>
    </card>
  </div>
</template>

<script>
import LineChart from '@/components/Charts/LineChart';
import * as chartConfigs from '@/components/Charts/config';

export default {
  components: { LineChart },
  data() {
    return {
      chartData: {
        datasets: [
          {
            label: 'milliseconds',
            data: [5, 5, 3, 86, 6, 3, 4]
          }
        ],
        labels: ['a', 'b', 'c', 'd', 'e', 'f', 'g']
      },
      options: chartConfigs.blueChartOptions
    };
  }
};
</script>

<style lang="scss" scoped>
</style>