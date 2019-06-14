<template>
  <div class="row justify-content-center">
    <div class="col-lg-5 col-md-7">
      <stats-card
        title="Function Windows"
        type="gradient-info"
        sub-title="350,897ms"
        icon="ni ni-active-40"
        class="mb-4 mb-xl-0"
      >
        <template slot="footer">
          <span class="text-success mr-2">
            <i class="fa fa-arrow-up"></i> 3.48%
          </span>
          <span class="text-nowrap">Since last 3 days</span>
        </template>
      </stats-card>
    </div>
  </div>
</template>
<script>
import * as chartConfigs from "@/components/Charts/config";
import LineChart from "@/components/Charts/LineChart";
import BarChart from "@/components/Charts/BarChart";

export default {
  components: {},
  name: "benchmark",
  data() {
    return {
      model: {}
    };
  }
};
</script>
<style>
</style>
