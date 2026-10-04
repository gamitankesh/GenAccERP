import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { MainComponent } from './main.component';
import { UsersComponent } from './users.component';
import { ItemsComponent } from './items.component';
import { PurchasesComponent } from './purchases.component';
import { SalesComponent } from './sales.component';
import { ReportsComponent } from './reports.component';
import { ModalComponent } from './modal.component';

@NgModule({
  declarations: [
    App,
    MainComponent,
    UsersComponent,
    ItemsComponent,
    PurchasesComponent,
    SalesComponent,
    ReportsComponent
    ,ModalComponent
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    AppRoutingModule
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
  ],
  bootstrap: [App]
})
export class AppModule { }
