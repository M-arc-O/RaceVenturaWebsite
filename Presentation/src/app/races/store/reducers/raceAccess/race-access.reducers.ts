import { ActionReducerMap, combineReducers } from "@ngrx/store";
import { createBaseReducer } from "src/app/store";
import { IRaceAccess } from "../../access.interface";
import { RaceAccessActions } from "../../actions";
import { raceAccessReducer } from "./race-access.reducer";

export const raceAccessReducers = combineReducers(<ActionReducerMap<IRaceAccess>>{
    accesses: raceAccessReducer,
    load: createBaseReducer(RaceAccessActions.LOAD_RACE_ACCESS, RaceAccessActions.LOAD_RACE_ACCESS_SUCCES, RaceAccessActions.LOAD_RACE_ACCESS_ERROR),
    add: createBaseReducer(RaceAccessActions.ADD_RACE_ACCESS, RaceAccessActions.ADD_RACE_ACCESS_SUCCES, RaceAccessActions.ADD_RACE_ACCESS_ERROR),
    edit: createBaseReducer(RaceAccessActions.EDIT_RACE_ACCESS, RaceAccessActions.EDIT_RACE_ACCESS_SUCCES, RaceAccessActions.EDIT_RACE_ACCESS_ERROR),
    delete: createBaseReducer(RaceAccessActions.DELETE_RACE_ACCESS, RaceAccessActions.DELETE_RACE_ACCESS_SUCCES, RaceAccessActions.DELETE_RACE_ACCESS_ERROR)
});