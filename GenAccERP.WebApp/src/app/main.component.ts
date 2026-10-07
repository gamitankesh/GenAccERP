import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { ModalComponent } from './modal.component';
import { Component, OnInit } from '@angular/core';
import { DataService } from './data.service';

@Component({
  selector: 'app-main',
  imports: [CommonModule],
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {
  totals = { users: 0, items: 0, purchases: 0, sales: 0 };
  items: any[] = [];

  constructor(public ds: DataService) {}
  ngOnInit(): void {
    this.ds.getUsers().subscribe(u => this.totals.users = u.length);
    this.ds.getItems().subscribe(i => { this.totals.items = i.length; this.items = i; });
    this.ds.getPurchases().subscribe(p => this.totals.purchases = p.length);
    this.ds.getSales().subscribe(s => this.totals.sales = s.length);
  }
}
