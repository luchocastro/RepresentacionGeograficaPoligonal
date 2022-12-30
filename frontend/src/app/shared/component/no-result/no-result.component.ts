import { Component, Input } from '@angular/core';

@Component({
  selector: 'no-result',
  templateUrl: './no-result.component.html',
  styles: ['div { padding-top: 8%; padding-bottom: 8% }'],
})
export class NoResultComponent {
  @Input() message: string;
}
