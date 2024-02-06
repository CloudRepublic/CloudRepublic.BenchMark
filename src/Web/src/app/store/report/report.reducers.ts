import { createReducer, on } from '@ngrx/store';
import {categoriesLoaded, reportLoaded, selectTestReport} from './report.actions';
import { ReportStore } from "./report.store";

export const initialState: ReportStore = {
  initialized: false,
  loading: false,
  refreshing: false,
  categories: [],
  currentStatistics: undefined,
  cachedStatistics: []
};

export const reportReducers = createReducer(
  initialState,
  on(selectTestReport, (state, action) => {
    let cachedReport = state.cachedStatistics.find(s =>
      s.sku == action.category.sku &&
         s.runtime == action.category.runtime &&
         s.language == action.category.language &&
         s.hostingEnvironment == action.category.os &&
         s.cloudProvider == action.category.cloud
    )

    return {
      ...state,
      loading: !cachedReport,
      refreshing: !!cachedReport,
      selectedCategory: action.category,
      currentStatistics: cachedReport ?? state.currentStatistics
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
    const stats = state.cachedStatistics.filter(s => {
      return s.cloudProvider != action.statistics.cloudProvider ||
          s.hostingEnvironment != action.statistics.hostingEnvironment ||
          s.language != action.statistics.language ||
          s.runtime != action.statistics.runtime ||
          s.sku != action.statistics.sku
    });

    return {
      ...state,
      loading: false,
      refreshing: false,
      currentStatistics: action.statistics,
      cachedStatistics: [
          ...stats,
          action.statistics
      ]
    }
  })
);
