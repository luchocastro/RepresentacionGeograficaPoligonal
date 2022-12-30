import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckInputComponent } from './checkbox-input.component';

describe('CheckInputComponent', () => {
  let component: CheckInputComponent;
  let fixture: ComponentFixture<CheckInputComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [CheckInputComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CheckInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
