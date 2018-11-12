import { RaceViewModel } from '../shared';
import { RaceDetailViewModel } from '../shared/models/race-detail-view-model';
import { AddRaceViewModel } from '../shared/models/add-race-view-model';

export const LOAD_RACES = 'LOAD_RACE';
export const LOAD_RACES_SUCCES = 'LOAD_RACE_SUCCES';
export const LOAD_RACE_DETAILS = 'LOAD_RACE_DETAILS';
export const LOAD_RACE_DETAILS_SUCCES = 'LOAD_RACE_DETAILS_SUCCES';
export const ADD_RACE = 'ADD_RACE';
export const ADD_RACE_SUCCES = 'ADD_RACE_SUCCES';

export class LoadRacesAction {
    readonly type = LOAD_RACES;
    constructor() { }
}

export class LoadRacesSuccesAction {
    readonly type = LOAD_RACES_SUCCES;
    constructor(public payload: RaceViewModel[]) { }
}

export class LoadRaceDetailsAction {
    readonly type = LOAD_RACE_DETAILS;
    constructor(public payload: number) { }
}

export class LoadRaceDetailsSuccesAction {
    readonly type = LOAD_RACE_DETAILS_SUCCES;
    constructor(public payload: RaceDetailViewModel) { }
}

export class AddRaceAction {
    readonly type = ADD_RACE;
    constructor(public payload: AddRaceViewModel) { }
}

export class AddRaceSuccesAction {
    readonly type = ADD_RACE_SUCCES;
    constructor(public payload: RaceViewModel) { }
}

export type Action
= LoadRacesAction
| LoadRacesSuccesAction
| LoadRaceDetailsAction
| LoadRaceDetailsSuccesAction
| AddRaceAction
| AddRaceSuccesAction;
