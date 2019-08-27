import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ConfigurationService } from './configuration-service';

@Injectable()
export class UserService {
    readonly baseUrl: string;
    readonly tokenKey: 'token';

    public get authToken(): string {
        const token = localStorage.getItem(this.tokenKey);
        if (token === null) {
            return undefined;
        }
        return token;
    }

    constructor(private http: Http) {
        this.baseUrl = ConfigurationService.ApiRoot;
    }

    register(email: string, password: string, firstName: string, lastName: string): Observable<boolean> {
        const body = JSON.stringify({ email, password, firstName, lastName });
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });

        return this.http.post(`${this.baseUrl}/api/accounts`, body, options).pipe(
            map(res => {
                return <boolean>res.json();
            }),
            catchError(error => {
                return throwError(error);
            }));
    }

    login(email: string, password: string): void {
        this.logout();

        const body = JSON.stringify({ email, password });
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });

        this.http.post(`${this.baseUrl}/api/auth/login`, body, options).subscribe(res => {
            const json = res.json();
            localStorage.setItem(this.tokenKey, json.auth_token);
        });
    }

    logout(): void {
        localStorage.removeItem(this.tokenKey);
    }
}
