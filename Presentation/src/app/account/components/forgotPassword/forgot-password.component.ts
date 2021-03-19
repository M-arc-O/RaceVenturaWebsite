import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { of } from "rxjs";
import { catchError, finalize, map } from "rxjs/operators";
import { CarouselService } from "src/app/components/carousel/carousel.service";
import { ComponentBase, UserService } from "src/app/shared";

@Component({
    selector: 'app-forgot-password',
    templateUrl: './forgot-password.component.html'
})
export class ForgotPasswordComponent extends ComponentBase implements OnInit {    
    public forgotPasswordForm: FormGroup;
    public requestSend = false;
    public sending = false;
    public serverError: any;

    constructor(
        userService: UserService,
        private carouselService: CarouselService,
        router: Router) {
        super(userService, router);
        this.carouselService.showCarousel$.next(true);
    }

    ngOnInit(): void {
        this.setupForm();
    }

    public submitClick(): void {
        if (this.forgotPasswordForm.valid) {
            this.sending = true;
            this.userService.forgotPassword(this.forgotPasswordForm.get("emailAddress").value).pipe(
                map(() => { this.requestSend = true}),
                catchError((error: HttpErrorResponse) => of(this.serverError = error.error)),
                finalize(() => this.sending = false)
            ).subscribe();
        } else {
            this.validateAllFormFields(this.forgotPasswordForm);
        }
    }

    private setupForm(): void {
        const formBuilder = new FormBuilder();

        this.forgotPasswordForm = formBuilder.group({
            emailAddress: ["", [Validators.required, Validators.email]]
        });
    }
}
