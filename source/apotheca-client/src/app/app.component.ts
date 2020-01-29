import { Component, NgModule } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from './_services/authentication.service';
import { User } from './_models/user';
import { Store } from './_models/store';
import { StoreService } from './_services/store.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent {
  currentUser: User;
  constructor(
    private router: Router,
    private authenticationService: AuthenticationService,
    private storeService: StoreService
  ) 
  {
    this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
  }

  addStore() {
    this.storeService.openAddStoreDialog();
  }

  isLoggedIn() {
    return this.authenticationService.isLoggedIn();
  }

  loadStore(store) {
    let urlName = store.name.toLowerCase().replace(' ', '-');
    this.router.navigate(['/store', urlName, store.id]);
  }

  logout() {
      if (this.isLoggedIn()) {
        this.authenticationService.logout();
        this.router.navigate(['/login']);
      }
  }
}