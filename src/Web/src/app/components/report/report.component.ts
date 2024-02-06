import {Component, OnInit} from '@angular/core';
import {EMPTY, Observable} from "rxjs";
import {MatCardModule} from "@angular/material/card";
import {Store} from "@ngrx/store";
import {Statistics} from "../../services/models/statistics.model";

import * as selectors from '../../store/report/report.selectors'
import {AsyncPipe, JsonPipe, NgIf} from "@angular/common";
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";
import {MatIconModule} from "@angular/material/icon";

@Component({
  selector: 'app-report',
  standalone: true,
  imports: [
    AsyncPipe,
    JsonPipe,
    NgIf,
    MatProgressSpinnerModule,
    MatCardModule,
    MatIconModule
  ],
  templateUrl: './report.component.html',
  styleUrl: './report.component.scss'
})
export class ReportComponent implements OnInit {
  public statistics$: Observable<Statistics | undefined> = EMPTY;
  public isRefreshing$: Observable<boolean> = EMPTY;

  constructor(private store: Store) {
  }

  ngOnInit(): void {
    this.statistics$ = this.store.select(selectors.getStatistics)
    this.isRefreshing$ = this.store.select(selectors.getIsRefreshing)
  }
}
