import {ChangeDetectionStrategy, Component, input} from '@angular/core';
import {MatCard, MatCardContent, MatCardHeader} from "@angular/material/card";
import {MatIcon} from "@angular/material/icon";
import {NgIf} from "@angular/common";
import {Median} from "../../store/report/models/median.model";

@Component({
  selector: 'app-median',
  standalone: true,
    imports: [
        MatCard,
        MatCardContent,
        MatCardHeader,
        MatIcon,
        NgIf
    ],
  templateUrl: './median.component.html',
  styleUrl: './median.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MedianComponent {
  median = input.required<Median>()
  title = input.required<string>()
}
