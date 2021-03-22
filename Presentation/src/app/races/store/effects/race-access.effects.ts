import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { RacesAccessService } from '../../shared';
import * as racesAccessActions from '../actions/race-access.actions';
import { RaceAccessActions } from '../actions/race-access.actions';


@Injectable()
export class RaceAccessEffects {
    constructor(
        private racesAccessService: RacesAccessService,
        private actions$: Actions
    ) { }

    @Effect() loadRacesAccess$ = this.actions$.pipe(
        ofType(RaceAccessActions.LOAD_RACE_ACCESS),
        switchMap(action => this.racesAccessService.getRaceAccess((action as racesAccessActions.LoadRaceAccessAction).payload).pipe(
            map(accesses => new racesAccessActions.LoadRaceAccessSuccesAction(accesses)),
            catchError((error: HttpErrorResponse) => of(new racesAccessActions.LoadRaceAccessErrorAction({ error: error }))))));

    @Effect() addRace$ = this.actions$.pipe(
        ofType(RaceAccessActions.ADD_RACE_ACCESS),
        switchMap(action => this.racesAccessService.addRaceAccess((action as racesAccessActions.AddRaceAccessAction).payload).pipe(
            map(access => new racesAccessActions.AddRaceAccessSuccesAction(access)),
            catchError((error: HttpErrorResponse) => of(new racesAccessActions.AddRaceAccessErrorAction({ error: error }))))));

    @Effect() editRace$ = this.actions$.pipe(
        ofType(RaceAccessActions.EDIT_RACE_ACCESS),
        switchMap(action => this.racesAccessService.editRaceAccess((action as racesAccessActions.EditRaceAccessAction).payload).pipe(
            map(access => new racesAccessActions.EditRaceAccessSuccesAction(access)),
            catchError((error: HttpErrorResponse) => of(new racesAccessActions.EditRaceAccessErrorAction({ error: error }))))));

    @Effect() deleteRace$ = this.actions$.pipe(
        ofType(RaceAccessActions.DELETE_RACE_ACCESS),
        switchMap(action => this.racesAccessService.deleteRaceAccess((action as racesAccessActions.DeleteRaceAccessAction).payload.raceId, (action as racesAccessActions.DeleteRaceAccessAction).payload.userEmail).pipe(
            map(access => { return new racesAccessActions.DeleteRaceAccessSuccesAction(access.userEmail) }),
            catchError((error: HttpErrorResponse) => of(new racesAccessActions.DeleteRaceAccessErrorAction({ error: error }))))));
}
