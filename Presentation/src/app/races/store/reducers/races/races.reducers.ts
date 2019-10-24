import { ActionReducerMap, combineReducers } from '@ngrx/store';
import { createBaseReducer } from 'src/app/store/base.reducer';
import { RaceActions } from '../../actions/race.actions';
import { IRaces } from '../../races.interface';
import { IRacesState } from '../../racesState.interface';
import { ISelectedRace } from '../../selectedRace.interface';
import { stagesReducers } from '../stages';
import { teamsReducers } from '../teams';
import { racesReducer } from './races.reducer';
import { selectedRaceReducer } from './selectedRace.reducer';

export const racesCombinedReducers = combineReducers(<ActionReducerMap<IRaces>>{
    races: racesReducer,
    load: createBaseReducer(RaceActions.LOAD_RACES, RaceActions.LOAD_RACES_SUCCES, RaceActions.LOAD_RACES_ERROR),
    add: createBaseReducer(RaceActions.ADD_RACE, RaceActions.ADD_RACE_SUCCES, RaceActions.ADD_RACE_ERROR),
    delete: createBaseReducer(RaceActions.DELETE_RACE, RaceActions.DELETE_RACE_SUCCES, RaceActions.DELETE_RACE_ERROR)
});

export const selectedRaceCombinedReducer = combineReducers(<ActionReducerMap<ISelectedRace>>{
    selectedRace: selectedRaceReducer,
    load: createBaseReducer(RaceActions.LOAD_RACE_DETAILS, RaceActions.LOAD_RACE_DETAILS_SUCCES,
        RaceActions.LOAD_RACE_DETAILS_ERROR),
    edit: createBaseReducer(RaceActions.EDIT_RACE, RaceActions.EDIT_RACE_SUCCES, RaceActions.EDIT_RACE_ERROR, RaceActions.LOAD_RACES),
    teams: teamsReducers,
    stages: stagesReducers
});

export const racesReducers = combineReducers(<ActionReducerMap<IRacesState>>{
    races: racesCombinedReducers,
    selectedRace: selectedRaceCombinedReducer,
});
