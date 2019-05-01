import { TestBed } from '@angular/core/testing';
//import { HttpClientModule } from '@angular/common/http';
import { HttpClientTestingModule, HttpTestingController, TestRequest } from '@angular/common/http/testing';
import { AppModule } from '../app.module';
import { User } from '../_models/user';
import { AuthenticationService } from './authentication.service';
import { HttpBackend } from '@angular/common/http';
import { environment } from '../../environments/environment';

describe('AuthenticationService', () => {

  let httpMock: HttpTestingController;
  let authService: AuthenticationService;

  beforeEach((() => {

    environment.apiUrl = 'https://testurl/api';

    TestBed.configureTestingModule({
      imports:  [AppModule,HttpClientTestingModule],
      declarations: [ ],
      providers: [AuthenticationService]
    })
    .compileComponents();

    authService = TestBed.get(AuthenticationService);
    httpMock = TestBed.get(HttpTestingController);
  }));

  afterEach((() => {
    authService.logout();
  }));

  it('should be created', () => {
    expect(authService).toBeTruthy();
  });

  it('logout removes local storage', () => {
    let user = new User();
    user.userName = 'test';
    localStorage.setItem('currentUser', JSON.stringify(user));

    expect(localStorage.getItem('currentUser')).toEqual(JSON.stringify(user));

    authService.logout();

    expect(authService.isLoggedIn()).toBeFalsy();
    expect(authService.isLoggedIn()).toBeFalsy();
    expect(authService.currentUserValue).toBeNull();
    expect(localStorage.getItem('currentUser')).toBeNull();
  });

  it('login success sets local storage', () => {

    let url = `${environment.apiUrl}/account/login`;
    let user : string = 'test';

    // call the method
    authService.login('test', 'tpass').subscribe((data: any) => {
        expect(data.userName).toBe('test');
    });    
    const req = httpMock.expectOne(url, 'post to api');
    expect(req.request.method).toBe('POST');

    // force the flush of the mocked response
    req.flush({
        userName: user,
        token: 'adsdgafshgfhsgafdhgasfd'
    });
    httpMock.verify();

    // make sure the current user has been set on the local storage
    expect(localStorage.getItem('currentUser')).toBeDefined();
    expect(JSON.parse(localStorage.getItem('currentUser')).userName).toBe(user);

    // make sure the user name is set on the auth service
    expect(authService.currentUserValue.userName).toBe(user);
  });

  it('login failure does not set local storage', () => {

    let url = `${environment.apiUrl}/account/login`;
    let error : string = 'login failed'; 
    localStorage.removeItem('currentUser');

    // call the method
    authService.login('test', 'tpass').subscribe((data: any) => {
        expect(data.message).toBe(error);
    });    
    const req = httpMock.expectOne(url, 'post to api');
    expect(req.request.method).toBe('POST');

    // force the flush of the mocked response
    req.flush({
        message: error,
    });
    httpMock.verify();

    // make sure the current user has been set on the local storage
    expect(localStorage.getItem('currentUser')).toBeNull();

    // make sure the user name is set on the auth service
    expect(authService.currentUserValue).toBeNull();
  });


});