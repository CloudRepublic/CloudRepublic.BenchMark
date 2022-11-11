<template>
  <div>
    <div class="row">
      <div v-if="environment !== null" class="col-md-6">
        <div class="row">
          <div class="col-md-4 d-flex flex-column">
            <div class="d-flex align-items-center">
              <span class="display-1 text-white p-0" style="line-height:0.7;">{{cloudProvider}}</span>
            </div>
            <span class="display-4 runtime-text">{{runtime}} - {{language}}</span>
          </div>
        </div>
      </div>
    </div>
    <div class="row">
      <div class="col-md-6">
        <div class="row">
          <div class="col-sm-12 col-md-12 col-lg-8 col-xl-7">
            <stats-card
              title="Coldstart median"
              :sub-title="coldMedianExecutionTime + 'ms'"
              class="mb-4 mb-xl-0 mt-2"
            >
              <template slot="icon">
                <img
                  style="max-width:32px;"
                  :src="environment === 'Windows' ? 'img/icons/common/microsoft.svg' : 'img/icons/common/linux.svg'"
                />
              </template>
              <template slot="footer">
                <span :class="[ coldPositiveChange ? 'text-success' : 'text-danger','mr-2']">
                  <i :class=" ['fa',coldPositiveChange ? 'fa-arrow-down' : 'fa-arrow-up']  "></i>
                  {{coldChangeSinceYesterday}}%
                </span>
                <span class="text-nowrap">Since yesterday</span>
              </template>
            </stats-card>
          </div>
        </div>
      </div>
      <div class="col-md-6">
        <div class="row">
          <div class="col-sm-12 col-md-12 col-lg-8 col-xl-7">
            <stats-card
              title="Warmstart median"
              :sub-title="warmMedianExecutionTime + 'ms'"
              class="mb-4 mb-xl-0 mt-2"
            >
              <template slot="icon">
                <img
                  style="max-width:32px;"
                  :src="environment === 'Windows' ? 'img/icons/common/microsoft.svg' : 'img/icons/common/linux.svg'"
                />
              </template>
              <template slot="footer">
                <span :class="[ warmPositiveChange ? 'text-success' : 'text-danger','mr-2']">
                  <i :class=" ['fa',warmPositiveChange ? 'fa-arrow-down' : 'fa-arrow-up']  "></i>
                  {{warmChangeSinceYesterday}}%
                </span>
                <span class="text-nowrap">Since yesterday</span>
              </template>
            </stats-card>
          </div>
        </div>
      </div>
    </div>
    <div class="row mt-3">
      <div v-if="coldBenchMarkData !== null" class="col-md-6 mb-3">
        <card type="white" header-classes="bg-transparent">
          <div slot="header" class="row align-items-center">
            <div class="col">
              <h6 class="text-light text-uppercase ls-1 mb-1">Performance</h6>
              <div class="d-flex align-items-center">
                <h5 class="h3 text-dark mb-0">Coldstart</h5>
                <img class="ml-1" style="max-width:20px;" src="img/icons/common/snow.svg" />
              </div>
            </div>
            <div class="col">
              <div class="justify-content-end">
                <h6 class="text-uppercase ls-1 mb-1 text-right">Execution 5/hour</h6>
                <h6
                  class="text-light text-uppercase ls-1 mb-1 text-right"
                >from {{coldBenchMarkDataPointsCount}} datapoints</h6>
              </div>
            </div>
          </div>
          <box-plot-chart
            :height="350"
            ref="coldChart"
            :chart-data="coldBenchMarkChart.chartData"
            :extra-options="coldBenchMarkChart.options"
          ></box-plot-chart>
        </card>
      </div>
      <div v-if="warmBenchMarkData !== null" class="col-md-6">
        <card type="white" header-classes="bg-transparent">
          <div slot="header" class="row align-items-center">
            <div class="col">
              <h6 class="text-light text-uppercase ls-1 mb-1">Performance</h6>
              <div class="d-flex align-items-center">
                <h5 class="h3 text-dark mb-0">Warmstart</h5>
                <img class="ml-1" style="max-width:20px;" src="img/icons/common/energy.svg" />
              </div>
            </div>
            <div class="col">
              <div class="justify-content-end">
                <h6 class="text-uppercase ls-1 mb-1 text-right">Execution 10/hour</h6>
                <h6
                  class="text-light text-uppercase ls-1 mb-1 text-right"
                >from {{warmBenchMarkDataPointsCount}} datapoints</h6>
              </div>
            </div>
          </div>
          <box-plot-chart
            :height="350"
            ref="warmChart"
            :chart-data="warmBenchMarkChart.chartData"
            :extra-options="warmBenchMarkChart.options"
          ></box-plot-chart>
        </card>
      </div>
    </div>
  </div>
