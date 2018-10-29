import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { ConfigurationService, ServiceBase } from 'src/app/Shared';
import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { RaceViewModel } from './Models/race-view-model';

@Injectable()
export class RaceService extends ServiceBase {
    readonly baseUrl: string;

    constructor(private http: Http) {
        super();
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
