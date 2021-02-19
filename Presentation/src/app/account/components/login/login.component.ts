import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService, ComponentBase } from 'src/app/shared';
import { Router } from '@angular/router';
import { catchError, finalize, map } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { of } from 'rxjs';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html'
})
export class LoginComponent extends ComponentBase implements OnInit {
    public loginin = false;
    public serverError: any;
    public loginForm: FormGroup;

    get loggedIn(): boolean {
        return this.userService.authToken !== undefined;
    }

    constructor(private formBuilder: FormBuilder,
        userService: UserService,
        router: Router) {
        super(userService, router);
    }

    ngOnInit(): void {
        this.setupForm();
    }

    setupForm(): void {
        this.loginForm = this.formBuilder.group({
            email: ['', [Validators.required]],
            password: ['', [Validators.required]]
        });
    }

    loginButtonClick(): void {
        if (this.loginForm.valid) {
            this.loginin = true;
            this.userService.login(this.loginForm.get('email').value, this.loginForm.get('password').value).pipe(
                map(res => this.userService.setJwtToken(res.auth_token)),
                catchError((error: HttpErrorResponse) => of(this.serverError = error.error)),
                finalize(() => this.loginin = false)
            ).subscribe();
        } else {
            this.validateAllFormFields(this.loginForm);
        }
    }

    logoutClicked(): void {
        this.userService.logout();
    }
}
