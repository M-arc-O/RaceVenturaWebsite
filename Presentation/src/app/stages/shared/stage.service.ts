import { Injectable } from '@angular/core';
import { Headers, Http, RequestOptions, Response, URLSearchParams } from '@angular/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { ConfigurationService, UserService } from 'src/app/shared';
import { StageViewModel } from './';
import { StageDetailViewModel } from './';

@Injectable()
export class StageService {
    readonly baseUrl: string;

    constructor(private http: Http,
        private userService: UserService) {
        this.baseUrl = ConfigurationService.ApiRoot + '/api/stages';
    }

    public getStages(raceId: string): Observable<StageViewModel[]> {
        const idHeader = { key: 'raceId', value: raceId.toString() };
        return this.http.get(`${this.baseUrl}/getracestages`, this.getHttpOptions(idHeader)).pipe(
            map((res: Response) => <StageViewModel[]>res.json()),
            catchError(error => throwError(error)));
    }

    public addStage(viewModel: StageDetailViewModel): Observable<StageViewModel> {
        const body = JSON.stringify(viewModel);

        return this.http.post(`${this.baseUrl}/addstage`, body, this.getHttpOptions()).pipe(
            map((res: Response) => <StageViewModel>res.json()),
            catchError(error => throwError(error)));
    }

    public deleteStage(id: string): Observable<string> {
        const body = JSON.stringify(id);

        return this.http.post(`${this.baseUrl}/deletestage`, body, this.getHttpOptions()).pipe(
            map((res: Response) => <string>res.json()),
            catchError(error => throwError(error)));
    }

    public getStageDetails(stageId: string): Observable<StageDetailViewModel> {
        const idHeader = { key: 'stageId', value: stageId.toString() };

        return this.http.get(`${this.baseUrl}/getstagedetails`, this.getHttpOptions(idHeader)).pipe(
            map((res: Response) => <StageDetailViewModel>res.json()),
            catchError(error => throwError(error)));
    }

    public editStage(viewModel: StageDetailViewModel): Observable<StageDetailViewModel> {
        const body = JSON.stringify(viewModel);

        return this.http.post(`${this.baseUrl}/editstage`, body, this.getHttpOptions()).pipe(
            map((res: Response) => <StageDetailViewModel>res.json()),
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
