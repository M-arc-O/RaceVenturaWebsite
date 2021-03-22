import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfigurationService, ServiceBase } from 'src/app/shared';
import { RaceViewModel, RaceStoreModel, RaceDetailViewModel, RaceAccessViewModel } from './models';

@Injectable()
export class RacesAccessService extends ServiceBase {
    readonly baseUrl: string;

    constructor(private http: HttpClient) {
        super();
        this.baseUrl = ConfigurationService.ApiRoot + '/api/raceaccess';
    }

    public getRaceAccess(raceId: string): Observable<RaceAccessViewModel[]> {
        const idHeader = [{ key: 'raceId', value: raceId }];
        return this.http.get<RaceAccessViewModel[]>(`${this.baseUrl}/getaccesses`, this.getHttpOptions(idHeader));
    }

    public addRaceAccess(viewModel: RaceAccessViewModel): Observable<RaceAccessViewModel> {
        const body = JSON.stringify(viewModel);
        return this.http.post<RaceAccessViewModel>(`${this.baseUrl}/addaccess`, body);
    }

    public editRaceAccess(viewModel: RaceAccessViewModel): Observable<RaceAccessViewModel> {
        const body = JSON.stringify(viewModel);
        return this.http.put<RaceAccessViewModel>(`${this.baseUrl}/editaccess`, body);
    }

    public deleteRaceAccess(raceId: string, email: string): Observable<RaceAccessViewModel> {
        return this.http.delete<RaceAccessViewModel>(`${this.baseUrl}/${raceId}/${email}/remove`);
    }
}
