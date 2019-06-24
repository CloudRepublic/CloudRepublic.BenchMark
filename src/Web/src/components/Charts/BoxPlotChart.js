import 'chartjs-chart-box-and-violin-plot';
import { generateChart, mixins } from 'vue-chartjs';
import globalOptionsMixin from '@/components/Charts/globalOptionsMixin';

const { reactiveProp } = mixins;

const BoxPlot = generateChart('boxplot', 'boxplot');

export default {
  extends: BoxPlot,
  mixins: [reactiveProp, globalOptionsMixin],
  props: {
    extraOptions: {
      type: Object,
      default: () => ({})
    }
  },
  mounted() {
    // this.chartData is created in the mixin.
    // If you want to pass options please create a local options object
    this.renderChart(this.chartData, this.options);
  }
};
