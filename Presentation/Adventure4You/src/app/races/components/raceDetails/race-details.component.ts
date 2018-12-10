import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { Observable } from 'rxjs';
import { RaceDetailViewModel } from '../../shared';
import * as raceActions from '../../store/actions/race.actions';
import { IRacesState } from '../../store/racesState.interface';
import { selectedRaceSelector } from '../../store/selectedRace.interface';

@Component({
    selector: 'app-race-details',
    templateUrl: './race-details.component.html'
})
export class RaceDetailsComponent implements OnChanges {
    @Input() raceId: number;

    raceDetails$: Observable<RaceDetailViewModel>;

    constructor(
        private store: Store<IRacesState>) {
            this.raceDetails$ = this.store.pipe(select(selectedRaceSelector));
    }

    ngOnChanges(changes: SimpleChanges): void {
        this.store.dispatch(new raceActions.LoadRaceDetailsAction(this.raceId));
    }
}
