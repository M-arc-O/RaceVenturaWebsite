import { HttpErrorResponse } from '@angular/common/http';
import { BaseErrorAction } from 'src/app/store';
import { TeamPointVisitedViewModel } from '../../shared/models';

export enum TeamPointVisitedActions {
    ADD_TEAM_POINT_VISITED = 'ADD_TEAM_POINT_VISITED',
    ADD_TEAM_POINT_VISITED_SUCCES = 'ADD_TEAM_POINT_VISITED_SUCCES',
    ADD_TEAM_POINT_VISITED_ERROR = 'ADD_TEAM_POINT_VISITED_ERROR',
    DELETE_TEAM_POINT_VISITED = 'DELETE_POINT_VISITED_TEAM',
    DELETE_TEAM_POINT_VISITED_SUCCES = 'DELETE_TEAM_POINT_VISITED_SUCCES',
    DELETE_TEAM_POINT_VISITED_ERROR = 'DELETE_TEAM_POINT_VISITED_ERROR',
}

export class AddTeamPointVisitedAction {
    public readonly type = TeamPointVisitedActions.ADD_TEAM_POINT_VISITED;
    constructor(public readonly payload: TeamPointVisitedViewModel) { }
}

export class AddTeamPointVisitedSuccesAction {
    public readonly type = TeamPointVisitedActions.ADD_TEAM_POINT_VISITED_SUCCES;
    constructor(public readonly payload: TeamPointVisitedViewModel) { }
}

export class AddTeamPointVisitedErrorAction implements BaseErrorAction {
    public readonly type = TeamPointVisitedActions.ADD_TEAM_POINT_VISITED_ERROR;
    constructor(public readonly payload: { error: HttpErrorResponse; }) { }
}

export class DeleteTeamPointVisitedAction {
    public readonly type = TeamPointVisitedActions.DELETE_TEAM_POINT_VISITED;
    constructor(public readonly payload: TeamPointVisitedViewModel) { }
}

export class DeleteTeamPointVisitedSuccesAction {
    public readonly type = TeamPointVisitedActions.DELETE_TEAM_POINT_VISITED_SUCCES;
    constructor(public readonly payload: string) { }
}

export class DeleteTeamPointVisitedErrorAction implements BaseErrorAction {
    public readonly type = TeamPointVisitedActions.DELETE_TEAM_POINT_VISITED_ERROR;
    constructor(public readonly payload: { error: HttpErrorResponse; }) { }
}

export type TeamPointVisitedActionsUnion
    = AddTeamPointVisitedAction
    | AddTeamPointVisitedSuccesAction
    | AddTeamPointVisitedErrorAction
    | DeleteTeamPointVisitedAction
    | DeleteTeamPointVisitedSuccesAction
    | DeleteTeamPointVisitedErrorAction;
