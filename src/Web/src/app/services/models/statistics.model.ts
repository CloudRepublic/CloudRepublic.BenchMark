import {Datapoint} from "./datapoint.model";

export interface Statistics {
  cloudProvider: string,
  hostingEnvironment: string,
  runtime: string,
  language: string,
  sku: string,
  coldPreviousDayPositive: boolean,
  coldPreviousDayDifference: number,
  warmPreviousDayPositive: boolean,
  warmPreviousDayDifference: number,
  coldMedianExecutionTime: number,
  warmMedianExecutionTime: number,
  coldDataPoints: Datapoint[],
  warmDataPoints: Datapoint[]
}
