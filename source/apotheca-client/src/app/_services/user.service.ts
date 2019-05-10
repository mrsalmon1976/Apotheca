import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { User } from '../_models/user';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient, private authService : AuthenticationService) { }

  getUser(id: string) : Observable<User> {
    const url = `${environment.apiUrl}/dashboard/userdetails/63d4e568-a4af-4370-b5e7-033b6fa7d9ff`;
    console.debug(`Request in getStores method: ${url}`);
    return this.http.get<User>(url, {
      headers: new HttpHeaders({
        "Authorization": "Bearer " + this.authService.token,
        "Content-Type": "application/json"
      })
    });
  }

  register(email: string, password: string, confirmPassword: string, firstName: string, lastName: string) {

    const url = `${environment.apiUrl}/account/register`;
    console.debug(`Request in register method: ${url}`);
    return this.http.post<any>(url, { email, password, confirmPassword, firstName, lastName })
        .pipe(map(user => {
            // // login successful if there's a jwt token in the response
            // if (user && user.token) {
            //     // store user details and jwt token in local storage to keep user logged in between page refreshes
            //     localStorage.setItem('currentUser', JSON.stringify(user));
            //     this.currentUserSubject.next(user);
            // }
            // else {
            //   // if we fail login, make sure we are logged out
            //   this.logout();
            // }
            return user;
        }));
  } 
}
