import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

function uuidv4(): string {
  return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, c => {
    const r = Math.random() * 16 | 0;
    const v = c === 'x' ? r : (r & 0x3 | 0x8);
    return v.toString(16);
  });
}

@Injectable({ providedIn: 'root' })
export class DataService {
  private systemUserId = uuidv4();

  private users$ = new BehaviorSubject<any[]>([{
    id: this.systemUserId,
    username: 'admin',
    email: 'admin@erp.local',
    role: 'Admin',
    createdDate: new Date().toISOString()
  }]);

  private categories$ = new BehaviorSubject<any[]>([
    { id: uuidv4(), name: 'Electronics & Computing', createdDate: new Date().toISOString() },
    { id: uuidv4(), name: 'Office Furniture', createdDate: new Date().toISOString() },
    { id: uuidv4(), name: 'Stationery', createdDate: new Date().toISOString() }
  ]);

  private items$ = new BehaviorSubject<any[]>([
    { id: uuidv4(), categoryId: this.categories$.value[0].id, itemNo: 'ITM-001', name: 'ThinkPad T14', desc: 'Business Laptop', purchasePrice: 1000, salesPrice: 1400, createdDate: new Date().toISOString() },
    { id: uuidv4(), categoryId: this.categories$.value[0].id, itemNo: 'ITM-002', name: '27" 4K Monitor', desc: 'High-res display', purchasePrice: 300, salesPrice: 450, createdDate: new Date().toISOString() },
    { id: uuidv4(), categoryId: this.categories$.value[1].id, itemNo: 'ITM-003', name: 'ErgoChair Pro', desc: 'Ergonomic Office Chair', purchasePrice: 150, salesPrice: 300, createdDate: new Date().toISOString() }
  ]);

  private purchases$ = new BehaviorSubject<any[]>([
    { id: uuidv4(), date: '2026-09-01', itemId: this.items$.value[0].id, qty: 50, createdDate: new Date().toISOString() },
    { id: uuidv4(), date: '2026-09-02', itemId: this.items$.value[1].id, qty: 100, createdDate: new Date().toISOString() }
  ]);

  private sales$ = new BehaviorSubject<any[]>([
    { id: uuidv4(), date: '2026-09-10', itemId: this.items$.value[0].id, qty: 5, createdDate: new Date().toISOString() }
  ]);

  // Read observables
  getUsers() { return this.users$.asObservable(); }
  getCategories() { return this.categories$.asObservable(); }
  getItems() { return this.items$.asObservable(); }
  getPurchases() { return this.purchases$.asObservable(); }
  getSales() { return this.sales$.asObservable(); }

  // CRUD
  addUser(u: any) { const cur = this.users$.value; this.users$.next([...cur, { id: uuidv4(), ...u, createdDate: new Date().toISOString() }]); }
  deleteUser(id: string) { this.users$.next(this.users$.value.filter(x => x.id !== id)); }
  updateUser(id: string, data: any) {
    const cur = this.users$.value.map(u => u.id === id ? { ...u, ...data, modifiedDate: new Date().toISOString() } : u);
    this.users$.next(cur);
  }

  addItem(i: any) { const cur = this.items$.value; this.items$.next([...cur, { id: uuidv4(), ...i, createdDate: new Date().toISOString() }]); }
  deleteItem(id: string) { this.items$.next(this.items$.value.filter(x => x.id !== id)); }
  updateItem(id: string, data: any) {
    const cur = this.items$.value.map(it => it.id === id ? { ...it, ...data, modifiedDate: new Date().toISOString() } : it);
    this.items$.next(cur);
  }
  addCategory(name: string) { const cur = this.categories$.value; const c = { id: uuidv4(), name, createdDate: new Date().toISOString() }; this.categories$.next([...cur, c]); return c;
  }

  addPurchase(p: any) { const cur = this.purchases$.value; this.purchases$.next([...cur, { id: uuid4(), ...p, createdDate: new Date().toISOString() }]); }
  updatePurchase(id: string, data: any) {
    const cur = this.purchases$.value.map(pp => pp.id === id ? { ...pp, ...data, modifiedDate: new Date().toISOString() } : pp);
    this.purchases$.next(cur);
  }
  deletePurchase(id: string) { this.purchases$.next(this.purchases$.value.filter(x => x.id !== id)); }

  addSale(s: any) { const cur = this.sales$.value; this.sales$.next([...cur, { id: uuidv4(), ...s, createdDate: new Date().toISOString() }]); }
  updateSale(id: string, data: any) {
    const cur = this.sales$.value.map(ss => ss.id === id ? { ...ss, ...data, modifiedDate: new Date().toISOString() } : ss);
    this.sales$.next(cur);
  }
  deleteSale(id: string) { this.sales$.next(this.sales$.value.filter(x => x.id !== id)); }

  // Query helpers
  getStockStats(itemId: string) {
    const purchased = this.purchases$.value.filter(p => p.itemId === itemId).reduce((sum, p) => sum + p.qty, 0);
    const sold = this.sales$.value.filter(s => s.itemId === itemId).reduce((sum, s) => sum + s.qty, 0);
    return { purchased, sold, available: purchased - sold };
  }
}

function uuid4() { return uuidv4(); }
