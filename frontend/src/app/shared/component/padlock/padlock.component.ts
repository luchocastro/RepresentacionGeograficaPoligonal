import { Component, Input } from '@angular/core';

@Component({
  selector: 'padlock',
  templateUrl: './padlock.component.html',
})
export class PadlockComponent {
  @Input() isOpen: boolean;
}
