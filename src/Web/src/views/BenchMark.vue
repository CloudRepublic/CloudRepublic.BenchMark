<template>
  <div>
    <vue-element-loading
      :active="isLoading"
      :is-full-screen="true"
      spinner="spinner"
      color="#fff"
      background-color="#172b4d"
    />
    <div class="row mb-4 d-flex justify-content-center">
      <div class="col-md-7 text-center">
        <span class="display-3 text-white p-0">How the benchmark is executed</span>
        <p
          class="text-white"
        >There's an orchestrator function that executes Http Get requests to every function app instance available. The first 5 calls are classified as coldstart, we then wait for 30 seconds to execute 10 requests per function instance to measure the warmed up Http requests.</p>
      </div>
    </div>
    <div class="row mb-3">
      <div class="col-md-12">
        <tabs @tabIndexChanged="loadEvironment">
          <tab-pane title="Windows Csharp"></tab-pane>
          <tab-pane title="Windows Nodejs"></tab-pane>
          <tab-pane title="Linux Csharp"></tab-pane>
          <tab-pane title="Linux Nodejs"></tab-pane>
        </tabs>
      </div>
    </div>
    <div v-if="benchMarkData !== null">
      <BenchMarkEnvi
        :environment="environment"
        :coldBenchMarkData="coldData"
        :coldMedianExecutionTime="coldMedianExecutionTime"
        :coldChangeSinceYesterday="coldPreviousDayDifference"
        :coldPositiveChange="coldPreviousDayPositive"
        :warmBenchMarkData="warmData"
        :warmMedianExecutionTime="warmMedianExecutionTime"
        :warmChangeSinceYesterday="warmPreviousDayDifference"
        :warmPositiveChange="warmPreviousDayPositive"
        :runtime="runtime"
      ></BenchMarkEnvi>
    </div>
  </div>
</template>
<script>
import { benchMarkService } from '@/services';
import BenchMarkEnvi from '@/components/Custom/BenchMarkEnvi';
import VueElementLoading from 'vue-element-loading';

export default {
  name: 'benchmark',
  components: { BenchMarkEnvi, VueElementLoading },
  computed: {
    environment() {
      return this.benchMarkData.hostingEnvironment;
    },
    coldData() {
      return this.benchMarkData.coldDataPoints;
    },
    warmData() {
      return this.benchMarkData.warmDataPoints;
    },
    coldMedianExecutionTime() {
      return this.benchMarkData.coldMedianExecutionTime;
    },
    coldPreviousDayDifference() {
      return this.benchMarkData.coldPreviousDayDifference;
    },
    coldPreviousDayPositive() {
      return this.benchMarkData.coldPreviousDayPositive;
    },
    warmMedianExecutionTime() {
      return this.benchMarkData.warmMedianExecutionTime;
    },
    warmPreviousDayDifference() {
      return this.benchMarkData.warmPreviousDayDifference;
    },
    warmPreviousDayPositive() {
      return this.benchMarkData.warmPreviousDayPositive;
    },
    runtime() {
      return this.benchMarkData.runtime;
    }
  },
  data() {
    return {
      benchMarkData: null,
      activeEnvironmentIndex: 0,
      isLoading: true
    };
  },
  methods: {
    async loadEvironment(tabIndex) {
      this.isLoading = true;
      let benchMarkData;
      if (tabIndex === 0) {
        benchMarkData = await benchMarkService.getBenchMarkData(
          'Azure',
          'Windows',
          'Csharp'
        );
      }

      if (tabIndex === 1) {
        benchMarkData = await benchMarkService.getBenchMarkData(
          'Azure',
          'Windows',
          'Nodejs'
        );
      }
      if (tabIndex === 2) {
        benchMarkData = await benchMarkService.getBenchMarkData(
          'Azure',
          'Linux',
          'Csharp'
        );
      }

      if (tabIndex === 3) {
        benchMarkData = await benchMarkService.getBenchMarkData(
          'Azure',
          'Linux',
          'Nodejs'
        );
      }
      console.log(benchMarkData);
      this.benchMarkData = benchMarkData;
      this.isLoading = false;
    }
  },
  mounted() {},
  async beforeMount() {
    await this.loadEvironment(0);
  }
};
</script>
<style></style>
