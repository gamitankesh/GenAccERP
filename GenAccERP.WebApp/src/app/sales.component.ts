import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { ModalComponent } from './modal.component';
import { Component, OnInit } from '@angular/core';
import { DataService } from './data.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-sales',
  imports: [CommonModule, ReactiveFormsModule, ModalComponent],
  templateUrl: './sales.component.html'
})
export class SalesComponent implements OnInit {
  sales: any[] = [];
  items: any[] = [];
  saleForm!: FormGroup;
  modalVisible = false;
  editingId: string | null = null;

  constructor(public ds: DataService, private fb: FormBuilder) {}
  ngOnInit(): void { this.ds.getSales().subscribe(s => this.sales = s); this.ds.getItems().subscribe(i => this.items = i);
    this.saleForm = this.fb.group({ date: [new Date().toISOString().slice(0,10), Validators.required], itemId: ['', Validators.required], qty: [1, [Validators.required, Validators.min(1)]] }); }

  openCreate() { this.editingId = null; this.saleForm.reset({ date: new Date().toISOString().slice(0,10), qty: 1, itemId: this.items[0]?.id || '' }); this.modalVisible = true; }

  openEdit(s: any) { this.editingId = s.id; this.saleForm.setValue({ date: s.date || new Date().toISOString().slice(0,10), itemId: s.itemId || (this.items[0]?.id || ''), qty: s.qty || 1 }); this.modalVisible = true; }

  save() {
    if(this.saleForm.invalid) return;
    const data = this.saleForm.value;
    const stats = this.ds.getStockStats(data.itemId);
    // if editing, allow existing qty adjustment by adding back existing sale qty
    if(this.editingId) {
      const existing = this.sales.find(x => x.id === this.editingId);
      if(existing && existing.itemId === data.itemId) {
        if(data.qty > stats.available + existing.qty) return alert('Quantity exceeds available stock');
      } else {
        if(data.qty > stats.available) return alert('Quantity exceeds available stock');
      }
      this.ds.updateSale(this.editingId, data);
    } else {
      if(data.qty > stats.available) return alert('Quantity exceeds available stock');
      this.ds.addSale(data);
    }
    this.modalVisible = false;
  }

  remove(id: string) { if(confirm('Delete sale?')) this.ds.deleteSale(id); }
}