</template>

<script>
import * as chartConfigs from '@/components/Charts/config';
import BoxPlotChart from '@/components/Charts/BoxPlotChart.js';

export default {
  name: 'benchmark-envi',
  components: { BoxPlotChart },
  props: {
    coldBenchMarkData: {
      type: Array
    },
    warmBenchMarkData: {
      type: Array
    },
    environment: {
      type: String,
      default: null
    },
    coldMedianExecutionTime: {
      type: Number,
      default: 0
    },
    coldPositiveChange: {
      type: Boolean,
      default: false
    },
    coldChangeSinceYesterday: {
      type: Number,
      default: 1.0
    },
    warmMedianExecutionTime: {
      type: Number,
      default: 0
    },
    warmPositiveChange: {
      type: Boolean,
      default: false
    },
    warmChangeSinceYesterday: {
      type: Number,
      default: 1.0
    },
    runtime: {
      type: String,
      default: ''
    },
    language: {
      type: String,
      default: ''
    },
    cloudProvider: {
      type: String,
      default: ""
    }
  },
  data() {
    return {
      coldBenchMarkChart: {
        chartData: {
          datasets: [
            {
              label: 'milliseconds',
              data: []
            }
          ],
          labels: []
        },
        options: chartConfigs.boxPlotOptions
      },
      coldBenchMarkDataPointsCount: 0,
      warmBenchMarkChart: {
        chartData: {
          datasets: [
            {
              label: 'milliseconds',
              data: []
            }
          ],
          labels: []
        },
        options: chartConfigs.boxPlotOptions
      },
      warmBenchMarkDataPointsCount: 0
    };
  },
  mounted() {
    this.initColdChart();
    this.initWarmChart();
  },
  methods: {
    initColdChart() {
      let chartData = this.formatChartData(this.coldBenchMarkData);
      this.coldBenchMarkDataPointsCount = 0;
      for (let i = 0; i < chartData.datasets[0].data.length; i++) {
        this.coldBenchMarkDataPointsCount +=
          chartData.datasets[0].data[i].length;
      }
      this.coldBenchMarkChart.chartData = chartData;
    },
    initWarmChart() {
      let chartData = this.formatChartData(this.warmBenchMarkData);
      this.warmBenchMarkDataPointsCount = 0;
      for (let i = 0; i < chartData.datasets[0].data.length; i++) {
        this.warmBenchMarkDataPointsCount +=
          chartData.datasets[0].data[i].length;
      }
      this.warmBenchMarkChart.chartData = chartData;
    },
    formatChartData(sourceData) {
      let dataSets = [];
      let benchMarkLabels = [];

      //Loop over initial array
      for (let index = 0; index < sourceData.length; index++) {
        benchMarkLabels.push(sourceData[index].createdAt);

        dataSets.push(sourceData[index].executionTimes);
      }

      let chartData = {
        datasets: [
          {
            label: 'milliseconds',
            backgroundColor: 'rgba(94, 114, 228, 0.5)',
            borderColor: '#5e72e4',
            borderWidth: 1,
            outlierColor: 'rgba(94, 114, 228, 0.5',
            padding: 10,
            itemRadius: 0,
            data: dataSets
          }
        ],
        labels: benchMarkLabels
      };

      return chartData;
    }
  },
  watch: {
    coldBenchMarkData() {
      this.initColdChart();
    },
    warmBenchMarkData() {
      this.initWarmChart();
    }
  }
};
</script>

<style>
.runtime-text {
  text-align: right;
}

@media (max-width: 768px) {
  .runtime-text {
    text-align: left !important;
    margin-left: 4rem;
  }
}

@media (max-width: 576px) {
  .runtime-text {
    text-align: left !important;
    margin-left: 4rem;
  }
}
</style>