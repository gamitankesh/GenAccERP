import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { ModalComponent } from './modal.component';
import { Component, OnInit } from '@angular/core';
import { DataService } from './data.service';

@Component({
  selector: 'app-reports',
  imports: [CommonModule],
  templateUrl: './reports.component.html'
})
export class ReportsComponent implements OnInit {
  items: any[] = [];
  constructor(public ds: DataService) {}
  ngOnInit(): void { this.ds.getItems().subscribe(i => this.items = i); }
}
