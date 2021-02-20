import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ConfirmEmailComponent } from './components/confirmEmail/confirm-email.component';
import { ForgotPasswordComponent } from './components/forgotPassword/forgot-password.component';
import { LoginComponent } from './components/login/login.component';
import { ResetPasswordComponent } from './components/resetPassword/reset-password.component';

const accountRoutes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'confirmemail/:code/:emailAddress', component: ConfirmEmailComponent, },
    { path: 'forgotpassword', component: ForgotPasswordComponent },
    { path: 'resetpassword/:code/:emailAddress', component: ResetPasswordComponent }
];

@NgModule({
    imports: [
        RouterModule.forChild(accountRoutes)
    ],
    exports: [
        RouterModule
    ]
})

export class AccountRoutingModule { }
