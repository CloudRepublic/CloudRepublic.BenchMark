<template>
  <div>
    <vue-element-loading
      :active="isLoading"
      :is-full-screen="true"
      spinner="spinner"
      color="#fff"
      background-color="#172b4d"
    />
    <div class="row exp-text mb-4 d-flex justify-content-center">
      <div class="col-md-7 text-center">
        <span class="display-3 text-white p-0">How the benchmark is executed</span>
        <p
          class="text-white"
        >There's an orchestrator function that executes Http Get requests to every function app instance available. The first 5 calls are classified as coldstart, we then wait for 30 seconds to execute 10 requests per function instance to measure the warmed up Http requests.</p>
      </div>
    </div>
    <div class="row mb-3">
      <div class="col-md-12">
        <tabs @activating-tab="loadEnvironment">
          <tab-pane class="envi-tab" title="Azure Windows Csharp"></tab-pane>
          <tab-pane class="envi-tab" title="Azure Windows Nodejs"></tab-pane>
          <tab-pane class="envi-tab" title="Azure Windows Fsharp"></tab-pane>
          <tab-pane class="envi-tab" title="Azure Linux Csharp"></tab-pane>
          <tab-pane class="envi-tab" title="Azure Linux Nodejs"></tab-pane>
          <tab-pane class="envi-tab" title="Azure Linux Fsharp"></tab-pane>
          <tab-pane class="envi-tab" title="Firebase Linux Nodejs"></tab-pane>
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
    cloudProvider() {
      return this.benchMarkData.cloudProvider;
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
    async loadEnvironment(tab) {
      this.isLoading = true;
      
      let environment = tab.title.split(' ');

      if(environment.length < 3) {
        console.log('Invalid environment string; needs at least 3 elements');
        return;
      }

      this.benchMarkData = await benchMarkService.getBenchMarkData(environment[0], environment[1], environment[2]);
      this.isLoading = false;
    }
  },
  mounted() {},
  async beforeMount() {
    await this.loadEnvironment({title:'Azure Windows Csharp'});
  }
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