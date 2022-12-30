import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-textarea-input',
  templateUrl: './textarea-input.component.html',
  styleUrls: ['./textarea-input.component.css'],
})
export class TextareaInputComponent implements OnInit {
  @Output() changed = new EventEmitter<boolean | null>();
  @Input() form: FormControl;
  @Input() controlName: string;
  @Input() readonly: boolean;
  @Input() rows = 3;

  value: string;

  ngOnInit(): void {
    this.value = this.form.get(this.controlName).value;
    this.form.get(this.controlName).valueChanges.subscribe((value: string) => {
      this.value = value;
    });
  }

  onValueChange() {
    this.changed.emit(this.form.get(this.controlName).value);
  }
}
