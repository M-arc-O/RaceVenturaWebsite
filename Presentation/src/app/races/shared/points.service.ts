import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfigurationService, ServiceBase } from 'src/app/shared';
import { PointDetailViewModel } from './models';

@Injectable()
export class PointsService extends ServiceBase {
    readonly baseUrl: string;

    constructor(private http: HttpClient) {
        super();
        this.baseUrl = ConfigurationService.ApiRoot + '/api/points';
    }

    public addPoint(viewModel: PointDetailViewModel): Observable<PointDetailViewModel> {
        const body = JSON.stringify(viewModel);
        return this.http.post<PointDetailViewModel>(`${this.baseUrl}/addPoint`, body);
    }

    public editPoint(viewModel: PointDetailViewModel): Observable<PointDetailViewModel> {
        const body = JSON.stringify(viewModel);
        return this.http.put<PointDetailViewModel>(`${this.baseUrl}/editPoint`, body);
    }

    public deletePoint(viewModel: PointDetailViewModel): Observable<string> {
        return this.http.delete<string>(`${this.baseUrl}/${viewModel.pointId}/${viewModel.stageId}/remove`);
    }
}
