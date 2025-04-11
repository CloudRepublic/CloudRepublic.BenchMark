import {
  ChangeDetectionStrategy,
  Component, computed, effect,
  ElementRef,
  input,
  Signal, viewChild
} from '@angular/core';
import {CategoryScale, Chart, LinearScale, Tooltip} from 'chart.js';
import {BoxAndWiskers, BoxPlotChart, BoxPlotController} from '@sgratzl/chartjs-chart-boxplot';
import {MatCard, MatCardContent, MatCardHeader} from "@angular/material/card";
import {GraphData} from "../../store/report/models/graph-data.model";
import {NgIf} from "@angular/common";


@Component({
  selector: 'app-graph',
  standalone: true,
  templateUrl: './graph.component.html',
  styleUrl: './graph.component.scss',
  imports: [
    MatCard,
    MatCardContent,
    MatCardHeader,
    NgIf
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class GraphComponent {
  canvas = viewChild.required<ElementRef<HTMLCanvasElement>>("canvas");
  graphData = input.required<GraphData>()
  dataType = input.required<'cold' | 'warm' | 'merged'>()
  dataPointCount: Signal<number> = computed(() => this.graphData().warmDataPoints.map(x => x.executionTimes.length).reduce((x, y) => x + y))
  private chart: Signal<BoxPlotChart | undefined> = computed(() => {
    let c = this.canvas().nativeElement?.getContext("2d");
    if (!c) {
      return;
    }

    return new BoxPlotChart(c, {
      data: {
        labels: [],
        datasets: []
      },
      options: {
        maintainAspectRatio: false,
        responsive: true,
      }
    })
  });

  constructor() {
    Chart.register(BoxPlotController, BoxAndWiskers, LinearScale, CategoryScale, Tooltip);

    effect(() => {
      const chart = this.chart();
      if (!chart) {
        return;
      }

      chart.data = {
        // define label tree
        labels: this.graphData().warmDataPoints.map(dp => dp.createdAt),
        datasets: [ {
          label: 'milliseconds',
          backgroundColor: 'rgba(94, 114, 228, 0.5)',
          borderColor: '#5e72e4',
          borderWidth: 1,
          itemRadius: 0,
          data: this.graphData().warmDataPoints.map(dp => dp.executionTimes)
        },
        // {
        //   label: 'milliseconds',
        //   backgroundColor: 'rgba(94, 114, 228, 0.5)',
        //   borderColor: '#5e72e4',
        //   borderWidth: 1,
        //   itemRadius: 0,
        //   data: this.graphData().coldDataPoints.map(dp => dp.executionTimes)
        // },
      ]
      }
      chart.update();
    })
  }
}
