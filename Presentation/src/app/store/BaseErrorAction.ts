import { HttpErrorResponse } from '@angular/common/http';

export interface BaseErrorAction {
    readonly type: string;
    readonly payload: {
        error: HttpErrorResponse;
    };
}
