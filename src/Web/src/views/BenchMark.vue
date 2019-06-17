<template>
  <div>
    <vue-element-loading
      :active="isLoading"
      :is-full-screen="true"
      spinner="spinner"
      color="#fff"
      background-color="#172b4d"
    />

    <div class="row mb-7">
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
        :benchMarkData="data"
        :averageExecutionTime="averageExecutionTime"
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
      return this.benchMarkData.cloudProviders[0].hostingEnvironments[0]
        .runtimes[0].dataPoints;
    },
    warmData() {},
    averageExecutionTime() {
      return this.benchMarkData.cloudProviders[0].hostingEnvironments[0]
        .runtimes[0].averageExecutionTime;
    },
    runtime() {
      return this.benchMarkData.cloudProviders[0].hostingEnvironments[0]
        .runtimes[0].name;
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
