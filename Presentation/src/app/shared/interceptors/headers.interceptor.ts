import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserService } from '..';

@Injectable()
export class HeadersInterceptor implements HttpInterceptor {
    constructor(public userService: UserService) { }
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        request = request.clone({
            setHeaders: {
                'Authorization': `Bearer ${this.userService.authToken}`,
                'Content-Type': 'application/json'
            }
        });
        return next.handle(request);
    }
}
