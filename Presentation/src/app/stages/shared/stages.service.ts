import { Injectable } from '@angular/core';
import { Headers, Http, RequestOptions, Response, URLSearchParams } from '@angular/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { ConfigurationService, UserService } from 'src/app/shared';
import { StageDetailViewModel, StageRequest, StageViewModel } from '.';

@Injectable()
export class StagesService {
    readonly baseUrl: string;

    constructor(private http: Http,
        private userService: UserService) {
        this.baseUrl = ConfigurationService.ApiRoot + '/api/stages';
    }

    public getStages(raceId: string): Observable<StageViewModel[]> {
        const idHeader = [{ key: 'raceId', value: raceId.toString() }];
        return this.http.get(`${this.baseUrl}/getracestages`, this.getHttpOptions(idHeader)).pipe(
            map((res: Response) => <StageViewModel[]>res.json()),
            catchError(error => throwError(error)));
    }

    public getStageDetails(request: StageRequest): Observable<StageDetailViewModel> {
        const idHeader = [{ key: 'stageId', value: request.stageId.toString() },
            {key: 'raceId', value: request.raceId.toString()}];

        return this.http.get(`${this.baseUrl}/getstagedetails`, this.getHttpOptions(idHeader)).pipe(
            map((res: Response) => <StageDetailViewModel>res.json()),
            catchError(error => throwError(error)));
    }

    public addStage(viewModel: StageDetailViewModel): Observable<StageViewModel> {
        const body = JSON.stringify(viewModel);

        return this.http.post(`${this.baseUrl}/addstage`, body, this.getHttpOptions()).pipe(
            map((res: Response) => <StageViewModel>res.json()),
            catchError(error => throwError(error)));
    }

    public deleteStage(viewModel: StageViewModel): Observable<string> {

        return this.http.delete(`${this.baseUrl}/${viewModel.id}/${viewModel.raceId}/remove`, this.getHttpOptions()).pipe(
            map((res: Response) => <string>res.json()),
            catchError(error => throwError(error)));
    }

    public editStage(viewModel: StageDetailViewModel): Observable<StageDetailViewModel> {
        const body = JSON.stringify(viewModel);

        return this.http.put(`${this.baseUrl}/editstage`, body, this.getHttpOptions()).pipe(
            map((res: Response) => <StageDetailViewModel>res.json()),
            catchError(error => throwError(error)));
    }

    private getHttpOptions(additionalHeaders?: { key: string, value: string }[]): RequestOptions {
        const headers = new Headers();
        headers.set('Content-Type', 'application/json');
        headers.set('Authorization', `Bearer ${this.userService.authToken}`);

        if (additionalHeaders !== undefined) {
            const params = new URLSearchParams();
            for (const header of additionalHeaders) {
                params.set(header.key, header.value);
            }
            return new RequestOptions({ headers: headers, params: params });
        }

        return new RequestOptions({ headers: headers });
    }
}
