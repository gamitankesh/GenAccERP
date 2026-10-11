import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, firstValueFrom } from 'rxjs';
import { ListRefreshService } from './list-refresh.service';

// Base URL of the GenAccERP backend API
export const API_BASE_URL = 'http://localhost:5277/api';

/**
 * All reads and writes go through the backend API. The BehaviorSubjects below are only
 * a client-side cache of the last API response so components can subscribe to them.
 */
@Injectable({ providedIn: 'root' })
export class DataService {
  private users$ = new BehaviorSubject<any[]>([]);
  private categories$ = new BehaviorSubject<any[]>([]);
  private items$ = new BehaviorSubject<any[]>([]);
  private purchases$ = new BehaviorSubject<any[]>([]);
  private sales$ = new BehaviorSubject<any[]>([]);

  constructor(private http: HttpClient, private lr: ListRefreshService) {
    this.loadAll();
  }

  // ---- mapping API DTOs <-> UI models ----
  private toUser = (u: any) => ({ id: u.userId, username: u.username, email: u.email, role: u.role });
  private toCategory = (c: any) => ({ id: c.categoryId, name: c.categoryName, createdDate: c.createdDate });
  private toItem = (i: any) => ({
    id: i.itemId, categoryId: i.categoryId, itemNo: i.itemNo, name: i.itemName, desc: i.itemDescription,
    purchasePrice: i.purchasePrice, salesPrice: i.salesPrice, currentStock: i.currentStock
  });
  private toPurchase = (p: any) => ({
    id: p.purchaseId, date: (p.purchaseDate || '').slice(0, 10), itemId: p.lines?.[0]?.itemId,
    qty: p.lines?.[0]?.quantity ?? 0, createdDate: p.createdDate
  });
  private toSale = (s: any) => ({
    id: s.saleId, date: (s.saleDate || '').slice(0, 10), itemId: s.lines?.[0]?.itemId,
    qty: s.lines?.[0]?.quantity ?? 0, createdDate: s.createdDate
  });

  // ---- loading ----
  async loadAll() {
    await Promise.all([this.loadUsers(), this.loadCategories(), this.loadItems(), this.loadPurchases(), this.loadSales()]);
  }
  private async load(url: string, map: (x: any) => any, subject: BehaviorSubject<any[]>) {
    try {
      const rows = await firstValueFrom(this.http.get<any[]>(`${API_BASE_URL}/${url}`));
      subject.next(rows.map(map));
    } catch (e) { this.fail(e); }
  }
  loadUsers() { return this.load('users', this.toUser, this.users$); }
  loadCategories() { return this.load('category', this.toCategory, this.categories$); }
  loadItems() { return this.load('items', this.toItem, this.items$); }
  loadPurchases() { return this.load('purchases', this.toPurchase, this.purchases$); }
  loadSales() { return this.load('sales', this.toSale, this.sales$); }

  private fail(e: any) {
    const msg = typeof e?.error === 'string' && e.error ? e.error : (e?.message || 'Request failed');
    console.error(e);
    alert(msg);
  }

  // Read observables
  getUsers() { return this.users$.asObservable(); }
  getCategories() { return this.categories$.asObservable(); }
  getItems() { return this.items$.asObservable(); }
  getPurchases() { return this.purchases$.asObservable(); }
  getSales() { return this.sales$.asObservable(); }

  // Users
  async addUser(u: any) {
    try { await firstValueFrom(this.http.post(`${API_BASE_URL}/users`, u)); await this.lr.refreshList('users'); } catch (e) { this.fail(e); }
  }
  async updateUser(id: string, data: any) {
    try { await firstValueFrom(this.http.put(`${API_BASE_URL}/users/${id}`, data)); await this.lr.refreshList('users'); } catch (e) { this.fail(e); }
  }
  async deleteUser(id: string) {
    try { await firstValueFrom(this.http.delete(`${API_BASE_URL}/users/${id}`)); await this.lr.refreshList('users'); } catch (e) { this.fail(e); }
  }

  // Categories
  async addCategory(name: string) {
    try {
      const c = await firstValueFrom(this.http.post<any>(`${API_BASE_URL}/category`, { categoryName: name }));
      await this.lr.refreshList('categories');
      return this.toCategory(c);
    } catch (e) { this.fail(e); return null; }
  }

  // Items
  private itemPayload(i: any) {
    return { categoryId: i.categoryId, itemNo: i.itemNo, itemName: i.name, itemDescription: i.desc, purchasePrice: i.purchasePrice, salesPrice: i.salesPrice };
  }
  async addItem(i: any) {
    try { await firstValueFrom(this.http.post(`${API_BASE_URL}/items`, this.itemPayload(i))); await this.lr.refreshList('items'); } catch (e) { this.fail(e); }
  }
  async updateItem(id: string, data: any) {
    try { await firstValueFrom(this.http.put(`${API_BASE_URL}/items/${id}`, this.itemPayload(data))); await this.lr.refreshList('items'); } catch (e) { this.fail(e); }
  }
  async deleteItem(id: string) {
    try { await firstValueFrom(this.http.delete(`${API_BASE_URL}/items/${id}`)); await this.lr.refreshList('items'); } catch (e) { this.fail(e); }
  }

  // Purchases (stock is maintained by the database)
  private txPayload(p: any) { return { itemId: p.itemId, quantity: p.qty, purchaseDate: p.date }; }
  async addPurchase(p: any) {
    try { await firstValueFrom(this.http.post(`${API_BASE_URL}/purchases`, this.txPayload(p))); await this.lr.refreshList('purchases', 'items'); } catch (e) { this.fail(e); }
  }
  async updatePurchase(id: string, data: any) {
    try { await firstValueFrom(this.http.put(`${API_BASE_URL}/purchases/${id}`, this.txPayload(data))); await this.lr.refreshList('purchases', 'items'); } catch (e) { this.fail(e); }
  }
  async deletePurchase(id: string) {
    try { await firstValueFrom(this.http.delete(`${API_BASE_URL}/purchases/${id}`)); await this.lr.refreshList('purchases', 'items'); } catch (e) { this.fail(e); }
  }

  // Sales
  private salePayload(s: any) { return { itemId: s.itemId, quantity: s.qty, saleDate: s.date }; }
  async addSale(s: any) {
    try { await firstValueFrom(this.http.post(`${API_BASE_URL}/sales`, this.salePayload(s))); await this.lr.refreshList('sales', 'items'); } catch (e) { this.fail(e); }
  }
  async updateSale(id: string, data: any) {
    try { await firstValueFrom(this.http.put(`${API_BASE_URL}/sales/${id}`, this.salePayload(data))); await this.lr.refreshList('sales', 'items'); } catch (e) { this.fail(e); }
  }
  async deleteSale(id: string) {
    try { await firstValueFrom(this.http.delete(`${API_BASE_URL}/sales/${id}`)); await this.lr.refreshList('sales', 'items'); } catch (e) { this.fail(e); }
  }

  // Query helpers (computed from API data loaded into the cache)
  getStockStats(itemId: string) {
    const purchased = this.purchases$.value.filter(p => p.itemId === itemId).reduce((sum, p) => sum + p.qty, 0);
    const sold = this.sales$.value.filter(s => s.itemId === itemId).reduce((sum, s) => sum + s.qty, 0);
    return { purchased, sold, available: purchased - sold };
  }
}
