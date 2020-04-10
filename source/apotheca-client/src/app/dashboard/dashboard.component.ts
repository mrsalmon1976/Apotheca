import { Component, OnInit } from '@angular/core';
import { UserService } from '../_services/user.service';
import { forkJoin, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { AuthenticationService } from '../_services/authentication.service';
import { MatDialogRef, MatDialog } from '@angular/material';
import { StoreDialogComponent } from '../_components/shared/store-dialog/store-dialog.component';
import { StoreService } from '../_services/store.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: [ './dashboard.component.less' ]
})
export class DashboardComponent implements OnInit {

  constructor(private userService: UserService
    , private authenticationService: AuthenticationService
    , private storeService: StoreService
    ) { }

  ngOnInit() {
    this.authenticationService.getTokenExpirationDate();
    this.loadAll();
  }

  addStore() {
    this.storeService.openAddStoreDialog();
  }

  loadAll() {
    let loadUserAsync = this.loadUserAsync();
    forkJoin([loadUserAsync]).subscribe(results => {
      let user = results[0];
    });
  }

  loadUserAsync() : Observable<any> {
    //debugger;
    return this.userService.getUser('test');
  }

}