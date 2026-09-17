import { Component } from '@angular/core';
import { InventoryService, Sale, Item } from '../../services/inventory.service';

@Component({
  selector: 'app-sales',
  templateUrl: './sales.component.html'
})
export class SalesComponent {
  sales: Sale[] = [];
  items: Item[] = [];
  constructor(private svc: InventoryService) {
    this.svc.sales$.subscribe(s=> this.sales = s);
    this.svc.items$.subscribe(i=> this.items = i);
  }

  addSale() {
    const itemId = this.items[0]?.id; if(!itemId) return alert('No items');
    const qty = parseInt(prompt('Quantity')||'0',10);
    this.svc.createSale({ date: new Date().toISOString().slice(0,10), itemId, qty });
  }

  deleteSale(id: string) { if(confirm('Delete sale?')) this.svc.deleteSale(id); }
}
