import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { TeamsService } from '../../shared';
import * as teamsActions from '../actions/teams.actions';
import { TeamActions } from '../actions/teams.actions';

@Injectable()
export class TeamsEffects {

    constructor(
        private teamsService: TeamsService,
        private actions$: Actions
    ) { }

    @Effect() addTeam$ = this.actions$.pipe(
        ofType(TeamActions.ADD_TEAM),
        switchMap(action => this.teamsService.addTeam((action as teamsActions.AddTeamAction).payload).pipe(
            map(team => new teamsActions.AddTeamSuccesAction(team)),
            catchError((error: HttpErrorResponse) => of(new teamsActions.AddTeamErrorAction({ error: error }))))));

    @Effect() deleteTeam$ = this.actions$.pipe(
        ofType(TeamActions.DELETE_TEAM),
        switchMap(action => this.teamsService.deleteTeam((action as teamsActions.DeleteTeamAction).payload).pipe(
            map(id => new teamsActions.DeleteTeamSuccesAction(id)),
            catchError((error: HttpErrorResponse) => of(new teamsActions.DeleteTeamErrorAction({ error: error }))))));

    @Effect() editTeam$ = this.actions$.pipe(
        ofType(TeamActions.EDIT_TEAM),
        switchMap(action => this.teamsService.editTeam((action as teamsActions.EditTeamAction).payload).pipe(
            map(team => new teamsActions.EditTeamSuccesAction(team)),
            catchError((error: HttpErrorResponse) => of(new teamsActions.EditTeamErrorAction({ error: error }))))));
}
