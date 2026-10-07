import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { provideHttpClient } from '@angular/common/http';
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
  // App is an NgModule-owned root component (not standalone) and is declared here.
  declarations: [ App ],
  // Standalone components must be imported, not declared.
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    AppRoutingModule,
    MainComponent,
    UsersComponent,
    ItemsComponent,
    PurchasesComponent,
    SalesComponent,
    ReportsComponent,
    ModalComponent
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(),
  ],
  bootstrap: [App]
})
export class AppModule { }
