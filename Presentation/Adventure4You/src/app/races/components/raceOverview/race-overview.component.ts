import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ComponentBase, UserService } from 'src/app/shared';
import { RaceViewModel } from '../../shared/';
import * as racesActions from '../../store/actions/race.actions';
import { loadRaceSelector, racesSelector } from '../../store/races.interface';
import { IRacesState } from '../../store/racesState.interface';
import { IBase } from 'src/app/store/base.interface';

@Component({
    selector: 'app-race-overview',
    templateUrl: './race-overview.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class RaceOverviewComponent extends ComponentBase implements OnInit {
    public races$: Observable<RaceViewModel[]>;
    public loadRacesBase$: Observable<IBase>;
    public selectedRace: RaceViewModel;

    constructor(
        private store: Store<IRacesState>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.races$ = this.store.pipe(select(racesSelector));
        this.loadRacesBase$ = this.store.pipe(select(loadRaceSelector));
    }

    ngOnInit(): void {
        this.loadRacesBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });
        this.getRaces();
    }

    getRaces(): void {
        this.store.dispatch(new racesActions.LoadRacesAction());
    }

    detailsClicked(race: RaceViewModel): void {
        this.selectedRace = race;
    }
}
