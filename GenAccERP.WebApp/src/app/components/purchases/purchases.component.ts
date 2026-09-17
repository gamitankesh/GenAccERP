import { Component } from '@angular/core';
import { InventoryService, Purchase, Item } from '../../services/inventory.service';

@Component({
  selector: 'app-purchases',
  templateUrl: './purchases.component.html'
})
export class PurchasesComponent {
  purchases: Purchase[] = [];
  items: Item[] = [];
  constructor(private svc: InventoryService) {
    this.svc.purchases$.subscribe(p=> this.purchases = p);
    this.svc.items$.subscribe(i=> this.items = i);
  }

  addPurchase() {
    const itemId = this.items[0]?.id; if(!itemId) return alert('No items');
    const qty = parseInt(prompt('Quantity')||'0',10);
    this.svc.createPurchase({ date: new Date().toISOString().slice(0,10), itemId, qty });
  }

  deletePurchase(id: string) { if(confirm('Delete purchase?')) this.svc.deletePurchase(id); }
}
