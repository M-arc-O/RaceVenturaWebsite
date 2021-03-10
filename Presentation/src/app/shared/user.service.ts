import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { ConfirmEmailViewModel, ForgotPasswordViewModel, ResetPasswordViewModel } from '../account/shared/models';
import { ConfigurationService } from './configuration-service';
import { JwtViewModel } from './models/jwt-view-model';

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

    public loggedIn$ = new BehaviorSubject<boolean>(false);

    constructor(private http: HttpClient) {
        this.baseUrl = ConfigurationService.ApiRoot;
        
        if (this.authToken !== undefined && this.authToken !== '') {
            this.loggedIn$.next(true);
        }
    }

    public confirmEmail(code: string, emailAddress: string): Observable<any> {        
        let viewModel = new ConfirmEmailViewModel();
        viewModel.emailAddress = emailAddress;
        viewModel.code = code;

        const body = JSON.stringify(viewModel);        
        return this.http.post<any>(`${this.baseUrl}/api/accounts/confirmemail`, body);
    }

    public forgotPassword(email: string): Observable<any> {
        let viewModel = new ForgotPasswordViewModel();
        viewModel.emailAddress = email;

        const body = JSON.stringify(viewModel);        
        return this.http.post(`${this.baseUrl}/api/accounts/forgotpassword`, body);
    }

    public resetPassword(password: string, emailAddress: string, code: string): Observable<any> {
        let viewModel = new ResetPasswordViewModel();
        viewModel.password = password;
        viewModel.emailAddress = emailAddress;
        viewModel.code = code;

        const body = JSON.stringify(viewModel);        
        return this.http.post<any>(`${this.baseUrl}/api/accounts/resetpassword`, body);
    }

    public login(email: string, password: string): Observable<JwtViewModel> {
        const body = JSON.stringify({ email, password });
        return this.http.post<JwtViewModel>(`${this.baseUrl}/api/auth/login`, body);
    }

    public setJwtToken(token: string) {
        localStorage.setItem(this.tokenKey, token);
        this.loggedIn$.next(true);
    }

    public logout(): void {
        localStorage.removeItem(this.tokenKey);
        this.loggedIn$.next(false);
    }
}
