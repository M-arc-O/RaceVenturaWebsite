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
        switchMap(() => this.raceService.getRaces()),
        map(races => new racesActions.LoadRacesSuccesAction(races)));

    @Effect() addRace$ = this.actions$.pipe(
        ofType(racesActions.ADD_RACE),
        switchMap(action => this.raceService.addRace((action as racesActions.AddRaceAction).payload)),
        map(race => new racesActions.AddRaceSuccesAction(race)));

    @Effect() loadRaceDetails$ = this.actions$.pipe(
        ofType(racesActions.LOAD_RACE_DETAILS),
        switchMap(action => this.raceService.getRaceDetails((action as racesActions.LoadRaceDetailsAction).payload)),
        map(raceDetails => new racesActions.LoadRaceDetailsSuccesAction(raceDetails)));
}
