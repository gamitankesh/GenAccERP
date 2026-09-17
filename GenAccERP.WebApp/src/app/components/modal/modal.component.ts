import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-modal',
  template: `
    <div class="modal-overlay" [class.active]="visible">
      <div class="modal">
        <div class="modal-header"><h3>{{ title }}</h3><button class="btn-action" (click)="close()">✕</button></div>
        <div class="modal-body"><ng-content></ng-content></div>
        <div class="modal-footer"><button class="btn btn-secondary" (click)="close()">Close</button></div>
      </div>
    </div>
  `
})
export class ModalComponent {
  @Input() visible = false;
  @Input() title = '';
  close() { this.visible = false; }
}
