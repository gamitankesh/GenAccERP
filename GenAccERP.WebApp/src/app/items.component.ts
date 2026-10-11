import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { ModalComponent } from './modal.component';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
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

  constructor(public ds: DataService, private fb: FormBuilder, private cdr: ChangeDetectorRef) {}
  ngOnInit(): void {
    this.ds.getItems().subscribe(i => { this.items = i; this.cdr.markForCheck(); });
    this.ds.getCategories().subscribe(c => { this.categories = c; this.cdr.markForCheck(); });
    this.itemForm = this.fb.group({ itemNo: ['', Validators.required], name: ['', Validators.required], categoryId: ['', Validators.required], purchasePrice: [0, Validators.required], salesPrice: [0, Validators.required] }); }

  openCreate() { this.editingId = null; this.itemForm.reset({ purchasePrice: 0, salesPrice: 0 }); if(this.categories.length) this.itemForm.patchValue({ categoryId: this.categories[0].id }); this.modalVisible = true; }

  openEdit(item: any) { this.editingId = item.id; this.itemForm.setValue({ itemNo: item.itemNo || '', name: item.name || '', categoryId: item.categoryId || (this.categories[0]?.id || ''), purchasePrice: item.purchasePrice || 0, salesPrice: item.salesPrice || 0 }); this.modalVisible = true; }

  async save() {
    if(this.itemForm.invalid) return;
    const data = { ...this.itemForm.value, purchasePrice: Number(this.itemForm.value.purchasePrice) || 0, salesPrice: Number(this.itemForm.value.salesPrice) || 0 };
    this.modalVisible = false;
    if(this.editingId) await this.ds.updateItem(this.editingId, data);
    else await this.ds.addItem(data);
    this.cdr.markForCheck();
  }

  remove(id: string) { if(confirm('Delete item?')) this.ds.deleteItem(id); }

  onlyNumber(e: KeyboardEvent) {
    const input = e.target as HTMLInputElement;
    const ok = /^[0-9]$/.test(e.key) || (e.key === '.' && !input.value.includes('.'));
    if (!ok) e.preventDefault();
  }

  onlyNumberPaste(e: ClipboardEvent) {
    const text = e.clipboardData?.getData('text') ?? '';
    if (!/^\d*\.?\d*$/.test(text)) e.preventDefault();
  }
}
