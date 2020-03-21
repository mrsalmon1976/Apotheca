import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, FormControl, Validators  } from '@angular/forms';
import { ValidationUtils } from '../_helpers/validation-utils';
import { UserService } from '../_services/user.service';
import { first } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material';
import { ErrorUtils } from '../_helpers/error-utils';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.less']
})

export class RegisterComponent implements OnInit {

  registerForm: FormGroup;
  loading = false;
  error = '';

  constructor(
    private formBuilder: FormBuilder,
    private validationUtils: ValidationUtils,
    private userService: UserService,
    private router: Router,
    private snackBar: MatSnackBar
    ) { }

  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      email: ['', [ Validators.required, this.validationUtils.validateEmail ]],
      password: ['', [ Validators.required, Validators.minLength(8) ]],
      confirmPassword: ['', [ Validators.required, this.validateConfirmPassword ]],
      firstName: ['', Validators.required],
      lastName: [''],
    });
  }

  private validateConfirmPassword = (c: FormControl) => {
    if (this.registerForm == undefined) return null;
    return (this.registerForm.controls.password.value === c.value) ? null : {
      validateConfirmPassword: {
        valid: false
      }
    };
  }

    // convenience getter for easy access to form fields
    get f() { return this.registerForm.controls; }

    register() {

      // stop here if form is invalid
      if (this.registerForm.invalid) {
          for (var i in this.registerForm.controls) {
            // console.log(this.registerForm.controls[i].value + '..' + this.registerForm.controls[i].status);
            this.registerForm.controls[i].markAsTouched();
          }
          return;
      }

      this.loading = true;

      this.userService.register(this.f.email.value, this.f.password.value, this.f.confirmPassword.value, this.f.firstName.value, this.f.lastName.value)
          .pipe(first())
          .subscribe(
              data => {
                  this.snackBar.open(`Thanks for signing up, ${this.f.firstName.value}!  You should receive an email to complete the registration process.`, null, { duration: 10000 });
                  this.router.navigate(['/login']);
              },
              error => {
                this.error = ErrorUtils.getMessage(error);
                this.loading = false;
          }
          );
  }

}
