<template>
  <div>
    <div class="row">
      <div v-if="benchMarkData !== null" class="col-md-4">
        <div class="row">
          <div class="col-md-6 d-flex flex-column">
            <div class="d-flex align-items-center">
              <span class="display-1 text-white p-0" style="line-height:0.7;">Azure</span>
            </div>
            <span class="display-4 text-right">{{runtime}}</span>
          </div>
        </div>
        <stats-card
          :title="'Function ' + environment"
          :sub-title="averageExecutionTime + 'ms'"
          class="mb-4 mb-xl-0 mt-2"
        >
          <template slot="icon">
            <img
              style="max-width:32px;"
              :src="environment === 'Windows' ? 'img/icons/common/microsoft.svg' : 'img/icons/common/linux.svg'"
            >
          </template>
          <template slot="footer">
            <span :class="[ positiveChange ? 'text-success' : 'text-danger','mr-2']">
              <i :class=" ['fa',positiveChange ? 'fa-arrow-up' : 'fa-arrow-down']  "></i>
              {{changeSinceYesterday}}
            </span>
            <span class="text-nowrap">Since yesterday</span>
          </template>
        </stats-card>
      </div>
    </div>
    <div class="row mt-3">
      <div v-if="benchMarkData !== null" class="col-md-6">
        <card type="white" header-classes="bg-transparent">
          <div slot="header" class="row align-items-center">
            <div class="col">
              <h6 class="text-light text-uppercase ls-1 mb-1">Performance</h6>
              <div class="d-flex align-items-center">
                <h5 class="h3 text-dark mb-0">Coldstart</h5>
                <img class="ml-1" style="max-width:20px;" src="img/icons/common/snow.svg">
              </div>
            </div>
          </div>
          <line-chart
            :height="350"
            ref="coldChart"
            :chart-data="benchMarkChart.chartData"
            :extra-options="benchMarkChart.options"
          ></line-chart>
        </card>
      </div>
      <div v-if="1===0" class="col-md-6">
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
          <line-chart
            :height="350"
            ref="hotChart"
            :chart-data="benchMarkChart.chartData"
            :extra-options="benchMarkChart.options"
          ></line-chart>
        </card>
      </div>
    </div>
  </div>
</template>

<script>
import * as chartConfigs from '@/components/Charts/config';

import LineChart from '@/components/Charts/LineChart';

export default {
  name: 'benchmark-envi',
  components: { LineChart },
  props: {
    benchMarkData: {
      type: Array
    },
    environment: {
      type: String,
      default: ''
    },
    averageExecutionTime: {
      type: Number,
      default: 0
    },
    positiveChange: {
      type: Boolean,
      default: false
    },
    changeSinceYesterday: {
      type: String,
      default: '1.00%'
    },
    runtime: {
      type: String,
      default: ''
    }
  },
  data() {
    return {
      benchMarkChart: {
        chartData: {
          datasets: [
            {
              label: 'milliseconds',
              data: []
            }
          ],
          labels: []
        },
        options: chartConfigs.blueChartOptions
      }
    };
  },
  mounted() {
    this.initChart();
  },
  methods: {
    initChart() {
      let data = this.benchMarkData;

      let benchMarkData = [];
      let benchMarkLabels = [];
      for (let i = 0; i < data.length; i++) {
        benchMarkData.push(data[i].executionTime);
        benchMarkLabels.push(data[i].createdAt);
      }

      let chartData = {
        datasets: [
          {
            label: 'milliseconds',
            data: benchMarkData
          }
        ],
        labels: benchMarkLabels
      };
      this.benchMarkChart.chartData = chartData;
    }
  },
  watch: {
    benchMarkData() {
      this.initChart();
    }
  }
};
</script>

<style lang="scss" scoped>
</style>