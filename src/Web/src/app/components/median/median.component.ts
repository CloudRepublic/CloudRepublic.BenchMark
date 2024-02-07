import {ChangeDetectionStrategy, Component, Input} from '@angular/core';
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
  @Input({required: true})
  median!: Median

  @Input({required: true})
  title!: string
}
