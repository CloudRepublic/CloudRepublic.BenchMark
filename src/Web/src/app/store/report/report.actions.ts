import { createAction, props } from '@ngrx/store';
import { Statistics } from "../../services/models/statistics.model";
import {Category} from "../../services/models/category.model";

export const selectTestReport = createAction('[Reports] select test report', props<{
  category: Category
}>());

export const reportLoaded = createAction('[Reports] report loaded', props<{
  statistics: Statistics
}>())
export const categoriesLoaded = createAction('[Reports] categories loaded', props<{
  categories: Category[]
}>())
