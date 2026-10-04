import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main.component';
import { UsersComponent } from './users.component';
import { ItemsComponent } from './items.component';
import { PurchasesComponent } from './purchases.component';
import { SalesComponent } from './sales.component';
import { ReportsComponent } from './reports.component';

const routes: Routes = [
  { path: '', component: MainComponent },
  { path: 'users', component: UsersComponent },
  { path: 'items', component: ItemsComponent },
  { path: 'purchases', component: PurchasesComponent },
  { path: 'sales', component: SalesComponent },
  { path: 'reports', component: ReportsComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
