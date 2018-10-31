import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { UserService } from '../../../shared/user.service';
import { map } from 'rxjs/operators';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {
    loginError = false;
    loginForm: FormGroup;

    get loggedIn(): boolean {
        return this.userService.loggedIn;
    }

    constructor(private formBuilder: FormBuilder,
        private userService: UserService) {
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
        this.userService.loggedIn = false;
        localStorage.removeItem('auth_token');
    }

    validateAllFormFields(formGroup: FormGroup): void {
        Object.keys(formGroup.controls).forEach(key => {
            const control = formGroup.get(key);
            if (control instanceof FormControl) {
                control.markAsTouched({ onlySelf: true });
            } else if (control instanceof FormGroup) {
                this.validateAllFormFields(control);
            }
        });
    }
}
