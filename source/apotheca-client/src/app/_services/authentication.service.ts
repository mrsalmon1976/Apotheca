import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { User } from '../_models/user';
import * as jwt_decode from 'jwt-decode';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {

    private currentUserSubject: BehaviorSubject<User>;
    public currentUser: Observable<User>;

    constructor(private http: HttpClient) {
        this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
        this.currentUser = this.currentUserSubject.asObservable();
    }

    public get currentUserValue(): User {
        return this.currentUserSubject.value;
    }

    public get token(): string {
        if (this.currentUserValue != null) {
            return this.currentUserValue.token;
        }
        return "";
    }

    getTokenExpirationDate() : Date {
        if(!this.token) return null;
        const decoded = jwt_decode(this.token);
        if (decoded.exp === undefined) return null;
        const date = new Date(0); 
        date.setUTCSeconds(decoded.exp);
        return date;    
    }
    
    isLoggedIn() {
        return (this.currentUserValue != null);
    }

    isTokenExpired(): boolean {
        const date = this.getTokenExpirationDate();
        if(date === undefined) return false;
        return !(date.valueOf() > new Date().valueOf());
    }    

    login(email: string, password: string) {
      const url = `${environment.apiUrl}/account/login`;
      console.debug(`Request in login method: ${url}`);
      return this.http.post<any>(url, { email, password })
          .pipe(map(user => {
              // login successful if there's a jwt token in the response
              if (user && user.token) {
                  // store user details and jwt token in local storage to keep user logged in between page refreshes
                  localStorage.setItem('currentUser', JSON.stringify(user));
                  this.currentUserSubject.next(user);
              }
              else {
                // if we fail login, make sure we are logged out
                this.logout();
              }
              return user;
          }));
    }

    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem('currentUser');
        this.currentUserSubject.next(null);
    }
}