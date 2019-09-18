import { Injectable } from '@angular/core';
import { Headers, Http, RequestOptions, Response, URLSearchParams } from '@angular/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { ConfigurationService, UserService } from 'src/app/shared';
import { TeamDetailViewModel, TeamRequest, TeamViewModel } from '.';

@Injectable()
export class TeamsService {
    readonly baseUrl: string;

    constructor(private http: Http,
        private userService: UserService) {
        this.baseUrl = ConfigurationService.ApiRoot + '/api/teams';
    }

    public getTeams(raceId: string): Observable<TeamViewModel[]> {
        const idHeader = [{ key: 'raceId', value: raceId.toString() }];
        return this.http.get(`${this.baseUrl}/getraceteams`, this.getHttpOptions(idHeader)).pipe(
            map((res: Response) => <TeamViewModel[]>res.json()),
            catchError(error => throwError(error)));
    }

    public getTeamDetails(request: TeamRequest): Observable<TeamDetailViewModel> {
        const idHeader = [{ key: 'teamId', value: request.teamId.toString() },
        { key: 'raceId', value: request.raceId.toString() }];

        return this.http.get(`${this.baseUrl}/getteamdetails`, this.getHttpOptions(idHeader)).pipe(
            map((res: Response) => <TeamDetailViewModel>res.json()),
            catchError(error => throwError(error)));
    }

    public addTeam(viewModel: TeamDetailViewModel): Observable<TeamViewModel> {
        const body = JSON.stringify(viewModel);

        return this.http.post(`${this.baseUrl}/addteam`, body, this.getHttpOptions()).pipe(
            map((res: Response) => <TeamViewModel>res.json()),
            catchError(error => throwError(error)));
    }

    public deleteTeam(viewModel: TeamViewModel): Observable<string> {

        return this.http.delete(`${this.baseUrl}/${viewModel.id}/${viewModel.raceId}/remove`, this.getHttpOptions()).pipe(
            map((res: Response) => <string>res.json()),
            catchError(error => throwError(error)));
    }

    public editTeam(viewModel: TeamDetailViewModel): Observable<TeamDetailViewModel> {
        const body = JSON.stringify(viewModel);

        return this.http.put(`${this.baseUrl}/editteam`, body, this.getHttpOptions()).pipe(
            map((res: Response) => <TeamDetailViewModel>res.json()),
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
