import { Injectable } from '@angular/core';
import { Headers, Http, RequestOptions, Response, URLSearchParams } from '@angular/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { ConfigurationService, UserService } from 'src/app/shared';
import { RaceViewModel } from '.';
import { RaceDetailViewModel } from './models/race-detail-view-model';

@Injectable()
export class RacesService {
    readonly baseUrl: string;

    constructor(private http: Http,
        private userService: UserService) {
        this.baseUrl = ConfigurationService.ApiRoot + '/api/races';
    }

    public getRaces(): Observable<RaceViewModel[]> {
        return this.http.get(`${this.baseUrl}/getallraces`, this.getHttpOptions()).pipe(
            map((res: Response) => <RaceViewModel[]>res.json()),
            catchError(error => throwError(error)));
    }

    public addRace(viewModel: RaceDetailViewModel): Observable<RaceViewModel> {
        const body = JSON.stringify(viewModel);

        return this.http.post(`${this.baseUrl}/addrace`, body, this.getHttpOptions()).pipe(
            map((res: Response) => <RaceViewModel>res.json()),
            catchError(error => throwError(error)));
    }

    public deleteRace(id: string): Observable<string> {
        return this.http.delete(`${this.baseUrl}/${id}/remove`, this.getHttpOptions()).pipe(
            map((res: Response) => <string>res.json()),
            catchError(error => throwError(error)));
    }

    public getRaceDetails(raceId: string): Observable<RaceDetailViewModel> {
        const idHeader = { key: 'raceId', value: raceId.toString() };

        return this.http.get(`${this.baseUrl}/getracedetails`, this.getHttpOptions(idHeader)).pipe(
            map((res: Response) => <RaceDetailViewModel>res.json()),
            catchError(error => throwError(error)));
    }

    public editRace(viewModel: RaceDetailViewModel): Observable<RaceDetailViewModel> {
        const body = JSON.stringify(viewModel);

        return this.http.put(`${this.baseUrl}/editrace`, body, this.getHttpOptions()).pipe(
            map((res: Response) => <RaceDetailViewModel>res.json()),
            catchError(error => throwError(error)));
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
