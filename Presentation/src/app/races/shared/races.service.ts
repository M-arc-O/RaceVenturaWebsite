import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfigurationService, ServiceBase } from 'src/app/shared';
import { RaceViewModel, RaceStoreModel, RaceDetailViewModel } from './models';

@Injectable()
export class RacesService extends ServiceBase {
    readonly baseUrl: string;

    constructor(private http: HttpClient) {
        super();
        this.baseUrl = ConfigurationService.ApiRoot + '/api/races';
    }

    public getRaces(): Observable<RaceViewModel[]> {
        return this.http.get<RaceViewModel[]>(`${this.baseUrl}/getallraces`);
    }

    public getRaceDetails(raceId: string): Observable<RaceDetailViewModel> {
        const idHeader = [{ key: 'raceId', value: raceId }];
        return this.http.get<RaceDetailViewModel>(`${this.baseUrl}/getracedetails`, this.getHttpOptions(idHeader));
    }

    public addRace(viewModel: RaceStoreModel): Observable<RaceViewModel> {
        const body = JSON.stringify(viewModel);
        return this.http.post<RaceViewModel>(`${this.baseUrl}/addrace`, body);
    }

    public editRace(viewModel: RaceStoreModel): Observable<RaceStoreModel> {
        const body = JSON.stringify(viewModel);
        return this.http.put<RaceStoreModel>(`${this.baseUrl}/editrace`, body);
    }

    public deleteRace(raceId: string): Observable<string> {
        return this.http.delete<string>(`${this.baseUrl}/${raceId}/remove`);
    }
}
