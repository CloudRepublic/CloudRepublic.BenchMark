import {Datapoint} from "../../../services/models/datapoint.model";

export interface GraphData {
  os: string;
  sku: string;
  cloud: string;
  runtime: string;
  language: string;
  dataPoints: Datapoint[]
}
