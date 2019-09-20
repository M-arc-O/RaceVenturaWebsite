import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
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

    constructor(private http: HttpClient) {
        this.baseUrl = ConfigurationService.ApiRoot;
    }

    register(email: string, password: string, firstName: string, lastName: string): Observable<boolean> {
        const body = JSON.stringify({ email, password, firstName, lastName });
        return this.http.post<boolean>(`${this.baseUrl}/api/accounts`, body);
    }

    login(email: string, password: string): void {
        this.logout();

        const body = JSON.stringify({ email, password });
        this.http.post<JwtViewModel>(`${this.baseUrl}/api/auth/login`, body).subscribe(res => {
            localStorage.setItem(this.tokenKey, res.auth_token);
        });
    }

    logout(): void {
        localStorage.removeItem(this.tokenKey);
    }
}
