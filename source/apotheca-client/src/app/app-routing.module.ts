import { NgModule }             from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent }   from './dashboard/dashboard.component';
import { StoreComponent }   from './store/store.component';
import { LoginComponent }  from './login/login.component';
import { LogoutComponent }  from './logout/logout.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { RegisterComponent }    from './register/register.component';

import { PageNotFoundComponent }  from './_errors/pagenotfound/pagenotfound.component';

import { AuthGuard } from './_guards/auth.guard';
import { from } from 'rxjs';

const routes: Routes = [
  // public routes
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'logout', component: LogoutComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  // authenticated routes
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: "store/:name/:id", component: StoreComponent, canActivate: [AuthGuard] },
  // error routes
  { path: '**', component: PageNotFoundComponent },
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}