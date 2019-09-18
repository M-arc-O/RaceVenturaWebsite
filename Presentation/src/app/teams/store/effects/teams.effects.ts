import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { TeamsService } from '../../shared';
import * as teamsActions from './../actions/team.actions';
import { TeamActions } from './../actions/team.actions';


@Injectable()
export class TeamsEffects {

    constructor(
        private teamsService: TeamsService,
        private actions$: Actions
    ) { }

    @Effect() loadTeams$ = this.actions$.pipe(
        ofType(TeamActions.LOAD_TEAMS),
        switchMap(action => this.teamsService.getTeams((action as teamsActions.LoadTeamsAction).payload).pipe(
            map(teams => new teamsActions.LoadTeamsSuccesAction(teams)),
            catchError((error: Response) => of(new teamsActions.LoadTeamsErrorAction({ error: error }))))));

    @Effect() addTeam$ = this.actions$.pipe(
        ofType(TeamActions.ADD_TEAM),
        switchMap(action => this.teamsService.addTeam((action as teamsActions.AddTeamAction).payload).pipe(
            map(team => new teamsActions.AddTeamSuccesAction(team)),
            catchError((error: Response) => of(new teamsActions.AddTeamErrorAction({ error: error }))))));

    @Effect() deleteTeam$ = this.actions$.pipe(
        ofType(TeamActions.DELETE_TEAM),
        switchMap(action => this.teamsService.deleteTeam((action as teamsActions.DeleteTeamAction).payload).pipe(
            map(id => new teamsActions.DeleteTeamSuccesAction(id)),
            catchError((error: Response) => of(new teamsActions.DeleteTeamErrorAction({ error: error }))))));

    @Effect() loadTeamDetails$ = this.actions$.pipe(
        ofType(TeamActions.LOAD_TEAM_DETAILS),
        switchMap(action => this.teamsService.getTeamDetails((action as teamsActions.LoadTeamDetailsAction).payload).pipe(
            map(teamDetails => new teamsActions.LoadTeamDetailsSuccesAction(teamDetails)),
            catchError((error: Response) => of(new teamsActions.LoadTeamDetailsErrorAction({ error: error }))))));

    @Effect() editTeam$ = this.actions$.pipe(
        ofType(TeamActions.EDIT_TEAM),
        switchMap(action => this.teamsService.editTeam((action as teamsActions.EditTeamAction).payload).pipe(
            map(team => new teamsActions.EditTeamSuccesAction(team)),
            catchError((error: Response) => of(new teamsActions.EditTeamErrorAction({ error: error }))))));
}
