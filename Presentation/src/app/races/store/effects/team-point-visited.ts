import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { TeamsService } from '../../shared';
import * as teamsActions from '../actions';

@Injectable()
export class TeamPointVisitedEffects {

    constructor(
        private teamsService: TeamsService,
        private actions$: Actions
    ) { }

    @Effect() addTeamPointVisited$ = this.actions$.pipe(
        ofType(teamsActions.TeamPointVisitedActions.ADD_TEAM_POINT_VISITED),
        switchMap(action => this.teamsService.addTeamPointVisited((action as teamsActions.AddTeamPointVisitedAction).payload).pipe(
            map(point => new teamsActions.AddTeamPointVisitedSuccesAction(point)),
            catchError((error: HttpErrorResponse) => of(new teamsActions.AddTeamPointVisitedErrorAction({ error: error }))))));

    @Effect() deleteTeamPointVisited$ = this.actions$.pipe(
        ofType(teamsActions.TeamPointVisitedActions.DELETE_TEAM_POINT_VISITED),
        switchMap(action => this.teamsService.deleteTeamPointVisited((action as teamsActions.DeleteTeamPointVisitedAction).payload).pipe(
            map(id => new teamsActions.DeleteTeamPointVisitedSuccesAction(id)),
            catchError((error: HttpErrorResponse) => of(new teamsActions.DeleteTeamPointVisitedErrorAction({ error: error }))))));
}
