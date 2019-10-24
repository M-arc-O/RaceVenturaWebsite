import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfigurationService, ServiceBase } from 'src/app/shared';
import { StageStoreModel } from './models';

@Injectable()
export class StagesService extends ServiceBase {
    readonly baseUrl: string;

    constructor(private http: HttpClient) {
        super();
        this.baseUrl = ConfigurationService.ApiRoot + '/api/stages';
    }

    public addStage(viewModel: StageStoreModel): Observable<StageStoreModel> {
        const body = JSON.stringify(viewModel);
        return this.http.post<StageStoreModel>(`${this.baseUrl}/addstage`, body);
    }

    public editStage(viewModel: StageStoreModel): Observable<StageStoreModel> {
        const body = JSON.stringify(viewModel);
        return this.http.put<StageStoreModel>(`${this.baseUrl}/editstage`, body);
    }

    public deleteStage(viewModel: StageStoreModel): Observable<string> {
        return this.http.delete<string>(`${this.baseUrl}/${viewModel.stageId}/${viewModel.raceId}/remove`);
    }
}
