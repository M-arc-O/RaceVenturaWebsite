import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfigurationService, ServiceBase } from 'src/app/shared';
import { TeamPointVisitedViewModel } from './models';

@Injectable()
export class VisitedPointsService extends ServiceBase {
    readonly baseUrl: string;

    constructor(private http: HttpClient) {
        super();
        this.baseUrl = ConfigurationService.ApiRoot + '/api/visitedpoints';
    }

    public addTeamPointVisited(viewModel: TeamPointVisitedViewModel): Observable<TeamPointVisitedViewModel> {
        const body = JSON.stringify(viewModel);
        return this.http.post<TeamPointVisitedViewModel>(`${this.baseUrl}/addvisitedpoint`, body);
    }

    public deleteTeamPointVisited(viewModel: TeamPointVisitedViewModel): Observable<string> {
        return this.http.delete<string>(`${this.baseUrl}/${viewModel.visitedPointId}/deletevisitedpoint`);
    }
}