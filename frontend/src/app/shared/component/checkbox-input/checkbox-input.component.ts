import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-checkbox-input',
  templateUrl: './checkbox-input.component.html',
  styleUrls: ['./checkbox-input.component.css'],
})
export class CheckInputComponent {
  @Output() changed = new EventEmitter<any>();
  @Input() form: FormControl;
  @Input() controlName: string;
  @Input() placeholder = '';
  @Input() label  = 'Checkbox';

  onValueChange() {
    const control = this.form.get(this.controlName);
    const value = control.value;
    this.changed.emit(value);
  }
}
