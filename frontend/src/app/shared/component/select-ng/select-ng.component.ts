import {
  Component,
  Input,
  Output,
  EventEmitter,
  forwardRef,
  OnInit,
} from '@angular/core';
import { FormControl, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Observable } from 'rxjs';
import { filter, first, flatMap, startWith } from 'rxjs/operators';
import { Option } from 'src/app/data/schema/option';

@Component({
  selector: 'app-select-ng',
  templateUrl: './select-ng.component.html',
  styleUrls: ['./select-ng.component.css'],
  // providers: [
  //   {
  //     provide: NG_VALUE_ACCESSOR,
  //     multi: true,
  //     useExisting: forwardRef(() => SelectNgComponent),
  //   },
  // ],
})
export class SelectNgComponent implements OnInit {
  @Input() id?: string;
  @Input() options$?: Observable<Option[]>;
  @Input() options?: Option[];
  @Output() changed: EventEmitter<Option> = new EventEmitter<Option>();
  @Input() placeholder = 'Selecciona una opción';
  @Input() form: FormControl;
  @Input() controlName: string;
  @Input() appendTo: string = null;
  /**
   * Must by a boolean value
   */
  @Input() conditionalCheck: string;
  /**
   * Must by a boolean value
   */
  @Input() conditionalNoCheck: string;
  @Input() initialDisabled = false;
  selectValue: any;

  constructor() {}

  ngOnInit(): void {
    if (this.conditionalCheck !== null && this.conditionalCheck !== undefined) {
      this.configConditionalCheck();
    } else if (
      this.conditionalNoCheck !== null &&
      this.conditionalNoCheck !== undefined
    ) {
      this.configConditionalNoCheck();
    }
  }

  configConditionalCheck() {
    this.form
      .get(this.conditionalCheck)
      ?.valueChanges.pipe(startWith([this.initialDisabled]))
      .subscribe((value) => {
        const control = this.form.get(this.controlName);
        if (value) {
          control.enable();
        } else {
          control.setValue(null);
          control.disable();
        }
        control.updateValueAndValidity();
      });
  }

  configConditionalNoCheck() {
    this.form
      .get(this.conditionalNoCheck)
      ?.valueChanges.pipe(startWith([this.initialDisabled]))
      .subscribe((value) => {
        const control = this.form.get(this.controlName);
        if (value) {
          control.disable();
        } else {
          control.setValue(null);
          control.enable();
        }
        control.updateValueAndValidity();
      });
  }

  dataCy(item: any): string {
    return `option-${this.controlName}-${item}`;
  }

  // propagateChange = (_: any) => {};

  onChange(value: any) {
    const o = this.options$
      .pipe(
        flatMap((x) => [...x]),
        filter((x) => x.id.toString() === value.toString()),
        first(),
      )
      .subscribe((x) => {
        this.changed.emit(x);
      });
    // this.propagateChange(value);
  }

  writeValue(obj: any): void {
    if (obj) {
      this.selectValue = obj;
    }
  }

  // registerOnChange(fn: any): void {
  //   this.propagateChange = fn;
  // }

  // registerOnTouched() {}

  filterOptions(query: string) {
    if (query !== '') {
      this.options$ = this.options$.pipe(
        filter(
          (x) =>
            x.filter((m) => m.name.toLowerCase().indexOf(query) > -1).length >
            0,
        ),
      );
    }
  }

  cleanUnnecessaryWhiteSpaces(cadena: string) {
    return cadena.replace(/\s{2,}/g, ' ').trim();
  }

  onValueChange() {
    this.changed.emit(this.form.get(this.controlName).value);
  }

  compareWith(item, selected): boolean {
    return item?.id === selected?.id;
  }
}
