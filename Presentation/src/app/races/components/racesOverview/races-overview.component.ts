import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ComponentBase, UserService } from 'src/app/shared';
import { IBase } from 'src/app/store/base.interface';
import { RaceViewModel } from '../../shared/';
import { deleteRaceSelector, IRacesState, loadRacesSelector, racesSelector } from '../../store';
import * as racesActions from '../../store/actions/race.actions';
import { AddEditType } from '../../../shared';

@Component({
    selector: 'app-race-overview',
    templateUrl: './races-overview.component.html',
    styleUrls: ['./races-overview.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class RacesOverviewComponent extends ComponentBase implements OnInit {
    public races$: Observable<RaceViewModel[]>;
    public loadRacesBase$: Observable<IBase>;
    public deleteRaceBase$: Observable<IBase>;
    public selectedRace: RaceViewModel;
    public addEditType = AddEditType;

    constructor(
        private store: Store<IRacesState>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.races$ = this.store.pipe(select(racesSelector));
        this.loadRacesBase$ = this.store.pipe(select(loadRacesSelector));
        this.deleteRaceBase$ = this.store.pipe(select(deleteRaceSelector));
    }

    ngOnInit(): void {
        this.resetSelectedRace();
        this.loadRacesBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });

        this.deleteRaceBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.success) {
                this.resetSelectedRace();
            }

            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });

        this.getRaces();
    }

    private resetSelectedRace(): void {
        this.selectedRace = new RaceViewModel();
        this.selectedRace.raceId = undefined;
    }

    private getRaces(): void {
        this.store.dispatch(new racesActions.LoadRacesAction());
    }

    detailsClicked(race: RaceViewModel): void {
        this.selectedRace = race;
    }
}
