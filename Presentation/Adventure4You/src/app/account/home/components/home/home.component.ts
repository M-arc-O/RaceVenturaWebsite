import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService, ComponentBase } from 'src/app/shared';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html'
})
export class HomeComponent extends ComponentBase implements OnInit {
    loginError = false;
    loginForm: FormGroup;

    get loggedIn(): boolean {
        return this.userService.userId !== undefined;
    }

    constructor(private formBuilder: FormBuilder,
        private userService: UserService) {
        super();
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
            this.userService.login(this.loginForm.get('email').value, this.loginForm.get('password').value);
        } else {
            this.validateAllFormFields(this.loginForm);
        }
    }

    logoutClicked(): void {
        this.userService.logout();
    }
}
