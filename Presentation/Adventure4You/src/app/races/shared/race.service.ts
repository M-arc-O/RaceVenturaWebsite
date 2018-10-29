import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { RaceViewModel } from './';
import { ConfigurationService } from 'src/app/shared';

@Injectable()
export class RaceService {
    readonly baseUrl: string;

    constructor(private http: Http) {
        this.baseUrl = ConfigurationService.ApiRoot + '/api/races';
    }

    public getRaces(): Observable<RaceViewModel[]> {
        return this.http.get(`${this.baseUrl}/getallraces`).pipe(
            map((res: Response) => {
                return <RaceViewModel[]>res.json();
            }),
            catchError(error => {
                return throwError(error);
            }));
    }
}
