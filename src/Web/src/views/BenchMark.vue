<template>
  <div>
    <vue-element-loading
      :active="showLoading"
      :is-full-screen="true"
      spinner="spinner"
      color="#fff"
      background-color="#172b4d"
    />
    <div class="row exp-text mb-4 d-flex justify-content-center">
      <div class="col-md-8 text-center">
        <span class="display-3 text-white p-0">How the benchmark is executed</span>
        <p class="text-white">
          There's an orchestrator function that executes Http Get requests to every function app instance available.
          The first 5 calls are classified as coldstart, we then wait for 30 seconds to execute 10 requests per function instance to measure the warmed up Http requests.
        </p>
      </div>
    </div>
    <div class="row mb-3" v-if='benchmarkOptions !== null'>
      <div class="col-md-12">
        <tabs>
          <tab-pane
            v-for="benchmarkOption in benchmarkOptions"
            v-bind:key="benchmarkOption.title"
            @tabBecomesActive="loadEnvironment(benchmarkOption)"
            class="envi-tab"
            :title="(benchmarkOption.title)"
          ></tab-pane>
        </tabs>
      </div>
    </div>
    <div v-if="benchMarkData === null">
      <p class="text-white text-center">The request did not return any benchmark data.</p>
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
        :language="language"
        :cloudProvider="cloudProvider"
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
    },
    language() {
      return this.benchMarkData.language;
    },
    cloudProvider() {
      return this.benchMarkData.cloudProvider;
    },
    showLoading() {
      return this.isLoading || this.isTabLoading;
    }
  },
  data() {
    return {
      benchMarkData: null,
      activeEnvironmentIndex: 0,
      isLoading: true,
      isTabLoading: true,
      benchmarkOptions: null
    };
  },
  created() {
    this.loadTabs();
  },
  methods: {
    async loadTabs() {
      this.isLoading = true;
      let benchmarkOptions = await benchMarkService.getCategories();
      this.benchmarkOptions = benchmarkOptions;
      this.isLoading = false;
    },
    async loadEnvironment(benchmarkOptions) {
      this.isTabLoading = true;
      let benchMarkData;
      if (benchmarkOptions != null) {
        benchMarkData = await benchMarkService.getBenchMarkData(
          benchmarkOptions.cloud,
          benchmarkOptions.os,
          benchmarkOptions.runtime,
          benchmarkOptions.language,
          benchmarkOptions.sku
        );
        this.benchMarkData = benchMarkData;
      }
      this.isTabLoading = false;
    }
  },
};
</script>
<style>
@media (width: 600px) {
  .nav-pills .nav-link {
    margin-right: 1rem;
    margin-bottom: 1rem;
    max-width: 10rem;
  }
}

@media (max-width: 576px) {
  .exp-text {
    margin-top: 2rem;
  }
  .nav-pills .nav-link {
    margin-right: 1rem;
  }
}
</style>