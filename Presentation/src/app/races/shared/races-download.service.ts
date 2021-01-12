import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfigurationService, ServiceBase } from 'src/app/shared';

@Injectable()
export class RacesDownloadService extends ServiceBase {
    readonly baseUrl: string;

    constructor(private http: HttpClient) {
        super();
        this.baseUrl = ConfigurationService.ApiRoot + '/api/races';
    }

    public downloadPointPdf(raceId: string): Observable<Blob> {
        return this.http.get(`${this.baseUrl}/getpointspdf`, { params: { raceId: raceId } , responseType: 'blob' });
    }

    public downloadTeamsPdf(raceId: string): Observable<Blob> {
        return this.http.get(`${this.baseUrl}/getteamspdf`, { params: { raceId: raceId } , responseType: 'blob' });
    }

    public downloadStagesAndRaceEndPdf(raceId: string): Observable<Blob> {
        return this.http.get(`${this.baseUrl}/getstagesandraceendpdf`, { params: { raceId: raceId } , responseType: 'blob' });
    }
}