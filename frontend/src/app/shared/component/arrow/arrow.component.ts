import { Component, Input } from '@angular/core';

@Component({
  selector: 'arrow',
  templateUrl: './arrow.component.html',
})
export class ArrowComponent {
  @Input() left: boolean;
  @Input() right: boolean;
}
