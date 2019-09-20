import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfigurationService, ServiceBase } from 'src/app/shared';
import { PointDetailViewModel, PointRequest, PointViewModel } from '.';

@Injectable()
export class PointsService extends ServiceBase {
    readonly baseUrl: string;

    constructor(private http: HttpClient) {
        super();
        this.baseUrl = ConfigurationService.ApiRoot + '/api/points';
    }

    public getPoints(stageId: string): Observable<PointViewModel[]> {
        const idHeader = [{ key: 'stageId', value: stageId }];
        return this.http.get<PointViewModel[]>(`${this.baseUrl}/getstagepoints`, this.getHttpOptions(idHeader));
    }

    public getPointDetails(request: PointRequest): Observable<PointDetailViewModel> {
        const idHeader = [
            { key: 'stageId', value: request.stageId },
            { key: 'pointId', value: request.pointId }
        ];
        return this.http.get<PointDetailViewModel>(`${this.baseUrl}/getpointdetails`, this.getHttpOptions(idHeader));
    }

    public addPoint(viewModel: PointDetailViewModel): Observable<PointViewModel> {
        const body = JSON.stringify(viewModel);
        return this.http.post<PointViewModel>(`${this.baseUrl}/addPoint`, body);
    }

    public deletePoint(viewModel: PointViewModel): Observable<string> {
        return this.http.delete<string>(`${this.baseUrl}/${viewModel.pointId}/${viewModel.stageId}/remove`);
    }

    public editPoint(viewModel: PointDetailViewModel): Observable<PointDetailViewModel> {
        const body = JSON.stringify(viewModel);
        return this.http.put<PointDetailViewModel>(`${this.baseUrl}/editPoint`, body);
    }
}
