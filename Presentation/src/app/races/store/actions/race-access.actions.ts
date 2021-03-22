import { HttpErrorResponse } from '@angular/common/http';
import { BaseErrorAction } from 'src/app/store';
import { RaceAccessViewModel } from '../../shared/models';

export enum RaceAccessActions {
    LOAD_RACE_ACCESS = 'LOAD_RACE_ACCESS',
    LOAD_RACE_ACCESS_SUCCES = 'LOAD_RACE_ACCESS_SUCCES',
    LOAD_RACE_ACCESS_ERROR = 'LOAD_RACE_ACCESS_ERROR',
    ADD_RACE_ACCESS = 'ADD_RACE_ACCESS',
    ADD_RACE_ACCESS_SUCCES = 'ADD_RACE_ACCESS_SUCCES',
    ADD_RACE_ACCESS_ERROR = 'ADD_RACE_ACCESS_ERROR',
    EDIT_RACE_ACCESS = 'EDIT_RACE_ACCESS',
    EDIT_RACE_ACCESS_SUCCES = 'EDIT_RACE_ACCESS_SUCCES',
    EDIT_RACE_ACCESS_ERROR = 'EDIT_RACE_ACCESS_ERROR',
    DELETE_RACE_ACCESS = 'DELETE_RACE_ACCESS',
    DELETE_RACE_ACCESS_SUCCES = 'DELETE_RACE_ACCESS_SUCCES',
    DELETE_RACE_ACCESS_ERROR = 'DELETE_RACE_ACCESS_ERROR'
}

export class LoadRaceAccessAction {
    public readonly type = RaceAccessActions.LOAD_RACE_ACCESS;
    constructor(public readonly payload: string) { }
}

export class LoadRaceAccessSuccesAction {
    public readonly type = RaceAccessActions.LOAD_RACE_ACCESS_SUCCES;
    constructor(public readonly payload: RaceAccessViewModel[]) { }
}

export class LoadRaceAccessErrorAction implements BaseErrorAction {
    public readonly type = RaceAccessActions.LOAD_RACE_ACCESS_ERROR;
    constructor(public readonly payload: { error: HttpErrorResponse; }) { }
}

export class AddRaceAccessAction {
    public readonly type = RaceAccessActions.ADD_RACE_ACCESS;
    constructor(public readonly payload: RaceAccessViewModel) { }
}

export class AddRaceAccessSuccesAction {
    public readonly type = RaceAccessActions.ADD_RACE_ACCESS_SUCCES;
    constructor(public readonly payload: RaceAccessViewModel) { }
}

export class AddRaceAccessErrorAction implements BaseErrorAction {
    public readonly type = RaceAccessActions.ADD_RACE_ACCESS_ERROR;
    constructor(public readonly payload: { error: HttpErrorResponse; }) { }
}

export class EditRaceAccessAction {
    public readonly type = RaceAccessActions.EDIT_RACE_ACCESS;
    constructor(public readonly payload: RaceAccessViewModel) { }
}

export class EditRaceAccessSuccesAction {
    public readonly type = RaceAccessActions.EDIT_RACE_ACCESS_SUCCES;
    constructor(public readonly payload: RaceAccessViewModel) { }
}

export class EditRaceAccessErrorAction implements BaseErrorAction {
    public readonly type = RaceAccessActions.EDIT_RACE_ACCESS_ERROR;
    constructor(public readonly payload: { error: HttpErrorResponse; }) { }
}

export class DeleteRaceAccessAction {
    public readonly type = RaceAccessActions.DELETE_RACE_ACCESS;
    constructor(public readonly payload: RaceAccessViewModel) { }
}

export class DeleteRaceAccessSuccesAction {
    public readonly type = RaceAccessActions.DELETE_RACE_ACCESS_SUCCES;
    constructor(public readonly payload: string) { }
}

export class DeleteRaceAccessErrorAction implements BaseErrorAction {
    public readonly type = RaceAccessActions.DELETE_RACE_ACCESS_ERROR;
    constructor(public readonly payload: { error: HttpErrorResponse; }) { }
}

export type RaceAccessActionsUnion
    = LoadRaceAccessAction
    | LoadRaceAccessSuccesAction
    | LoadRaceAccessErrorAction
    | AddRaceAccessAction
    | AddRaceAccessSuccesAction
    | AddRaceAccessErrorAction
    | EditRaceAccessAction
    | EditRaceAccessSuccesAction
    | EditRaceAccessErrorAction
    | DeleteRaceAccessAction
    | DeleteRaceAccessSuccesAction
    | DeleteRaceAccessErrorAction
