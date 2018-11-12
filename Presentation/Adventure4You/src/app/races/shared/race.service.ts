import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, URLSearchParams } from '@angular/http';
import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { RaceViewModel } from './';
import { ConfigurationService, UserService } from 'src/app/shared';
import { AddRaceViewModel } from './models/add-race-view-model';
import { RaceDetailViewModel } from './models/race-detail-view-model';
import { stringify } from '@angular/core/src/util';

@Injectable()
export class RaceService {
    readonly baseUrl: string;

    constructor(private http: Http,
        private userService: UserService) {
        this.baseUrl = ConfigurationService.ApiRoot + '/api/races';
    }

    public getRaces(): Observable<RaceViewModel[]> {
        return this.http.get(`${this.baseUrl}/getallraces`, this.getHttpOptions()).pipe(
            map((res: Response) => {
                return <RaceViewModel[]>res.json();
            }),
            catchError(error => {
                return throwError(error);
            }));
    }

    public addRace(viewModel: AddRaceViewModel): Observable<RaceViewModel> {
        const body = JSON.stringify(viewModel);

        return this.http.post(`${this.baseUrl}/addrace`, body, this.getHttpOptions()).pipe(
            map((res: Response) => {
                return <RaceViewModel>res.json();
            }),
            catchError(error => {
                return throwError(error);
            }));
    }

    public getRaceDetails(raceId: number): Observable<RaceDetailViewModel> {
        const idHeader = { key: 'raceId', value: raceId.toString() };

        return this.http.get(`${this.baseUrl}/getracedetails`, this.getHttpOptions(idHeader)).pipe(
            map((res: Response) => {
                return <RaceDetailViewModel>res.json();
            }),
            catchError(error => {
                return throwError(error);
            }));
    }

    private getHttpOptions(additionalHeader?: { key: string, value: string }): RequestOptions {
        const headers = new Headers();
        headers.set('Content-Type', 'application/json');
        headers.set('Authorization', `Bearer ${this.userService.authToken}`);

        if (additionalHeader !== undefined) {
            const params = new URLSearchParams();
            params.set(additionalHeader.key, additionalHeader.value);
            return new RequestOptions({ headers: headers, params: params });
        }

        return new RequestOptions({ headers: headers });
    }
}
