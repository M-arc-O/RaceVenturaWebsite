import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { map, switchMap } from 'rxjs/operators';
import { RaceService } from '../../shared';
import { RaceActions } from './../actions/race.actions';
import * as racesActions from './../actions/race.actions';


@Injectable()
export class RacesEffects {

    constructor(
        private raceService: RaceService,
        private actions$: Actions
    ) { }

    @Effect() loadRaces$ = this.actions$.pipe(
        ofType(RaceActions.LOAD_RACES),
        switchMap(() => this.raceService.getRaces()),
        map(races => new racesActions.LoadRacesSuccesAction(races)));

    @Effect() addRace$ = this.actions$.pipe(
        ofType(RaceActions.ADD_RACE),
        switchMap(action => this.raceService.addRace((action as racesActions.AddRaceAction).payload)),
        map(race => new racesActions.AddRaceSuccesAction(race)));

    @Effect() loadRaceDetails$ = this.actions$.pipe(
        ofType(RaceActions.LOAD_RACE_DETAILS),
        switchMap(action => this.raceService.getRaceDetails((action as racesActions.LoadRaceDetailsAction).payload)),
        map(raceDetails => new racesActions.LoadRaceDetailsSuccesAction(raceDetails)));
}
