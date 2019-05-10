import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators  } from '@angular/forms';
import { first } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material';
import { AuthenticationService } from '../_services/authentication.service';

@Component({ templateUrl: 'login.component.html', styleUrls: ['./login.component.less'] })
export class LoginComponent implements OnInit {
    loginForm: FormGroup;
    loading = false;
    returnUrl: string;
    error = '';

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        private snackBar: MatSnackBar
    ) { }

    ngOnInit() {

        this.loginForm = this.formBuilder.group({
            email: ['', Validators.required],
            password: ['', Validators.required]
        });

        // get return url from route parameters or default to '/dashboard'
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/dashboard';

        if (this.authenticationService.isLoggedIn()) {
          this.router.navigate([this.returnUrl]);
        }
    }

    // convenience getter for easy access to form fields
    get f() { return this.loginForm.controls; }

    login() {

        // stop here if form is invalid
        if (this.loginForm.invalid) {
            for (var i in this.loginForm.controls) {
                this.loginForm.controls[i].markAsTouched();
            }
            return;
        }

        this.loading = true;
        this.authenticationService.login(this.f.email.value, this.f.password.value)
            .pipe(first())
            .subscribe(
                data => {
                    this.snackBar.open(`Welcome, ${this.f.email.value}!`, null, { duration: 2000 });
                    this.router.navigate([this.returnUrl]);
                },
                error => {
                    if (error.error) {
                        this.error = error.error;
                    }
                    else if (error.status) {
                      this.error = `An error occurred on the server: ${error.status} ${error.statusText}`;
                    }
                    else {
                      this.error = error;
                    }
                    this.loading = false;
                }
            );
    }
}