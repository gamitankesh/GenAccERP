import { Component, OnInit } from '@angular/core';
import { DataService } from './data.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html'
})
export class UsersComponent implements OnInit {
  users: any[] = [];
  userForm!: FormGroup;
  userModalVisible = false;
  editingId: string | null = null;

  constructor(public ds: DataService, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.ds.getUsers().subscribe(u => this.users = u);
    this.userForm = this.fb.group({ username: ['', Validators.required], email: ['', [Validators.required, Validators.email]], role: ['User', Validators.required] });
  }

  openCreate() { this.editingId = null; this.userForm.reset({ role: 'User' }); this.userModalVisible = true; }

  openEdit(user: any) {
    this.editingId = user.id;
    this.userForm.setValue({ username: user.username || '', email: user.email || '', role: user.role || 'User' });
    this.userModalVisible = true;
  }

  saveUser() {
    if (this.userForm.invalid) return;
    const data = this.userForm.value;
    if (this.editingId) {
      this.ds.updateUser(this.editingId, data);
    } else {
      this.ds.addUser(data);
    }
    this.userModalVisible = false;
  }

  closeUserModal() { this.userModalVisible = false; }

  remove(id: string) { if(confirm('Delete user?')) this.ds.deleteUser(id); }
}
