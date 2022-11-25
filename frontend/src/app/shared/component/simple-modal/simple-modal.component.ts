import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { RandomService } from '@shared/service/random.service';

@Component({
  selector: 'app-simple-modal',
  templateUrl: './simple-modal.component.html',
})
export class SimpleModalComponent {
  public static readonly SAVE_SUCCESS_MESSAGE = 'Operación Exitosa';
  public static readonly INVALID_FORM_MESSAGE = 'Formulario incompleto';
  public static readonly OPERATION_UNFINISHED_MESSAGE = 'No se pudo realizar la operación';
  public static readonly MODAL_TYPE_ERROR = 'warning';
  public static readonly MODAL_TYPE_INFO = 'info';
  public static readonly MODAL_TYPE_QUESTION = 'question';

  @Input() message: string = null;
  @Input() type: string = SimpleModalComponent.MODAL_TYPE_INFO;
  id: number = this.random.randomizeInteger();
  SAVE_SUCCESS_MESSAGE = SimpleModalComponent.SAVE_SUCCESS_MESSAGE;
  INVALID_FORM_MESSAGE = SimpleModalComponent.INVALID_FORM_MESSAGE;
  MODAL_TYPE_ERROR = SimpleModalComponent.MODAL_TYPE_ERROR;
  MODAL_TYPE_INFO = SimpleModalComponent.MODAL_TYPE_INFO;
  MODAL_TYPE_QUESTION = SimpleModalComponent.MODAL_TYPE_QUESTION;

  constructor(
    private random: RandomService,
    public activeModal: NgbActiveModal,
  ) {}
}
