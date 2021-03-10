import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { CarouselService } from 'src/app/components/carousel/carousel.service';
import { UserService } from 'src/app/shared';
import { IBase } from 'src/app/store/base.interface';
import { AddEditType } from '../../../shared';
import { RacesDownloadService } from '../../shared';
import { RaceStoreModel, RaceType } from '../../shared/models';
import { IRacesState, loadSelectedRaceSelector, selectedRaceSelector } from '../../store';
import * as raceActions from '../../store/actions/race.actions';
import { RaceComponentBase } from '../race-component-base.component';

@Component({
    selector: 'app-race-details',
    templateUrl: './race-details.component.html'
})
export class RaceDetailsComponent extends RaceComponentBase implements OnInit, OnChanges {
    @Input() raceId: string;
    
    public raceTypes = RaceType;

    public raceDetails$: Observable<RaceStoreModel>;
    public raceDetailsLoad$: Observable<IBase>;

    public addEditType = AddEditType;

    constructor(private store: Store<IRacesState>,
        private route: ActivatedRoute,
        private racesDownloadService: RacesDownloadService,
        private carouselService: CarouselService,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.carouselService.showCarousel$.next(false);
        this.raceDetails$ = this.store.pipe(select(selectedRaceSelector));
        this.raceDetailsLoad$ = this.store.pipe(select(loadSelectedRaceSelector));

        this.route.params.pipe(takeUntil(this.unsubscribe$)).subscribe(params => {
            if (params['id'] !== "") { 
                this.raceId = params['id'];
                this.store.dispatch(new raceActions.LoadRaceDetailsAction(this.raceId));
            }
          });
    }

    public ngOnInit(): void {
        this.raceDetailsLoad$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });
    }

    public ngOnChanges(changes: SimpleChanges): void {
        this.store.dispatch(new raceActions.LoadRaceDetailsAction(this.raceId));
    }

    public downloadPointsPdf(): void {
        this.racesDownloadService.downloadPointPdf(this.raceId).pipe(takeUntil(this.unsubscribe$)).subscribe(response => {
            let blob:any = new Blob([response], { type: 'application/pdf' });
            const url= window.URL.createObjectURL(blob);
            window.open(url);
        });
    }

    public downloadTeamsPdf(): void {
        this.racesDownloadService.downloadTeamsPdf(this.raceId).pipe(takeUntil(this.unsubscribe$)).subscribe(response => {
            let blob:any = new Blob([response], { type: 'application/pdf' });
            const url= window.URL.createObjectURL(blob);
            window.open(url);
        });
    }

    public downloadStagesAndRaceEndPdf(): void {
        this.racesDownloadService.downloadStagesAndRaceEndPdf(this.raceId).pipe(takeUntil(this.unsubscribe$)).subscribe(response => {
            let blob:any = new Blob([response], { type: 'application/pdf' });
            const url= window.URL.createObjectURL(blob);
            window.open(url);
        });
    }

    public RemoveRaceClicked(): void {
        this.store.dispatch(new raceActions.DeleteRaceAction(this.raceId));
    }

    public GetRaceDurationString(value: string): string {
        if (value !== null && value !== undefined) {
            let values = value.split(`:`);
            return `${values[0]} hours, ${values[1]} minutes`; 
        }

        return "";
    }
}
