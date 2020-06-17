import { OnDestroy } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { UserService } from './user.service';
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

    protected getDate(date: string, time: string): Date {
        const [day, month, year] = date.split('-');
        const [hours, minutes, seconds] = time.split(':');
        return new Date(+year, +month - 1, +day, +hours, +minutes, seconds === undefined ? 0 : +seconds);
    }

    public getDateString(input: Date): string {
        if (input !== undefined) {
            const date = new Date(input);
            const yy = date.getFullYear();
            const mm = date.getMonth() + 1;
            const mmStr = mm < 10 ? `0${mm}` : mm;
            const dd = date.getDate();
            const ddStr = dd < 10 ? `0${dd}` : dd;

            return `${ddStr}-${mmStr}-${yy}`;
        }

        return '';
    }

    public getTimeString(input: Date): string {
        if (input !== undefined) {
            const time = new Date(input.toString().replace(/\Z+$/g, '') + 'Z');
            const hours = time.getHours();
            const hoursStr = hours < 10 ? `0${hours}` : hours;
            const minutes = time.getMinutes();
            const minutesStr = minutes < 10 ? `0${minutes}` : minutes;
            const seconds = time.getSeconds();
            const secondsStr = seconds === 0 ? "": seconds < 10 ? `0${seconds}` : seconds;

            return `${hoursStr}:${minutesStr}${secondsStr === "" ? "": `:${secondsStr}`}`;
        }

        return '';
    }

    handleError(error: HttpErrorResponse) {
        if (error.status === 401) {
            this.userService.logout();
            this.router.navigateByUrl('home');
        }
    }
}
