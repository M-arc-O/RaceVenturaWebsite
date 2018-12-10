import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { Observable } from 'rxjs';
import { ComponentBase } from 'src/app/shared';
import { RaceViewModel } from '../../shared/';
import * as racesActions from '../../store/actions/race.actions';
import { IRacesState } from '../../store/racesState.interface';
import { racesSelector, racesBaseSelector } from '../../store/races.interface';
import { IBase } from 'src/app/store/base.interface';

@Component({
    selector: 'app-race-overview',
    templateUrl: './race-overview.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class RaceOverviewComponent extends ComponentBase implements OnInit {
    races$: Observable<RaceViewModel[]>;
    racesBase$: Observable<IBase>;
    selectedRace: RaceViewModel;

    constructor(
        private store: Store<IRacesState>) {
        super();
        this.races$ = this.store.pipe(select(racesSelector));
        this.racesBase$ = this.store.pipe(select(racesBaseSelector));
    }

    ngOnInit(): void {
        this.getRaces();
    }

    getRaces(): void {
        this.store.dispatch(new racesActions.LoadRacesAction());
    }

    detailsClicked(race: RaceViewModel): void {
        this.selectedRace = race;
    }
}
