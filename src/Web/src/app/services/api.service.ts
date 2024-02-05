import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {map, Observable} from "rxjs";
import {Category} from "./models/category.model";
import {Statistics} from "./models/statistics.model";

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  baseurl = "https://serverlessbenchmark.cloudrepublic.nl/api"

  constructor(private httpClient: HttpClient) { }

  public getStatistics(category: Category): Observable<Statistics> {
    return this.httpClient.get<Statistics>(`${this.baseurl}/statistics`, {
      params: {
        cloudProvider: category.cloud,
        hostingEnvironment: category.os,
        runtime: category.runtime,
        language: category.language,
        sku: category.sku
      }
    })
  }

  public getCategories(): Observable<Category[]> {
    return this.httpClient.get<Category[]>(`${this.baseurl}/categories`)
  }
}
