import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { of } from "rxjs";
import { catchError, finalize, map, takeUntil } from "rxjs/operators";
import { CarouselService } from "src/app/components/carousel/carousel.service";
import { ComponentBase, MustMatch, UserService } from "src/app/shared";

@Component({
    selector: 'app-reset-password',
    templateUrl: './reset-password.component.html'
})
export class ResetPasswordComponent extends ComponentBase implements OnInit {
    private code: string;
    private emailAddress: string;

    public resetPasswordForm: FormGroup;
    public passwordReseted = false;
    public sending = false;
    public serverError: any;

    constructor(
        userService: UserService,
        private route: ActivatedRoute,
        carouselService: CarouselService,
        router: Router) {
        super(userService, carouselService, router);
        this.carouselService.showCarousel = true;
    }

    ngOnInit(): void {
        this.route.params.pipe(takeUntil(this.unsubscribe$)).subscribe(params => {
            this.code = params['code'];
            this.emailAddress = params['emailAddress'];
        });

        this.setupForm();
    }

    public submitClick(): void {
        if (this.resetPasswordForm.valid) {
            let password = this.resetPasswordForm.get("password").value;
            this.sending = true;
            this.userService.resetPassword(password, this.emailAddress, this.code).pipe(
                map(() => this.passwordReseted = true),
                catchError((error: HttpErrorResponse) => of(this.serverError = error.error)),
                finalize(() => this.sending = false)
            ).subscribe();
        } else {
            this.validateAllFormFields(this.resetPasswordForm);
        }
    }

    private setupForm(): void {
        const formBuilder = new FormBuilder();

        this.resetPasswordForm = formBuilder.group({
            password: ["", [Validators.required, Validators.minLength(8), Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[$@$!%?&])[A-Za-z\d$@$!%?&]{8,}$/)]],
            confirmPassword: ["", [Validators.required]]
        }, {
            validator: MustMatch('password', 'confirmPassword')
        });
    }
}
