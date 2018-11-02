import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { RaceViewModel } from './';
import { ConfigurationService, UserService } from 'src/app/shared';

@Injectable()
export class RaceService {
    readonly baseUrl: string;

    constructor(private http: Http,
        private userService: UserService) {
        this.baseUrl = ConfigurationService.ApiRoot + '/api/races';
    }

    public getRaces(): Observable<RaceViewModel[]> {
        const headers = new Headers();
        headers.append('Content-Type', 'application/json');
        headers.append('Authorization', `Bearer ${this.userService.authToken}`);

        const options = new RequestOptions({ headers: headers });

        return this.http.get(`${this.baseUrl}/getallraces`, options).pipe(
            map((res: Response) => {
                return <RaceViewModel[]>res.json();
            }),
            catchError(error => {
                return throwError(error);
            }));
    }
}
