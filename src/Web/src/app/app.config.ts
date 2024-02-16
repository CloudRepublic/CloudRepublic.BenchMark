import {ApplicationConfig, isDevMode, NgZone, ɵNoopNgZone} from '@angular/core';
import { provideStore } from '@ngrx/store';
import { reportReducers } from "./store/report/report.reducers";
import { provideStoreDevtools } from '@ngrx/store-devtools';
import { provideEffects } from '@ngrx/effects';
import {ReportEffects} from "./store/report/report.effects";
import {provideHttpClient} from "@angular/common/http";
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(),
    provideStore({
        report: reportReducers,
    }),
    provideStoreDevtools({ maxAge: 25, logOnly: !isDevMode() }),
    provideEffects([ReportEffects]),
    provideAnimationsAsync(),
    {provide: NgZone, useClass: ɵNoopNgZone}
  ]
};
