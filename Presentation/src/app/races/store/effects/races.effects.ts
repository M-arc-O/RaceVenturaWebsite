import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { RacesService } from '../../shared';
import * as racesActions from './../actions/race.actions';
import { RaceActions } from './../actions/race.actions';


@Injectable()
export class RacesEffects {

    constructor(
        private racesService: RacesService,
        private actions$: Actions
    ) { }

    @Effect() loadRaces$ = this.actions$.pipe(
        ofType(RaceActions.LOAD_RACES),
        switchMap(() => this.racesService.getRaces().pipe(
            map(races => new racesActions.LoadRacesSuccesAction(races)),
            catchError((error: HttpErrorResponse) => of(new racesActions.LoadRacesErrorAction({ error: error }))))));

    @Effect() addRace$ = this.actions$.pipe(
        ofType(RaceActions.ADD_RACE),
        switchMap(action => this.racesService.addRace((action as racesActions.AddRaceAction).payload).pipe(
            map(race => new racesActions.AddRaceSuccesAction(race)),
            catchError((error: HttpErrorResponse) => of(new racesActions.AddRaceErrorAction({ error: error }))))));

    @Effect() deleteRace$ = this.actions$.pipe(
        ofType(RaceActions.DELETE_RACE),
        switchMap(action => this.racesService.deleteRace((action as racesActions.DeleteRaceAction).payload).pipe(
            map(id => new racesActions.DeleteRaceSuccesAction(id)),
            catchError((error: HttpErrorResponse) => of(new racesActions.DeleteRaceErrorAction({ error: error }))))));

    @Effect() loadRaceDetails$ = this.actions$.pipe(
        ofType(RaceActions.LOAD_RACE_DETAILS),
        switchMap(action => this.racesService.getRaceDetails((action as racesActions.LoadRaceDetailsAction).payload).pipe(
            map(raceDetails => new racesActions.LoadRaceDetailsSuccesAction(raceDetails)),
            catchError((error: HttpErrorResponse) => of(new racesActions.LoadRaceDetailsErrorAction({ error: error }))))));

    @Effect() editRace$ = this.actions$.pipe(
        ofType(RaceActions.EDIT_RACE),
        switchMap(action => this.racesService.editRace((action as racesActions.EditRaceAction).payload).pipe(
            map(race => new racesActions.EditRaceSuccesAction(race)),
            catchError((error: HttpErrorResponse) => of(new racesActions.EditRaceErrorAction({ error: error }))))));
}
