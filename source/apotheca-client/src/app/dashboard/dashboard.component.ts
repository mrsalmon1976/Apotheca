import { Component, OnInit } from '@angular/core';
import { UserService } from '../_services/user.service';
import { forkJoin, Observable } from 'rxjs';
import { environment } from '../../environments/environment';


@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: [ './dashboard.component.less' ]
})
export class DashboardComponent implements OnInit {

  constructor(private userService: UserService) { }

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