import { createReducer, on } from '@ngrx/store';
import {categoriesLoaded, reportLoaded, selectTestReport} from './report.actions';
import { ReportStore } from "./report.store";

export const initialState: ReportStore = {
  initialized: false,
  loading: false,
  categories: [],
  currentStatistics: undefined
};

export const reportReducers = createReducer(
  initialState,
  on(selectTestReport, (state, action) => {
    return {
      ...state,
      loading: true,
      selectedCategory: action.category
    }
  }),

  on(categoriesLoaded, (state, action) => {
    return {
      ...state,
      initialized: true,
      categories: action.categories
    }
  }),

  on(reportLoaded, (state, action) => {
    return {
      ...state,
      loading: false,
      currentStatistics: action.statistics
    }
  })
);
