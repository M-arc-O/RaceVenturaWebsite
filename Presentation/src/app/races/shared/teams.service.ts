import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfigurationService, ServiceBase } from 'src/app/shared';
import { TeamDetailViewModel } from './models';

@Injectable()
export class TeamsService extends ServiceBase {
    readonly baseUrl: string;

    constructor(private http: HttpClient) {
        super();
        this.baseUrl = ConfigurationService.ApiRoot + '/api/teams';
    }

    public addTeam(viewModel: TeamDetailViewModel): Observable<TeamDetailViewModel> {
        const body = JSON.stringify(viewModel);
        return this.http.post<TeamDetailViewModel>(`${this.baseUrl}/addteam`, body);
    }

    public editTeam(viewModel: TeamDetailViewModel): Observable<TeamDetailViewModel> {
        const body = JSON.stringify(viewModel);
        return this.http.put<TeamDetailViewModel>(`${this.baseUrl}/editteam`, body);
    }

    public deleteTeam(viewModel: TeamDetailViewModel): Observable<string> {
        return this.http.delete<string>(`${this.baseUrl}/${viewModel.teamId}/${viewModel.raceId}/remove`);
    }
}
