import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface User { id: string; username: string; email: string; role: string; createdBy: string; createdDate: string; modifiedBy?: string | null; modifiedDate?: string | null }
export interface Category { id: string; name: string; createdBy: string; createdDate: string }
export interface Item { id: string; categoryId: string; itemNo: string; name: string; desc?: string; purchasePrice: number; salesPrice: number; createdBy: string; createdDate: string; modifiedBy?: string | null; modifiedDate?: string | null }
export interface Purchase { id: string; date: string; itemId: string; qty: number; createdBy: string; createdDate: string; modifiedBy?: string | null; modifiedDate?: string | null }
export interface Sale { id: string; date: string; itemId: string; qty: number; createdBy: string; createdDate: string; modifiedBy?: string | null; modifiedDate?: string | null }

function uuidv4() {
  return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
    var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
    return v.toString(16);
  });
}
function getUtcNow() { return new Date().toISOString(); }

@Injectable({ providedIn: 'root' })
export class InventoryService {
  private systemUserId = uuidv4();

  private usersSubject = new BehaviorSubject<User[]>([]);
  users$ = this.usersSubject.asObservable();

  private categoriesSubject = new BehaviorSubject<Category[]>([]);
  categories$ = this.categoriesSubject.asObservable();

  private itemsSubject = new BehaviorSubject<Item[]>([]);
  items$ = this.itemsSubject.asObservable();

  private purchasesSubject = new BehaviorSubject<Purchase[]>([]);
  purchases$ = this.purchasesSubject.asObservable();

  private salesSubject = new BehaviorSubject<Sale[]>([]);
  sales$ = this.salesSubject.asObservable();

  constructor() {
    this.seed();
  }

  private seed() {
    const system = this.systemUserId;
    const users: User[] = [ { id: system, username: 'admin', email: 'admin@erp.local', role: 'Admin', createdBy: system, createdDate: getUtcNow(), modifiedBy: null, modifiedDate: null } ];

    const categories: Category[] = [ { id: uuidv4(), name: 'Electronics & Computing', createdBy: system, createdDate: getUtcNow() }, { id: uuidv4(), name: 'Office Furniture', createdBy: system, createdDate: getUtcNow() }, { id: uuidv4(), name: 'Stationery', createdBy: system, createdDate: getUtcNow() } ];

    const items: Item[] = [];
    items.push({ id: uuidv4(), categoryId: categories[0].id, itemNo: 'ITM-001', name: 'ThinkPad T14', desc: 'Business Laptop', purchasePrice: 1000, salesPrice: 1400, createdBy: system, createdDate: getUtcNow(), modifiedBy: null, modifiedDate: null });
    items.push({ id: uuidv4(), categoryId: categories[0].id, itemNo: 'ITM-002', name: '27" 4K Monitor', desc: 'High-res display', purchasePrice: 300, salesPrice: 450, createdBy: system, createdDate: getUtcNow(), modifiedBy: null, modifiedDate: null });
    items.push({ id: uuidv4(), categoryId: categories[1].id, itemNo: 'ITM-003', name: 'ErgoChair Pro', desc: 'Ergonomic Office Chair', purchasePrice: 150, salesPrice: 300, createdBy: system, createdDate: getUtcNow(), modifiedBy: null, modifiedDate: null });

    const purchases: Purchase[] = [ { id: uuidv4(), date: '2026-09-01', itemId: items[0].id, qty: 50, createdBy: system, createdDate: getUtcNow(), modifiedBy: null, modifiedDate: null }, { id: uuidv4(), date: '2026-09-02', itemId: items[1].id, qty: 100, createdBy: system, createdDate: getUtcNow(), modifiedBy: null, modifiedDate: null } ];

    const sales: Sale[] = [ { id: uuidv4(), date: '2026-09-10', itemId: items[0].id, qty: 5, createdBy: system, createdDate: getUtcNow(), modifiedBy: null, modifiedDate: null } ];

    this.usersSubject.next(users);
    this.categoriesSubject.next(categories);
    this.itemsSubject.next(items);
    this.purchasesSubject.next(purchases);
    this.salesSubject.next(sales);
  }

  // Users
  createUser(u: Omit<User, 'id'|'createdBy'|'createdDate'|'modifiedBy'|'modifiedDate'>) {
    const newUser: User = { id: uuidv4(), ...u, createdBy: this.systemUserId, createdDate: getUtcNow(), modifiedBy: null, modifiedDate: null };
    this.usersSubject.next([...this.usersSubject.value, newUser]);
  }
  updateUser(id: string, data: Partial<User>) { const arr = this.usersSubject.value.map(u=>u.id===id?{...u,...data,modifiedBy:this.systemUserId,modifiedDate:getUtcNow()}:u); this.usersSubject.next(arr); }
  deleteUser(id: string) { this.usersSubject.next(this.usersSubject.value.filter(u=>u.id!==id)); }

  // Categories
  getCategories() { return this.categoriesSubject.value; }

  // Items
  createItem(data: Omit<Item,'id'|'createdBy'|'createdDate'|'modifiedBy'|'modifiedDate'>) { const item: Item = { id: uuidv4(), ...data, createdBy: this.systemUserId, createdDate: getUtcNow(), modifiedBy: null, modifiedDate: null }; this.itemsSubject.next([...this.itemsSubject.value, item]); }
  updateItem(id: string, data: Partial<Item>) { const arr = this.itemsSubject.value.map(i=>i.id===id?{...i,...data,modifiedBy:this.systemUserId,modifiedDate:getUtcNow()}:i); this.itemsSubject.next(arr); }
  deleteItem(id: string) { this.itemsSubject.next(this.itemsSubject.value.filter(i=>i.id!==id)); }

  // Purchases
  createPurchase(data: Omit<Purchase,'id'|'createdBy'|'createdDate'|'modifiedBy'|'modifiedDate'>) { const p: Purchase = { id: uuidv4(), ...data, createdBy: this.systemUserId, createdDate: getUtcNow(), modifiedBy: null, modifiedDate: null }; this.purchasesSubject.next([...this.purchasesSubject.value, p]); }
  updatePurchase(id: string, data: Partial<Purchase>) { const arr = this.purchasesSubject.value.map(p=>p.id===id?{...p,...data,modifiedBy:this.systemUserId,modifiedDate:getUtcNow()}:p); this.purchasesSubject.next(arr); }
  deletePurchase(id: string) { this.purchasesSubject.next(this.purchasesSubject.value.filter(p=>p.id!==id)); }

  // Sales
  createSale(data: Omit<Sale,'id'|'createdBy'|'createdDate'|'modifiedBy'|'modifiedDate'>) { const s: Sale = { id: uuidv4(), ...data, createdBy: this.systemUserId, createdDate: getUtcNow(), modifiedBy: null, modifiedDate: null }; this.salesSubject.next([...this.salesSubject.value, s]); }
  updateSale(id: string, data: Partial<Sale>) { const arr = this.salesSubject.value.map(s=>s.id===id?{...s,...data,modifiedBy:this.systemUserId,modifiedDate:getUtcNow()}:s); this.salesSubject.next(arr); }
  deleteSale(id: string) { this.salesSubject.next(this.salesSubject.value.filter(s=>s.id!==id)); }

  // Stats
  getStockStats(itemId: string) {
    const purchased = this.purchasesSubject.value.filter(p => p.itemId === itemId).reduce((sum,p)=>sum+p.qty,0);
    const sold = this.salesSubject.value.filter(s => s.itemId === itemId).reduce((sum,s)=>sum+s.qty,0);
    return { purchased, sold, available: purchased - sold };
  }
}
