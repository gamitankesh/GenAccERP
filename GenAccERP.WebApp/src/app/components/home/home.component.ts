import { Component } from '@angular/core';
import { InventoryService } from '../../services/inventory.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent {
  totalUsers = 0;
  totalItems = 0;
  totalPurchases = 0;
  totalSales = 0;

  constructor(private svc: InventoryService) {
    this.svc.users$.subscribe(u=> this.totalUsers = u.length);
    this.svc.items$.subscribe(i=> this.totalItems = i.length);
    this.svc.purchases$.subscribe(p=> this.totalPurchases = p.reduce((s,x)=>s+x.qty,0));
    this.svc.sales$.subscribe(s=> this.totalSales = s.reduce((acc,x)=>acc+x.qty,0));
  }
}
