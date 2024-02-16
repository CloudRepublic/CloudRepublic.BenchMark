import {ChangeDetectionStrategy, Component} from '@angular/core';
import {AsyncPipe, NgIf} from "@angular/common";
import {LoadingOverlayComponent} from "./components/loading-overlay/loading-overlay.component";
import {CategoriesComponent} from "./components/categories/categories.component";
import {ReportComponent} from "./components/report/report.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [ AsyncPipe, LoadingOverlayComponent, NgIf, CategoriesComponent, ReportComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AppComponent {
  currentYear = new Date().getFullYear();
}
