import { Injectable } from '@angular/core';
import { Effect, Actions, ofType } from '@ngrx/effects';
import { RaceService } from '../shared';
import { switchMap, map } from 'rxjs/operators';

import * as racesActions from './../actions/race.actions';

@Injectable()
export class RacesEffects {

    constructor(
        private raceService: RaceService,
        private actions$: Actions
    ) { }

    @Effect() loadRaces$ = this.actions$.pipe(
        ofType(racesActions.LOAD_RACES),
        switchMap(() => {
            console.log('Load races switchmap');
            return this.raceService.getRaces();
        }),
        map(races => {
            console.log('Load races map');
            return new racesActions.LoadRacesSuccesAction(races);
        }));

    @Effect() addRace$ = this.actions$.pipe(
        ofType(racesActions.ADD_RACE),
        switchMap(action => {
            console.log('Add race switchmap');
            return this.raceService.addRace((action as racesActions.AddRaceAction).payload);
        }),
        map(race => {
            console.log('Add races map');
            return new racesActions.AddRaceSuccesAction(race);
        }));
}
