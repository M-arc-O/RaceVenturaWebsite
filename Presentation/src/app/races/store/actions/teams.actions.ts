import { HttpErrorResponse } from '@angular/common/http';
import { BaseErrorAction } from 'src/app/store';
import { TeamDetailViewModel } from '../../shared/models';

export enum TeamActions {
    ADD_TEAM = 'ADD_TEAM',
    ADD_TEAM_SUCCES = 'ADD_TEAM_SUCCES',
    ADD_TEAM_ERROR = 'ADD_TEAM_ERROR',
    EDIT_TEAM = 'EDIT_TEAM',
    EDIT_TEAM_SUCCES = 'EDIT_TEAM_SUCCES',
    EDIT_TEAM_ERROR = 'EDIT_TEAM_ERROR',
    DELETE_TEAM = 'DELETE_TEAM',
    DELETE_TEAM_SUCCES = 'DELETE_TEAM_SUCCES',
    DELETE_TEAM_ERROR = 'DELETE_TEAM_ERROR',
}

export class AddTeamAction {
    public readonly type = TeamActions.ADD_TEAM;
    constructor(public readonly payload: TeamDetailViewModel) { }
}

export class AddTeamSuccesAction {
    public readonly type = TeamActions.ADD_TEAM_SUCCES;
    constructor(public readonly payload: TeamDetailViewModel) { }
}

export class AddTeamErrorAction implements BaseErrorAction {
    public readonly type = TeamActions.ADD_TEAM_ERROR;
    constructor(public readonly payload: { error: HttpErrorResponse; }) { }
}

export class EditTeamAction {
    public readonly type = TeamActions.EDIT_TEAM;
    constructor(public readonly payload: TeamDetailViewModel) { }
}

export class EditTeamSuccesAction {
    public readonly type = TeamActions.EDIT_TEAM_SUCCES;
    constructor(public readonly payload: TeamDetailViewModel) { }
}

export class EditTeamErrorAction implements BaseErrorAction {
    public readonly type = TeamActions.EDIT_TEAM_ERROR;
    constructor(public readonly payload: { error: HttpErrorResponse; }) { }
}

export class DeleteTeamAction {
    public readonly type = TeamActions.DELETE_TEAM;
    constructor(public readonly payload: TeamDetailViewModel) { }
}

export class DeleteTeamSuccesAction {
    public readonly type = TeamActions.DELETE_TEAM_SUCCES;
    constructor(public readonly payload: string) { }
}

export class DeleteTeamErrorAction implements BaseErrorAction {
    public readonly type = TeamActions.DELETE_TEAM_ERROR;
    constructor(public readonly payload: { error: HttpErrorResponse; }) { }
}

export type TeamActionsUnion
    = AddTeamAction
    | AddTeamSuccesAction
    | AddTeamErrorAction
    | EditTeamAction
    | EditTeamSuccesAction
    | EditTeamErrorAction
    | DeleteTeamAction
    | DeleteTeamSuccesAction
    | DeleteTeamErrorAction;
