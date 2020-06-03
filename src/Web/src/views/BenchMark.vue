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
      <div class="col-md-8 text-center">
        <span class="display-3 text-white p-0">How the benchmark is executed</span>
        <p class="text-white">
          There's an orchestrator function that executes Http Get requests to every function app instance available.
          The first 5 calls are classified as coldstart, we then wait for 30 seconds to execute 10 requests per function instance to measure the warmed up Http requests.
        </p>
      </div>
    </div>
    <div class="row mb-3">
      <div class="col-md-12">
        <tabs>
          <tab-pane
            v-for="benchmarkOption in benchmarkOptions"
            v-bind:key="benchmarkOption.position"
            @tabBecomesActive="loadEnvironment(benchmarkOption)"
            class="envi-tab"
            :title="(benchmarkOption.title)"
          ></tab-pane>
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
      isLoading: true,
      benchmarkOptions: [
        {
          position: 1,
          title: 'Azure - Windows C#',
          cloud: 'Azure',
          os: 'Windows',
          language: 'Csharp'
        },
        {
          position: 2,
          title: 'Azure - Windows Nodejs',
          cloud: 'Azure',
          os: 'Windows',
          language: 'Nodejs'
        },
        {
          position: 3,
          title: 'Azure - Windows Python',
          cloud: 'Azure',
          os: 'Windows',
          language: 'Python'
        },
        {
          position: 4,
          title: 'Azure - Linux C#',
          cloud: 'Azure',
          os: 'Linux',
          language: 'Csharp'
        },
        {
          position: 5,
          title: 'Azure - Linux  Nodejs',
          cloud: 'Azure',
          os: 'Linux',
          language: 'Nodejs'
        },
        {
          position: 6,
          title: 'Azure - Linux Python',
          cloud: 'Azure',
          os: 'Linux',
          language: 'Python'
        },
        {
          position: 7,
          title: 'Firebase - Linux Nodejs',
          cloud: 'Firebase',
          os: 'Linux',
          language: 'Nodejs'
        }
      ]
    };
  },
  methods: {
    async loadEnvironment(benchmarkOptions) {
      this.isLoading = true;
      let benchMarkData;
      // todo enable when backend attached/active
      if (benchmarkOptions != null) {
        benchMarkData = await benchMarkService.getBenchMarkData(
          benchmarkOptions.cloud,
          benchmarkOptions.os,
          benchmarkOptions.language
        );
        this.benchMarkData = benchMarkData;
      }
      this.isLoading = false;
    },
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
          'Windows',
          'Python'
        );
      }

      if (tabIndex === 3) {
        benchMarkData = await benchMarkService.getBenchMarkData(
          'Azure',
          'Linux',
          'Csharp'
        );
      }

      if (tabIndex === 4) {
        benchMarkData = await benchMarkService.getBenchMarkData(
          'Azure',
          'Linux',
          'Nodejs'
        );
      }

      if (tabIndex === 5) {
        benchMarkData = await benchMarkService.getBenchMarkData(
          'Azure',
          'Linux',
          'Python'
        );
      }

      if (tabIndex === 6) {
        benchMarkData = await benchMarkService.getBenchMarkData(
          'Firebase',
          'Linux',
          'Nodejs'
        );
      }

      this.benchMarkData = benchMarkData;
      this.isLoading = false;
    }
  },
  mounted() {},
  async beforeMount() {
    await this.loadEnvironment(null);
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