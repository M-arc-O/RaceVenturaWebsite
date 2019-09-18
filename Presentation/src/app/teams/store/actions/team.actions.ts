import { BaseErrorAction } from 'src/app/store/BaseErrorAction';
import { TeamDetailViewModel, TeamRequest, TeamViewModel } from '../../shared';

export enum TeamActions {
    LOAD_TEAMS = 'LOAD_TEAMS',
    LOAD_TEAMS_SUCCES = 'LOAD_TEAMS_SUCCES',
    LOAD_TEAMS_ERROR = 'LOAD_TEAMS_ERROR',
    ADD_TEAM = 'ADD_TEAM',
    ADD_TEAM_SUCCES = 'ADD_TEAM_SUCCES',
    ADD_TEAM_ERROR = 'ADD_TEAM_ERROR',
    DELETE_TEAM = 'DELETE_TEAM',
    DELETE_TEAM_SUCCES = 'DELETE_TEAM_SUCCES',
    DELETE_TEAM_ERROR = 'DELETE_TEAM_ERROR',
    LOAD_TEAM_DETAILS = 'LOAD_TEAM_DETAILS',
    LOAD_TEAM_DETAILS_SUCCES = 'LOAD_TEAM_DETAILS_SUCCES',
    LOAD_TEAM_DETAILS_ERROR = 'LOAD_TEAM_DETAILS_ERROR',
    EDIT_TEAM = 'EDIT_TEAM',
    EDIT_TEAM_SUCCES = 'EDIT_TEAM_SUCCES',
    EDIT_TEAM_ERROR = 'EDIT_TEAM_ERROR',
}

export class LoadTeamsAction {
    public readonly type = TeamActions.LOAD_TEAMS;
    constructor(public readonly payload: string) { }
}

export class LoadTeamsSuccesAction {
    public readonly type = TeamActions.LOAD_TEAMS_SUCCES;
    constructor(public readonly payload: TeamViewModel[]) { }
}

export class LoadTeamsErrorAction implements BaseErrorAction {
    public readonly type = TeamActions.LOAD_TEAMS_ERROR;
    constructor(public readonly payload: { error: Response; }) { }
}

export class AddTeamAction {
    public readonly type = TeamActions.ADD_TEAM;
    constructor(public readonly payload: TeamDetailViewModel) { }
}

export class AddTeamSuccesAction {
    public readonly type = TeamActions.ADD_TEAM_SUCCES;
    constructor(public readonly payload: TeamViewModel) { }
}

export class AddTeamErrorAction implements BaseErrorAction {
    public readonly type = TeamActions.ADD_TEAM_ERROR;
    constructor(public readonly payload: { error: Response; }) { }
}

export class DeleteTeamAction {
    public readonly type = TeamActions.DELETE_TEAM;
    constructor(public readonly payload: TeamViewModel) { }
}

export class DeleteTeamSuccesAction {
    public readonly type = TeamActions.DELETE_TEAM_SUCCES;
    constructor(public readonly payload: string) { }
}

export class DeleteTeamErrorAction implements BaseErrorAction {
    public readonly type = TeamActions.DELETE_TEAM_ERROR;
    constructor(public readonly payload: { error: Response; }) { }
}

export class LoadTeamDetailsAction {
    public readonly type = TeamActions.LOAD_TEAM_DETAILS;
    constructor(public readonly payload: TeamRequest) { }
}

export class LoadTeamDetailsSuccesAction {
    public readonly type = TeamActions.LOAD_TEAM_DETAILS_SUCCES;
    constructor(public readonly payload: TeamDetailViewModel) { }
}

export class LoadTeamDetailsErrorAction implements BaseErrorAction {
    public readonly type = TeamActions.LOAD_TEAM_DETAILS_ERROR;
    constructor(public readonly payload: { error: Response; }) { }
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
    constructor(public readonly payload: { error: Response; }) { }
}

export type TeamActionsUnion
    = LoadTeamsAction
    | LoadTeamsSuccesAction
    | LoadTeamsErrorAction
    | AddTeamAction
    | AddTeamSuccesAction
    | AddTeamErrorAction
    | DeleteTeamAction
    | DeleteTeamSuccesAction
    | DeleteTeamErrorAction
    | LoadTeamDetailsAction
    | LoadTeamDetailsSuccesAction
    | LoadTeamDetailsErrorAction
    | EditTeamAction
    | EditTeamSuccesAction
    | EditTeamErrorAction;
