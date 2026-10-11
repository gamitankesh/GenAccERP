import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { ModalComponent } from './modal.component';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { DataService } from './data.service';
import { ListRefreshService } from './list-refresh.service';

@Component({
  selector: 'app-reports',
  imports: [CommonModule],
  templateUrl: './reports.component.html'
})
export class ReportsComponent implements OnInit, OnDestroy {
  private unregister: Array<() => void> = [];
  items: any[] = [];
  constructor(public ds: DataService, private lr: ListRefreshService) {}
  ngOnInit(): void {
    this.unregister.push(this.lr.register('items', () => this.ds.loadItems()));
    this.ds.getItems().subscribe(i => this.items = i); }

  ngOnDestroy() { this.unregister.forEach(u => u()); }
}
