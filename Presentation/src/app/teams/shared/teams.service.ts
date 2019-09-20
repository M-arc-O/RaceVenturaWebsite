import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfigurationService, ServiceBase } from 'src/app/shared';
import { TeamDetailViewModel, TeamRequest, TeamViewModel } from '.';

@Injectable()
export class TeamsService extends ServiceBase {
    readonly baseUrl: string;

    constructor(private http: HttpClient) {
        super();
        this.baseUrl = ConfigurationService.ApiRoot + '/api/teams';
    }

    public getTeams(raceId: string): Observable<TeamViewModel[]> {
        const idHeader = [{ key: 'raceId', value: raceId }];
        return this.http.get<TeamViewModel[]>(`${this.baseUrl}/getraceteams`, this.getHttpOptions(idHeader));
    }

    public getTeamDetails(request: TeamRequest): Observable<TeamDetailViewModel> {
        const idHeader = [
            { key: 'teamId', value: request.teamId },
            { key: 'raceId', value: request.raceId }
        ];
        return this.http.get<TeamDetailViewModel>(`${this.baseUrl}/getteamdetails`, this.getHttpOptions(idHeader));
    }

    public addTeam(viewModel: TeamDetailViewModel): Observable<TeamViewModel> {
        const body = JSON.stringify(viewModel);
        return this.http.post<TeamViewModel>(`${this.baseUrl}/addteam`, body);
    }

    public deleteTeam(viewModel: TeamViewModel): Observable<string> {
        return this.http.delete<string>(`${this.baseUrl}/${viewModel.teamId}/${viewModel.raceId}/remove`);
    }

    public editTeam(viewModel: TeamDetailViewModel): Observable<TeamDetailViewModel> {
        const body = JSON.stringify(viewModel);
        return this.http.put<TeamDetailViewModel>(`${this.baseUrl}/editteam`, body);
    }
}
