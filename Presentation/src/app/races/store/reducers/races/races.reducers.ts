import { combineReducers, ActionReducerMap } from '@ngrx/store';
import { IRacesState } from '../../racesState.interface';
import { racesReducer } from './races.reducer';
import { selectedRaceReducer } from './selectedRace.reducer';
import { IRaces } from '../../races.interface';
import { ISelectedRace } from '../../selectedRace.interface';
import { createBaseReducer } from 'src/app/store/base.reducer';
import { RaceActions } from '../../actions/race.actions';
import { stagesReducers } from 'src/app/stages/store/reducers';
import { teamsReducers } from '../teams';

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
    stagesState: stagesReducers,
    teams: teamsReducers
});

export const racesReducers = combineReducers(<ActionReducerMap<IRacesState>>{
    races: racesCombinedReducers,
    selectedRace: selectedRaceCombinedReducer,
});
