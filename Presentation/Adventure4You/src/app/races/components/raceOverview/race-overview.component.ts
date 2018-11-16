import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { RaceViewModel } from '../../shared/';
import { ComponentBase, AppState } from 'src/app/shared';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';

import * as racesActions from './../../actions/race.actions';

@Component({
    selector: 'app-race-overview',
    templateUrl: './race-overview.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class RaceOverviewComponent extends ComponentBase implements OnInit {
    races$: Observable<RaceViewModel[]>;
    selectedRace: RaceViewModel;

    constructor(
        private store: Store<AppState>) {
        super();
        this.races$ = this.store.select(state => state.racesFeature.races);
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
