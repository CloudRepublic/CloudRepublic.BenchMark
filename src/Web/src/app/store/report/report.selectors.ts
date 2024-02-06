import { createFeatureSelector, createSelector } from "@ngrx/store";
import { ReportStore } from "./report.store";

const getReportState = createFeatureSelector<ReportStore>('report');

export const getCategories = createSelector(
  getReportState,
  (state) => state.categories
)

export const getSelectedCategory = createSelector(
  getReportState,
  (state) => state.selectedCategory
)

export const getStatistics = createSelector(
  getReportState,
  (state) => state.currentStatistics
)

export const getLoadingState = createSelector(
  getReportState,
  (state) => {
    return {
      initialized: state.initialized,
      loading: state.loading
    }
  }
)

export const getIsRefreshing = createSelector(
  getReportState,
  (state) => state.refreshing
)
