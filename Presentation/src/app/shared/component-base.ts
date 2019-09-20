import { OnDestroy } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { UserService } from '.';
import { HttpErrorResponse } from '@angular/common/http';

export abstract class ComponentBase implements OnDestroy {
    protected unsubscribe$: Subject<void>;

    constructor(protected userService: UserService,
        protected router: Router) {
        this.unsubscribe$ = new Subject<void>();
    }

    ngOnDestroy(): void {
        this.unsubscribe$.next();
        this.unsubscribe$.complete();
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

    handleError(error: HttpErrorResponse) {
        if (error.status === 401) {
            this.userService.logout();
            this.router.navigateByUrl('home');
        }
        console.log(error);
    }
}
