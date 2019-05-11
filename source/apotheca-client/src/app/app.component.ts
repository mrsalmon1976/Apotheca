import { Component, NgModule } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from './_services/authentication.service';
import { User } from './_models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent {
  currentUser: User;
  constructor(
    private router: Router,
    private authenticationService: AuthenticationService
  ) 
  {
    this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
  }

  isLoggedIn() {
    return this.authenticationService.isLoggedIn();
  }

  logout() {
      if (this.isLoggedIn()) {
        this.authenticationService.logout();
        this.router.navigate(['/login']);
      }
  }
}