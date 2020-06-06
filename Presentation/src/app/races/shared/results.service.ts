import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfigurationService, ServiceBase } from 'src/app/shared';
import { TeamResultViewModel } from './models';

@Injectable()
export class ResultsService extends ServiceBase {
    readonly baseUrl: string;

    constructor(private http: HttpClient) {
        super();
        this.baseUrl = ConfigurationService.ApiRoot + '/api/races';
    }

    public getRaceResult(raceId: string): Observable<TeamResultViewModel[]> {
        const idHeader = [{ key: 'raceId', value: raceId }];
        return this.http.get<TeamResultViewModel[]>(`${ConfigurationService.ApiRoot}/api/results/getraceresults`, this.getHttpOptions(idHeader));
    }
}
