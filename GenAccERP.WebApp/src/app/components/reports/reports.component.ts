import { Component } from '@angular/core';
import { InventoryService, Item } from '../../services/inventory.service';

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html'
})
export class ReportsComponent {
  items: Item[] = [];
  constructor(public svc: InventoryService) {
    this.svc.items$.subscribe(i=> this.items = i);
  }
}
