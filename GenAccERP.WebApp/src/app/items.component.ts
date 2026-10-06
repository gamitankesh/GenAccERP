import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { ModalComponent } from './modal.component';
import { Component, OnInit } from '@angular/core';
import { DataService } from './data.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-items',
  imports: [CommonModule, ReactiveFormsModule, ModalComponent],
  templateUrl: './items.component.html'
})
export class ItemsComponent implements OnInit {
  items: any[] = [];
  categories: any[] = [];
  itemForm!: FormGroup;
  modalVisible = false;
  editingId: string | null = null;

  constructor(public ds: DataService, private fb: FormBuilder) {}
  ngOnInit(): void { this.ds.getItems().subscribe(i => this.items = i); this.ds.getCategories().subscribe(c => this.categories = c);
    this.itemForm = this.fb.group({ itemNo: ['', Validators.required], name: ['', Validators.required], categoryId: ['', Validators.required], purchasePrice: [0, Validators.required], salesPrice: [0, Validators.required] }); }

  openCreate() { this.editingId = null; this.itemForm.reset({ purchasePrice: 0, salesPrice: 0 }); if(this.categories.length) this.itemForm.patchValue({ categoryId: this.categories[0].id }); this.modalVisible = true; }

  openEdit(item: any) { this.editingId = item.id; this.itemForm.setValue({ itemNo: item.itemNo || '', name: item.name || '', categoryId: item.categoryId || (this.categories[0]?.id || ''), purchasePrice: item.purchasePrice || 0, salesPrice: item.salesPrice || 0 }); this.modalVisible = true; }

  save() {
    if(this.itemForm.invalid) return;
    const data = this.itemForm.value;
    if(this.editingId) this.ds.updateItem(this.editingId, data);
    else this.ds.addItem(data);
    this.modalVisible = false;
  }

  remove(id: string) { if(confirm('Delete item?')) this.ds.deleteItem(id); }
}
