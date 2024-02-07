import {Component, OnInit} from '@angular/core';
import {Store} from "@ngrx/store";
import {EMPTY, Observable} from "rxjs";
import {Category} from "../../services/models/category.model";
import {NgForOf, NgIf} from "@angular/common";
import { PushPipe } from '@ngrx/component';

import * as selectors from '../../store/report/report.selectors'
import * as actions from '../../store/report/report.actions'

@Component({
  selector: 'app-categories',
  standalone: true,
  imports: [
    PushPipe,
    NgForOf,
    NgIf
  ],
  templateUrl: './categories.component.html',
  styleUrl: './categories.component.scss'
})
export class CategoriesComponent implements OnInit {
  public categories$: Observable<Category[]> = EMPTY;
  public current_category$: Observable<Category | undefined> = EMPTY;

  constructor(private store: Store) {
  }

  ngOnInit(): void {
    this.categories$ = this.store.select(selectors.getCategories);
    this.current_category$ = this.store.select(selectors.getSelectedCategory);
  }

  selectCategory(category: Category) {
    this.store.dispatch(actions.selectTestReport({
      category: category
    }))
  }

  areEqual(cat1: Category, cat2: Category) {
    return cat1.cloud == cat2.cloud &&
      cat1.os == cat2.os &&
      cat1.sku == cat2.sku &&
      cat1.runtime == cat2.runtime &&
      cat1.language == cat2.language
  }
}
