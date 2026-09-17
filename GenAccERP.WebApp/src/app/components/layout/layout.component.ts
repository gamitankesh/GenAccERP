import { Component } from '@angular/core';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css']
})
export class LayoutComponent {
  currentTab: string = 'home';

  switchTab(tab: string) { this.currentTab = tab; }
}
