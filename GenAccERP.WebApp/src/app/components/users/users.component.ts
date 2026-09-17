import { Component } from '@angular/core';
import { InventoryService, User } from '../../services/inventory.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html'
})
export class UsersComponent {
  users: User[] = [];
  constructor(private svc: InventoryService) {
    this.svc.users$.subscribe(u=> this.users = u);
  }

  addUser() {
    const username = prompt('Username');
    if(!username) return;
    const email = prompt('Email') || '';
    const role = prompt('Role') || 'User';
    this.svc.createUser({ username, email, role });
  }

  deleteUser(id: string) { if(confirm('Delete user?')) this.svc.deleteUser(id); }
}
