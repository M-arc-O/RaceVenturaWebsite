import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { of } from "rxjs";
import { catchError, finalize, map, takeUntil } from "rxjs/operators";
import { CarouselService } from "src/app/components/carousel/carousel.service";
import { ComponentBase, UserService } from "src/app/shared";

@Component({
    selector: 'app-confirm-email',
    templateUrl: './confirm-email.component.html'
})
export class ConfirmEmailComponent extends ComponentBase implements OnInit {   
    private code: string;
    private emailAddress: string;

    public emailConfirmed = false;
    public sending = false;
    public serverError: any;

    constructor(
        private route: ActivatedRoute,
        userService: UserService,
        private carouselService: CarouselService,
        router: Router) {
        super(userService, router);
        this.carouselService.showCarousel$.next(true);
    }

    ngOnInit(): void {
        this.route.params.pipe(takeUntil(this.unsubscribe$)).subscribe(params => {
            this.code = params['code'];
            this.emailAddress = params['emailAddress'];
        });

        this.sending = true;
        this.userService.confirmEmail(this.code, this.emailAddress).pipe(
            map(() => { this.emailConfirmed = true}),
            catchError((error: HttpErrorResponse) => of(this.serverError = error.error)),
            finalize(() => this.sending = false)
        ).subscribe();
    }
}
