import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountRoutingModule } from './account-routing.module';
import { LoginComponent } from './components/login/login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ValidationModule } from '../shared/components/validation/validation.module';
import { ConfirmEmailComponent } from './components/confirmEmail/confirm-email.component';
import { ForgotPasswordComponent } from './components/forgotPassword/forgot-password.component';
import { ResetPasswordComponent } from './components/resetPassword/reset-password.component';

@NgModule({
  declarations: [
    LoginComponent,
    ConfirmEmailComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    AccountRoutingModule,
    ValidationModule
  ],
  providers: []
})
export class AccountModule { }
