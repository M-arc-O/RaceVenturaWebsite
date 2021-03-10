import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService, ComponentBase } from 'src/app/shared';
import { Router } from '@angular/router';
import { catchError, finalize, map, takeUntil } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { of } from 'rxjs';
import { CarouselService } from 'src/app/components/carousel/carousel.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html'
})
export class LoginComponent extends ComponentBase implements OnInit {
    public loginin = false;
    public serverError: any;
    public loginForm: FormGroup;
    public loggedIn = false;

    constructor(private formBuilder: FormBuilder,
        userService: UserService,
        private carouselService: CarouselService,
        router: Router) {
        super(userService, router);
        this.carouselService.showCarousel$.next(true);

        this.userService.loggedIn$.pipe(takeUntil(this.unsubscribe$)).subscribe(value => this.loggedIn = value);
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
                finalize(() => {
                    this.loginin = false;
                    this.loginForm.reset();
                })
            ).subscribe();
        } else {
            this.validateAllFormFields(this.loginForm);
        }
    }

    logoutClicked(): void {
        this.userService.logout();
    }
}
