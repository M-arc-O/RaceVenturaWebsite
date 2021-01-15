import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';

const accountRoutes: Routes = [
    { path: 'login', component: LoginComponent },
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
