import { Component, OnInit } from '@angular/core';
import { DataService } from './data.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-purchases',
  templateUrl: './purchases.component.html'
})
export class PurchasesComponent implements OnInit {
  purchases: any[] = [];
  items: any[] = [];
  purchaseForm!: FormGroup;
  modalVisible = false;
  editingId: string | null = null;

  constructor(public ds: DataService, private fb: FormBuilder) {}
  ngOnInit(): void { this.ds.getPurchases().subscribe(p => this.purchases = p); this.ds.getItems().subscribe(i => this.items = i);
    this.purchaseForm = this.fb.group({ date: [new Date().toISOString().slice(0,10), Validators.required], itemId: ['', Validators.required], qty: [1, [Validators.required, Validators.min(1)]] }); }

  openCreate() { this.editingId = null; this.purchaseForm.reset({ date: new Date().toISOString().slice(0,10), qty: 1, itemId: this.items[0]?.id || '' }); this.modalVisible = true; }

  openEdit(p: any) { this.editingId = p.id; this.purchaseForm.setValue({ date: p.date || new Date().toISOString().slice(0,10), itemId: p.itemId || (this.items[0]?.id || ''), qty: p.qty || 1 }); this.modalVisible = true; }

  save() {
    if(this.purchaseForm.invalid) return;
    const data = this.purchaseForm.value;
    if(this.editingId) this.ds.updatePurchase(this.editingId, data);
    else this.ds.addPurchase(data);
    this.modalVisible = false;
  }

  remove(id: string) { if(confirm('Delete purchase?')) this.ds.deletePurchase(id); }
}
