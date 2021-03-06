import { HttpErrorResponse } from '@angular/common/http';
import { BaseErrorAction } from 'src/app/store';
import { RaceViewModel, RaceStoreModel, RaceDetailViewModel, TeamResultViewModel } from '../../shared/models';

export enum RaceActions {
    LOAD_RACES = 'LOAD_RACES',
    LOAD_RACES_SUCCES = 'LOAD_RACES_SUCCES',
    LOAD_RACES_ERROR = 'LOAD_RACES_ERROR',
    ADD_RACE = 'ADD_RACE',
    ADD_RACE_SUCCES = 'ADD_RACE_SUCCES',
    ADD_RACE_ERROR = 'ADD_RACE_ERROR',
    EDIT_RACE = 'EDIT_RACE',
    EDIT_RACE_SUCCES = 'EDIT_RACE_SUCCES',
    EDIT_RACE_ERROR = 'EDIT_RACE_ERROR',
    DELETE_RACE = 'DELETE_RACE',
    DELETE_RACE_SUCCES = 'DELETE_RACE_SUCCES',
    DELETE_RACE_ERROR = 'DELETE_RACE_ERROR',
    LOAD_RACE_DETAILS = 'LOAD_RACE_DETAILS',
    LOAD_RACE_DETAILS_SUCCES = 'LOAD_RACE_DETAILS_SUCCES',
    LOAD_RACE_DETAILS_ERROR = 'LOAD_RACE_DETAILS_ERROR',
    GET_RACE_RESULT = 'GET_RACE_RESULT',
    GET_RACE_RESULT_SUCCES = 'GET_RACE_RESULT_SUCCES',
    GET_RACE_RESULT_ERROR = 'GET_RACE_RESULT_ERROR',
}

export class LoadRacesAction {
    public readonly type = RaceActions.LOAD_RACES;
    constructor() { }
}

export class LoadRacesSuccesAction {
    public readonly type = RaceActions.LOAD_RACES_SUCCES;
    constructor(public readonly payload: RaceViewModel[]) { }
}

export class LoadRacesErrorAction implements BaseErrorAction {
    public readonly type = RaceActions.LOAD_RACES_ERROR;
    constructor(public readonly payload: { error: HttpErrorResponse; }) { }
}

export class AddRaceAction {
    public readonly type = RaceActions.ADD_RACE;
    constructor(public readonly payload: RaceStoreModel) { }
}

export class AddRaceSuccesAction {
    public readonly type = RaceActions.ADD_RACE_SUCCES;
    constructor(public readonly payload: RaceViewModel) { }
}

export class AddRaceErrorAction implements BaseErrorAction {
    public readonly type = RaceActions.ADD_RACE_ERROR;
    constructor(public readonly payload: { error: HttpErrorResponse; }) { }
}

export class EditRaceAction {
    public readonly type = RaceActions.EDIT_RACE;
    constructor(public readonly payload: RaceStoreModel) { }
}

export class EditRaceSuccesAction {
    public readonly type = RaceActions.EDIT_RACE_SUCCES;
    constructor(public readonly payload: RaceStoreModel) { }
}

export class EditRaceErrorAction implements BaseErrorAction {
    public readonly type = RaceActions.EDIT_RACE_ERROR;
    constructor(public readonly payload: { error: HttpErrorResponse; }) { }
}

export class DeleteRaceAction {
    public readonly type = RaceActions.DELETE_RACE;
    constructor(public readonly payload: string) { }
}

export class DeleteRaceSuccesAction {
    public readonly type = RaceActions.DELETE_RACE_SUCCES;
    constructor(public readonly payload: string) { }
}

export class DeleteRaceErrorAction implements BaseErrorAction {
    public readonly type = RaceActions.DELETE_RACE_ERROR;
    constructor(public readonly payload: { error: HttpErrorResponse; }) { }
}

export class LoadRaceDetailsAction {
    public readonly type = RaceActions.LOAD_RACE_DETAILS;
    constructor(public readonly payload: string) { }
}

export class LoadRaceDetailsSuccesAction {
    public readonly type = RaceActions.LOAD_RACE_DETAILS_SUCCES;
    constructor(public readonly payload: RaceDetailViewModel) { }
}

export class LoadRaceDetailsErrorAction implements BaseErrorAction {
    public readonly type = RaceActions.LOAD_RACE_DETAILS_ERROR;
    constructor(public readonly payload: { error: HttpErrorResponse; }) { }
}

export class GetRaceResultAction {
    public readonly type = RaceActions.GET_RACE_RESULT;
    constructor(public readonly payload: string) { }
}

export class GetRaceResultSuccesAction {
    public readonly type = RaceActions.GET_RACE_RESULT_SUCCES;
    constructor(public readonly payload: TeamResultViewModel[]) { }
}

export class GetRaceResultErrorAction implements BaseErrorAction {
    public readonly type = RaceActions.GET_RACE_RESULT_ERROR;
    constructor(public readonly payload: { error: HttpErrorResponse; }) { }
}

export type RaceActionsUnion
    = LoadRacesAction
    | LoadRacesSuccesAction
    | LoadRacesErrorAction
    | AddRaceAction
    | AddRaceSuccesAction
    | AddRaceErrorAction
    | EditRaceAction
    | EditRaceSuccesAction
    | EditRaceErrorAction
    | DeleteRaceAction
    | DeleteRaceSuccesAction
    | DeleteRaceErrorAction
    | LoadRaceDetailsAction
    | LoadRaceDetailsSuccesAction
    | LoadRaceDetailsErrorAction
    | GetRaceResultAction
    | GetRaceResultSuccesAction
    | GetRaceResultErrorAction;
