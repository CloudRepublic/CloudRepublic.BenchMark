import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  Input,
  OnChanges,
  SimpleChanges,
  ViewChild
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
export class GraphComponent implements AfterViewInit, OnChanges {
  @ViewChild('canvas')
  canvas!: ElementRef

  @Input({required: true})
  graphData!: GraphData

  @Input({required: true})
  dataType!: 'cold' | 'warm'

  dataPointCount: number = 0

  private chart!: BoxPlotChart;

  ngAfterViewInit(): void {
    Chart.register(BoxPlotController, BoxAndWiskers, LinearScale, CategoryScale, Tooltip);
    const boxplotData = this.getBoxplotData();

    this.chart = new BoxPlotChart(this.canvas.nativeElement.getContext("2d"), {
      data: boxplotData,
      options: {
        maintainAspectRatio: false,
        responsive: true,
      }
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.dataPointCount = this.graphData.dataPoints.map(x => x.executionTimes.length).reduce((x, y) => x + y)

    if (!this.chart) {
      return;
    }

    this.chart.data = this.getBoxplotData();
    this.chart.update();
  }


  private getBoxplotData() {
    return {
      // define label tree
      labels: this.graphData.dataPoints.map(dp => dp.createdAt),
      datasets: [ {
        label: 'milliseconds',
        backgroundColor: 'rgba(94, 114, 228, 0.5)',
        borderColor: '#5e72e4',
        borderWidth: 1,
        outlierColor: 'rgba(94, 114, 228, 0.5)',
        padding: 10,
        itemRadius: 0,
        data: this.graphData.dataPoints.map(dp => dp.executionTimes)
    }]
    };
  }
}
