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

// The frontend -> backend communication looks at string names not enum int values so we use strings here that must match the names of the backend enum.
const Language = Object.freeze({
  Csharp: 'Csharp',
  Nodejs: 'Nodejs',
  Python: 'Python',
  Java: 'Java',
  Fsharp: 'Fsharp'
});

const HostEnvironment = Object.freeze({
  Windows: 'Windows',
  Linux: 'Linux'
});

const CloudProvider = Object.freeze({
  Azure: 'Azure',
  Firebase: 'Firebase'
});

const AzureRuntimeVersion = Object.freeze({
  Version_2: 'Version_2',
  Version_3: 'Version_3',
  Not_Azure: 'Not_Azure'
});
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
    language() {
      return this.benchMarkData.language;
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
        // AZURE WINDOWS V2
        {
          title: 'Azure - Windows C#',
          cloud: CloudProvider.Azure,
          os: HostEnvironment.Windows,
          language: Language.Csharp,
          azureRuntimeVersion: AzureRuntimeVersion.Version_2
        },
        {
          title: 'Azure - Windows Nodejs',
          cloud: CloudProvider.Azure,
          os: HostEnvironment.Windows,
          language: Language.Nodejs,
          azureRuntimeVersion: AzureRuntimeVersion.Version_2
        },
        {
          title: 'Azure - Windows Python',
          cloud: CloudProvider.Azure,
          os: HostEnvironment.Windows,
          language: Language.Python,
          azureRuntimeVersion: AzureRuntimeVersion.Version_2
        },
        {
          title: 'Azure - Windows Java',
          cloud: CloudProvider.Azure,
          os: HostEnvironment.Windows,
          language: Language.Java,
          azureRuntimeVersion: AzureRuntimeVersion.Version_2
        },
        {
          title: 'Azure - Windows Fsharp',
          cloud: CloudProvider.Azure,
          os: HostEnvironment.Windows,
          language: Language.Fsharp,
          azureRuntimeVersion: AzureRuntimeVersion.Version_2
        },
        // AZURE LINUX V2
        {
          title: 'Azure - Linux C#',
          cloud: CloudProvider.Azure,
          os: HostEnvironment.Linux,
          language: Language.Csharp,
          azureRuntimeVersion: AzureRuntimeVersion.Version_2
        },
        {
          title: 'Azure - Linux  Nodejs',
          cloud: CloudProvider.Azure,
          os: HostEnvironment.Linux,
          language: Language.Nodejs,
          azureRuntimeVersion: AzureRuntimeVersion.Version_2
        },
        {
          title: 'Azure - Linux Python',
          cloud: CloudProvider.Azure,
          os: HostEnvironment.Linux,
          language: Language.Python,
          azureRuntimeVersion: AzureRuntimeVersion.Version_2
        },
        {
          title: 'Azure - Linux Java',
          cloud: CloudProvider.Azure,
          os: HostEnvironment.Linux,
          language: Language.Java,
          azureRuntimeVersion: AzureRuntimeVersion.Version_2
        },
        {
          title: 'Azure - Linux Fsharp',
          cloud: CloudProvider.Azure,
          os: HostEnvironment.Linux,
          language: Language.Fsharp,
          azureRuntimeVersion: AzureRuntimeVersion.Version_2
        },
        // AZURE WINDOWS V3
        {
          title: 'Azure V3 - Windows C#',
          cloud: CloudProvider.Azure,
          os: HostEnvironment.Windows,
          language: Language.Csharp,
          azureRuntimeVersion: AzureRuntimeVersion.Version_3
        },
        {
          title: 'Azure V3 - Windows Nodejs',
          cloud: CloudProvider.Azure,
          os: HostEnvironment.Windows,
          language: Language.Nodejs,
          azureRuntimeVersion: AzureRuntimeVersion.Version_3
        },
        {
          title: 'Azure V3 - Windows Java',
          cloud: CloudProvider.Azure,
          os: HostEnvironment.Windows,
          language: Language.Java,
          azureRuntimeVersion: AzureRuntimeVersion.Version_3
        },
        {
          title: 'Azure V3 - Windows Fsharp ',
          cloud: CloudProvider.Azure,
          os: HostEnvironment.Windows,
          language: Language.Fsharp,
          azureRuntimeVersion: AzureRuntimeVersion.Version_3
        },

        // FIREBASE
        {
          title: 'Firebase - Linux Nodejs',
          cloud: CloudProvider.Firebase,
          os: HostEnvironment.Linux,
          language: Language.Nodejs,
          azureRuntimeVersion: AzureRuntimeVersion.Not_Azure
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
          benchmarkOptions.language,
          benchmarkOptions.azureRuntimeVersion
        );
        this.benchMarkData = benchMarkData;
      }
      this.isLoading = false;
    }
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