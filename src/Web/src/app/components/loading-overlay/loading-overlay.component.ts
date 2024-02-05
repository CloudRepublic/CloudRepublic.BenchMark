import {ChangeDetectionStrategy, Component, OnInit} from '@angular/core';
import {MatProgressSpinner} from "@angular/material/progress-spinner";
import {debounceTime, switchMap, EMPTY, map, Observable, of, startWith, combineLatest, delay} from "rxjs";
import {Store} from "@ngrx/store";
import * as selectors from "../../store/report/report.selectors";
import {AsyncPipe, NgIf, NgStyle} from "@angular/common";
@Component({
  selector: 'app-loading-overlay',
  standalone: true,
  imports: [
    MatProgressSpinner,
    NgIf,
    AsyncPipe,
    NgStyle
  ],
  templateUrl: './loading-overlay.component.html',
  styleUrl: './loading-overlay.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoadingOverlayComponent implements OnInit {
  public showLoadingOverlay$: Observable<{
    showLoading: boolean
  }> = EMPTY

  constructor(private store: Store) {
  }

  ngOnInit(): void {
    let loadingState$ = this.store.select(selectors.getLoadingState);
    let showForInit$ = loadingState$.pipe(
      map(state => !state.initialized)
    )

    let showForLoading$ = loadingState$.pipe(
      switchMap(state => {
        if (state.loading) {
          return of(state.loading)
            .pipe(delay(100))
        }

        return of(state.loading)
      }),
      startWith(true)
    )

    this.showLoadingOverlay$ = combineLatest([showForInit$, showForLoading$])
      .pipe(
        map(([init, loading]) => ({
          showLoading: init || loading
        })),
        switchMap(showLoadingScreen => {
          console.log(showLoadingScreen)
          if (showLoadingScreen.showLoading) {
            return of(showLoadingScreen);
          }

          return of(showLoadingScreen).pipe(
            delay(400)
          )
        })
      )
  }
}
