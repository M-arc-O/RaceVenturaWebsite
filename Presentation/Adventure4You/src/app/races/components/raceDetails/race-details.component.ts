import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/shared';
import { Observable } from 'rxjs';
import { RaceDetailViewModel } from '../../shared';

import * as raceActions from './../../actions/race.actions';

@Component({
    selector: 'app-race-details',
    templateUrl: './race-details.component.html'
})
export class RaceDetailsComponent implements OnChanges {
    @Input() raceId: number;

    raceDetails$: Observable<RaceDetailViewModel>;

    constructor(
        private store: Store<AppState>) {
        this.raceDetails$ = this.store.select(state => state.racesFeature.selectedRace);
        this.raceDetails$.subscribe(details => console.log(details));
    }

    ngOnChanges(changes: SimpleChanges): void {
        console.log(this.raceId);
        this.store.dispatch(new raceActions.LoadRaceDetailsAction(this.raceId));
    }
}
