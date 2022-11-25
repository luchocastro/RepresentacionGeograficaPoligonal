import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TextareaInputComponent } from './textarea-input.component';

describe('TextareaInputComponent', () => {
  let component: TextareaInputComponent;
  let fixture: ComponentFixture<TextareaInputComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TextareaInputComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TextareaInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
