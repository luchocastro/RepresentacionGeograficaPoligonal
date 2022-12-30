import { Component, Output, EventEmitter, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Option } from 'src/app/data/schema/option';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
})
export class PaginationComponent implements OnInit {
  NEXT = 'next';
  FORWARD = 'forward';
  INPUT_PAGE_SIZE = 'pageSize';
  @Input() total: number = null;
  @Output() changed: EventEmitter<{
    pageNumber: number;
    pageSize: number;
  }> = new EventEmitter();

  currentPage = 1;
  sizeOptions: Option[] = [
    {
      name: '10',
      value: 10,
    },
    {
      name: '20',
      value: 20,
    },
    {
      name: '50',
      value: 50,
    },
    {
      name: '100',
      value: 100,
    },
  ];
  form: FormGroup;

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.buildForm();
  }

  get pageSize(): number {
    return this.form.get(this.INPUT_PAGE_SIZE).value;
  }

  get skip(): number {
    return (this.currentPage - 1) * this.pageSize;
  }

  buildForm() {
    this.form = this.fb.group({});
    this.form.addControl(this.INPUT_PAGE_SIZE, this.fb.control(10, []));
  }

  changePagination(direction: string) {
    this.currentPage =
      direction === this.NEXT ? this.currentPage + 1 : this.currentPage - 1;
    this.currentPage = this.currentPage < 1 ? 1 : this.currentPage;
    this.onSubmit();
  }

  onSubmit() {
    this.changed.next({
      pageNumber: this.currentPage,
      pageSize: this.pageSize,
    });
  }
}
