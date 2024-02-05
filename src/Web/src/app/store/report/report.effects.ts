import { Injectable } from '@angular/core';
import {Actions, createEffect, ofType, ROOT_EFFECTS_INIT} from '@ngrx/effects';
import { ApiService } from '../../services/api.service'
import {categoriesLoaded, reportLoaded, selectTestReport} from "./report.actions";
import {map, switchMap} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ReportEffects {

  constructor(private actions$: Actions, private api: ApiService) { }

  loadReport$ = createEffect(() => this.actions$.pipe(
    ofType(selectTestReport),
    switchMap((action) => this.api.getStatistics(action.category)
      .pipe(map((stats) => reportLoaded({
        statistics: stats
      })))
    )
  ))

  loadCategories$ = createEffect(() => this.actions$.pipe(
    ofType(ROOT_EFFECTS_INIT),
    switchMap((action) => this.api.getCategories()
      .pipe(
        map(categories => categoriesLoaded({categories: categories}))
      ))
  ))

  initialReportLoad$ = createEffect(() => this.actions$.pipe(
    ofType(categoriesLoaded),
    map(categoriesLoadedAction => {
      const firstCategory = categoriesLoadedAction.categories[0]
      return selectTestReport({
        category: firstCategory
      })
    })
  ))
}
