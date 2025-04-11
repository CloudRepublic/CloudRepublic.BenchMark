import {ChangeDetectionStrategy, Component, OnInit} from '@angular/core';
import {distinctUntilChanged, EMPTY, Observable} from "rxjs";
import {Store} from "@ngrx/store";

import * as selectors from '../../store/report/report.selectors'
import {JsonPipe, NgIf} from "@angular/common";
import {MedianComponent} from "../median/median.component";
import {Median} from "../../store/report/models/median.model";
import {GraphComponent} from "../graph/graph.component";
import {GraphData} from "../../store/report/models/graph-data.model";
import { PushPipe } from '@ngrx/component';
import {MatProgressSpinner} from "@angular/material/progress-spinner";

@Component({
  selector: 'app-report',
  standalone: true,
  imports: [
    PushPipe,
    JsonPipe,
    NgIf,
    MedianComponent,
    GraphComponent,
    MatProgressSpinner,
  ],
  templateUrl: './report.component.html',
  styleUrl: './report.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ReportComponent implements OnInit {
  isRefreshing$: Observable<boolean> = EMPTY;
  coldStartMedian$: Observable<undefined | Median> = EMPTY;
  warmStartMedian$: Observable<undefined | Median> = EMPTY;
  coldGraphData$: Observable<undefined | GraphData> = EMPTY;
  warmGraphData$: Observable<undefined | GraphData> = EMPTY;

  constructor(private store: Store) {
  }

  ngOnInit(): void {
    this.coldStartMedian$ = this.store.select(selectors.getColdMedian)
    this.warmStartMedian$ = this.store.select(selectors.getWarmMedian)
    this.coldGraphData$ = this.store.select(selectors.getGraphData).pipe(
      // we are refreshing data but don't want it to propagate if it is exactly the same as the previous value
      distinctUntilChanged((previous, current) => JSON.stringify(previous) == JSON.stringify(current)),
    )
    this.warmGraphData$ = this.store.select(selectors.getGraphData).pipe(
      // we are refreshing data but don't want it to propagate if it is exactly the same as the previous value
      distinctUntilChanged((previous, current) => JSON.stringify(previous) == JSON.stringify(current)),
    )
    this.isRefreshing$ = this.store.select(selectors.getIsRefreshing)
  }
}
