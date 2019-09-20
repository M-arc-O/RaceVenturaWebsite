import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfigurationService, ServiceBase } from 'src/app/shared';
import { StageDetailViewModel, StageRequest, StageViewModel } from '.';

@Injectable()
export class StagesService extends ServiceBase {
    readonly baseUrl: string;

    constructor(private http: HttpClient) {
        super();
        this.baseUrl = ConfigurationService.ApiRoot + '/api/stages';
    }

    public getStages(raceId: string): Observable<StageViewModel[]> {
        const idHeader = [{ key: 'raceId', value: raceId }];
        return this.http.get<StageViewModel[]>(`${this.baseUrl}/getracestages`, this.getHttpOptions(idHeader));
    }

    public getStageDetails(request: StageRequest): Observable<StageDetailViewModel> {
        const idHeader = [
            { key: 'stageId', value: request.stageId },
            { key: 'raceId', value: request.raceId }
        ];
        return this.http.get<StageDetailViewModel>(`${this.baseUrl}/getstagedetails`, this.getHttpOptions(idHeader));
    }

    public addStage(viewModel: StageDetailViewModel): Observable<StageViewModel> {
        const body = JSON.stringify(viewModel);
        return this.http.post<StageViewModel>(`${this.baseUrl}/addstage`, body);
    }

    public deleteStage(viewModel: StageViewModel): Observable<string> {
        return this.http.delete<string>(`${this.baseUrl}/${viewModel.stageId}/${viewModel.raceId}/remove`);
    }

    public editStage(viewModel: StageDetailViewModel): Observable<StageDetailViewModel> {
        const body = JSON.stringify(viewModel);
        return this.http.put<StageDetailViewModel>(`${this.baseUrl}/editstage`, body);
    }
}
