import { RaceViewModel } from '../../shared';
import { RaceDetailViewModel } from '../../shared/models/race-detail-view-model';
import { AddRaceViewModel } from '../../shared/models/add-race-view-model';
import { BaseErrorAction } from 'src/app/store/BaseErrorAction';
import { extendsDirectlyFromObject } from '@angular/core/src/render3/jit/directive';

export enum RaceActions {
    LOAD_RACES = 'LOAD_RACE',
    LOAD_RACES_SUCCES = 'LOAD_RACE_SUCCES',
    LOAD_RACES_ERROR = 'LOAD_RACE_ERROR',
    LOAD_RACE_DETAILS = 'LOAD_RACE_DETAILS',
    LOAD_RACE_DETAILS_SUCCES = 'LOAD_RACE_DETAILS_SUCCES',
    LOAD_RACE_DETAILS_ERROR = 'LOAD_RACE_DETAILS_ERROR',
    ADD_RACE = 'ADD_RACE',
    ADD_RACE_SUCCES = 'ADD_RACE_SUCCES',
    ADD_RACE_ERROR = 'ADD_RACE_ERROR',
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
    constructor(public readonly payload: { error: Response; }) { }
}

export class LoadRaceDetailsAction {
    public readonly type = RaceActions.LOAD_RACE_DETAILS;
    constructor(public readonly payload: number) { }
}

export class LoadRaceDetailsSuccesAction {
    public readonly type = RaceActions.LOAD_RACE_DETAILS_SUCCES;
    constructor(public readonly payload: RaceDetailViewModel) { }
}

export class LoadRaceDetailsErrorAction implements BaseErrorAction {
    public readonly type = RaceActions.LOAD_RACE_DETAILS_ERROR;
    constructor(public readonly payload: { error: Response; }) { }
}

export class AddRaceAction {
    public readonly type = RaceActions.ADD_RACE;
    constructor(public readonly payload: AddRaceViewModel) { }
}

export class AddRaceSuccesAction {
    public readonly type = RaceActions.ADD_RACE_SUCCES;
    constructor(public readonly payload: RaceViewModel) { }
}

export class AddRaceErrorAction implements BaseErrorAction {
    public readonly type = RaceActions.ADD_RACE_ERROR;
    constructor(public readonly payload: { error: Response; }) { }
}

export type RaceActionsUnion
    = LoadRacesAction
    | LoadRacesSuccesAction
    | LoadRacesErrorAction
    | LoadRaceDetailsAction
    | LoadRaceDetailsSuccesAction
    | LoadRaceDetailsErrorAction
    | AddRaceAction
    | AddRaceSuccesAction
    | AddRaceErrorAction;
