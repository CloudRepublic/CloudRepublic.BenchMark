import { createFeatureSelector, createSelector } from "@ngrx/store";
import { ReportStore } from "./report.store";
import {Median} from "./models/median.model";
import {GraphData} from "./models/graph-data.model";

const getReportState = createFeatureSelector<ReportStore>('report');

export const getCategories = createSelector(
  getReportState,
  (state) => state.categories
)

export const getSelectedCategory = createSelector(
  getReportState,
  (state) => state.selectedCategory
)

export const getColdMedian = createSelector(
    getReportState,
    (state) => !state.currentStatistics ? undefined : {
        medianExecutionTime: state.currentStatistics.coldMedianExecutionTime,
        previousDayDifference: state.currentStatistics.coldPreviousDayDifference,
        isPositiveDifference: state.currentStatistics.coldPreviousDayPositive,
        os: state.currentStatistics.hostingEnvironment,
        sku: state.currentStatistics.sku,
        cloud: state.currentStatistics.cloudProvider,
        language: state.currentStatistics.language,
        runtime: state.currentStatistics.runtime
    } as Median
)

export const getWarmMedian = createSelector(
  getReportState,
  (state) =>  !state.currentStatistics ? undefined : {
      medianExecutionTime: state.currentStatistics.warmMedianExecutionTime,
      previousDayDifference: state.currentStatistics.warmPreviousDayDifference,
      isPositiveDifference: state.currentStatistics.warmPreviousDayPositive,
      os: state.currentStatistics.hostingEnvironment,
      sku: state.currentStatistics.sku,
      cloud: state.currentStatistics.cloudProvider,
      language: state.currentStatistics.language,
      runtime: state.currentStatistics.runtime
    } as Median
)

// export const getColdGraphData = createSelector(
//   getReportState,
//   (state) => !state.currentStatistics ? undefined : {
//     os: state.currentStatistics.hostingEnvironment,
//     sku: state.currentStatistics.sku,
//     cloud: state.currentStatistics.cloudProvider,
//     language: state.currentStatistics.language,
//     runtime: state.currentStatistics.runtime,
//     dataPoints: state.currentStatistics.coldDataPoints
//   } as GraphData
// )

// export const getWarmGraphData = createSelector(
//   getReportState,
//   (state) => !state.currentStatistics ? undefined : {
//     os: state.currentStatistics.hostingEnvironment,
//     sku: state.currentStatistics.sku,
//     cloud: state.currentStatistics.cloudProvider,
//     language: state.currentStatistics.language,
//     runtime: state.currentStatistics.runtime,
//     dataPoints: state.currentStatistics.warmDataPoints
//   } as GraphData
// )

export const getGraphData = createSelector(
  getReportState,
  (state) => !state.currentStatistics ? undefined : {
    os: state.currentStatistics.hostingEnvironment,
    sku: state.currentStatistics.sku,
    cloud: state.currentStatistics.cloudProvider,
    language: state.currentStatistics.language,
    runtime: state.currentStatistics.runtime,
    warmDataPoints: state.currentStatistics.warmDataPoints,
    coldDataPoints: state.currentStatistics.coldDataPoints
  } as GraphData
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
