import { Component } from '@angular/core';
import { InventoryService, Item, Category } from '../../services/inventory.service';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html'
})
export class CategoriesComponent {
  categories: Category[] = [];
  items: Item[] = [];

  constructor(public svc: InventoryService) {
    this.categories = svc.getCategories();
    this.svc.items$.subscribe(i=> this.items = i);
  }

  getItemsForCategory(catId: string) { return this.items.filter(x=>x.categoryId===catId); }

  addItem() {
    const itemNo = prompt('Item No'); if(!itemNo) return;
    const name = prompt('Name') || '';
    const categoryId = this.categories[0]?.id || '';
    this.svc.createItem({ itemNo, categoryId, name, desc: '', purchasePrice: 0, salesPrice: 0 });
  }

  deleteItem(id: string) { if(confirm('Delete item?')) this.svc.deleteItem(id); }
}
