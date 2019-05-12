import { Component, OnInit } from '@angular/core';
import { UserService } from '../_services/user.service';
import { forkJoin, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { AuthenticationService } from '../_services/authentication.service';


@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: [ './dashboard.component.less' ]
})
export class DashboardComponent implements OnInit {

  constructor(private userService: UserService, private authenticationService: AuthenticationService) { }

  ngOnInit() {
    //this.loadAll();
  }

  loadAll() {
    let loadUserAsync = this.loadUserAsync();
    forkJoin([loadUserAsync]).subscribe(results => {
      let user = results[0];
    });
  }

  loadUserAsync() : Observable<any> {
    return this.userService.getUser('test');
  }

}