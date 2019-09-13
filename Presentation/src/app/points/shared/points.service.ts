import { Injectable } from '@angular/core';
import { Headers, Http, RequestOptions, Response, URLSearchParams } from '@angular/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { ConfigurationService, UserService } from 'src/app/shared';
import { PointDetailViewModel, PointRequest, PointViewModel } from '.';

@Injectable()
export class PointsService {
    readonly baseUrl: string;

    constructor(private http: Http,
        private userService: UserService) {
        this.baseUrl = ConfigurationService.ApiRoot + '/api/points';
    }

    public getPoints(stageId: string): Observable<PointViewModel[]> {
        const idHeader = [{ key: 'stageId', value: stageId.toString() }];
        return this.http.get(`${this.baseUrl}/getstagepoints`, this.getHttpOptions(idHeader)).pipe(
            map((res: Response) => <PointViewModel[]>res.json()),
            catchError(error => throwError(error)));
    }

    public getPointDetails(request: PointRequest): Observable<PointDetailViewModel> {
        const idHeader = [{ key: 'stageId', value: request.stageId.toString() },
        { key: 'pointId', value: request.pointId.toString() }];

        return this.http.get(`${this.baseUrl}/getpointdetails`, this.getHttpOptions(idHeader)).pipe(
            map((res: Response) => <PointDetailViewModel>res.json()),
            catchError(error => throwError(error)));
    }

    public addPoint(viewModel: PointDetailViewModel): Observable<PointViewModel> {
        const body = JSON.stringify(viewModel);

        return this.http.post(`${this.baseUrl}/addPoint`, body, this.getHttpOptions()).pipe(
            map((res: Response) => <PointViewModel>res.json()),
            catchError(error => throwError(error)));
    }

    public deletePoint(viewModel: PointViewModel): Observable<string> {

        return this.http.delete(`${this.baseUrl}/${viewModel.id}/${viewModel.stageId}/remove`, this.getHttpOptions()).pipe(
            map((res: Response) => <string>res.json()),
            catchError(error => throwError(error)));
    }

    public editPoint(viewModel: PointDetailViewModel): Observable<PointDetailViewModel> {
        const body = JSON.stringify(viewModel);

        return this.http.put(`${this.baseUrl}/editPoint`, body, this.getHttpOptions()).pipe(
            map((res: Response) => <PointDetailViewModel>res.json()),
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
