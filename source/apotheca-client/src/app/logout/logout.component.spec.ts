import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LogoutComponent } from './logout.component';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientModule } from '@angular/common/http';
import {Router} from "@angular/router";
import { AppModule } from '../app.module';
import { AuthenticationService } from '../_services/authentication.service';

describe('LogoutComponent', () => {
  let component: LogoutComponent;
  let fixture: ComponentFixture<LogoutComponent>;
  let router = {
    navigate: jasmine.createSpy('navigate')
  }
  let authServiceSpy: jasmine.SpyObj<AuthenticationService>;
  
  beforeEach(async(() => {

    authServiceSpy = jasmine.createSpyObj('AuthenticationService', ['logout']);
    TestBed.configureTestingModule({
      imports:  [AppModule,HttpClientModule,RouterTestingModule],
      declarations: [ ],
      providers: [LogoutComponent, { provide: AuthenticationService, useValue: authServiceSpy }, { provide: Router, useValue: router }]
    })
    .compileComponents();

  }));

  beforeEach(() => {
    

    fixture = TestBed.createComponent(LogoutComponent);
    component = fixture.componentInstance;    
    //router = TestBed.get(Router);
    //authService = TestBed.get(AuthenticationService);//fixture.debugElement.injector.get(AuthenticationService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should log user out', () => {
    component.ngOnInit();
    //authService = TestBed.get(AuthenticationService);
    expect(authServiceSpy.logout).toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/login']);
    //console.log(authService);
    
  });
});
