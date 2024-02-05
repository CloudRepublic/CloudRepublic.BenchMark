import {Component, OnInit} from '@angular/core';
import {EMPTY, Observable} from "rxjs";
import {MatCard, MatCardContent} from "@angular/material/card";
import {Store} from "@ngrx/store";
import {Statistics} from "../../services/models/statistics.model";

import * as selectors from '../../store/report/report.selectors'
import {AsyncPipe, JsonPipe, NgIf} from "@angular/common";

@Component({
  selector: 'app-report',
  standalone: true,
  imports: [
    AsyncPipe,
    JsonPipe,
    NgIf
  ],
  templateUrl: './report.component.html',
  styleUrl: './report.component.scss'
})
export class ReportComponent implements OnInit {
  public statistics$: Observable<Statistics | undefined> = EMPTY;

  constructor(private store: Store) {
  }

  ngOnInit(): void {
    this.statistics$ = this.store.select(selectors.getStatistics)
  }
}
