import { Component, OnInit } from '@angular/core';
import { DataService } from './data.service';

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html'
})
export class ReportsComponent implements OnInit {
  items: any[] = [];
  constructor(public ds: DataService) {}
  ngOnInit(): void { this.ds.getItems().subscribe(i => this.items = i); }
}
