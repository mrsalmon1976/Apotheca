import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RegisterComponent } from './register.component';
import { FormsModule, ReactiveFormsModule  } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'; 
import { Observable } from 'rxjs';
import { 
  MatCardModule,
  MatFormFieldModule,
  MatInputModule,
  MatProgressSpinnerModule,
  MatSnackBarModule
} from '@angular/material';
import { UserService } from '../_services/user.service';
import { IterableDiffers } from '@angular/core';

class MockUserService extends UserService {

  private registered: boolean = false;

  isRegistered() {
    return this.registered;
  }

  register(email: string, password: string, confirmPassword: string, firstName: string, lastName: string) {

    console.log("MockUserService.register called");
    this.registered = true;
    return Observable.create();//.of("").map(x => x);
  }
}

describe('RegisterComponent', () => {
  let component: RegisterComponent;
  let fixture: ComponentFixture<RegisterComponent>;
  let userService: UserService;
  let userServiceRegisterSpy;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        BrowserAnimationsModule ,
        FormsModule,
        HttpClientTestingModule,
        MatCardModule,
        MatFormFieldModule,
        MatInputModule,
        MatProgressSpinnerModule,
        MatSnackBarModule,
        ReactiveFormsModule,
        RouterTestingModule
      ],
      declarations: [ RegisterComponent ],
      providers: [
        { provide: UserService, useClass: MockUserService}
      ]
    })
    .compileComponents();


  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegisterComponent);
    component = fixture.componentInstance;
    userService = fixture.debugElement.injector.get(UserService);
    userServiceRegisterSpy = spyOn(userService, 'register').and.callThrough();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('register with valid data calls UserService.register', () => {
    updateForm('test@apotheca.com', 'password', 'password', 'John', 'Smith');
    component.register();
    expect(userServiceRegisterSpy).toHaveBeenCalledTimes(1);
  });

  it('register with no email does not call UserService.register', () => {
    updateForm('', 'password', 'password', 'John', 'Smith');
    component.register();
    expect(userServiceRegisterSpy).toHaveBeenCalledTimes(0);
  });

  it('register with no password does not call UserService.register', () => {
    updateForm('test@apotheca.com', '', '', 'John', 'Smith');
    component.register();
    expect(userServiceRegisterSpy).toHaveBeenCalledTimes(0);
  });

  it('register with password not matching confirmation does not call UserService.register', () => {
    updateForm('test@apotheca.com', 'password', 'password2', 'John', 'Smith');
    component.register();
    expect(userServiceRegisterSpy).toHaveBeenCalledTimes(0);
  });

  it('register with no first name does not call UserService.register', () => {
    updateForm('test@apotheca.com', 'password', 'password', '', 'Smith');
    component.register();
    expect(userServiceRegisterSpy).toHaveBeenCalledTimes(0);
  });

  it('register with no last name calls UserService.register', () => {
    updateForm('test@apotheca.com', 'password', 'password', 'John', '');
    component.register();
    expect(userServiceRegisterSpy).toHaveBeenCalledTimes(1);
  });

  function updateForm(email: string, password: string, confirmPassword: string, firstName: string, lastName: string) {
    component.registerForm.controls['email'].setValue(email);
    component.registerForm.controls['password'].setValue(password);
    component.registerForm.controls['confirmPassword'].setValue(confirmPassword);
    component.registerForm.controls['firstName'].setValue(firstName);
    component.registerForm.controls['lastName'].setValue(lastName);
  }


});

