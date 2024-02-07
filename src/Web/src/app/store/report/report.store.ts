import {Category} from "../../services/models/category.model";
import {Statistics} from "../../services/models/statistics.model";

export interface ReportStore {
  initialized: boolean;
  loading: boolean;
  refreshing: boolean;
  categories: Category[];
  selectedCategory?: Category;
  currentStatistics?: Statistics;
  cachedStatistics: Statistics[]
}
