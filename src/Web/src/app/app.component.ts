import {ChangeDetectionStrategy, Component, OnInit} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {AsyncPipe, NgIf} from "@angular/common";
import {LoadingOverlayComponent} from "./components/loading-overlay/loading-overlay.component";
import {CategoriesComponent} from "./components/categories/categories.component";
import {ReportComponent} from "./components/report/report.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, AsyncPipe, LoadingOverlayComponent, NgIf, CategoriesComponent, ReportComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AppComponent {

}
