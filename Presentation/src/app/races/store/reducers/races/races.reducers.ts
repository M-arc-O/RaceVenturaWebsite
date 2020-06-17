import { ActionReducerMap, combineReducers } from '@ngrx/store';
import { createBaseReducer } from 'src/app/store/base.reducer';
import { RaceActions } from '../../actions/race.actions';
import { IRaces } from '../../races.interface';
import { IRacesState } from '../../races-state.interface';
import { ISelectedRace } from '../../selected-race.interface';
import { pointsReducers } from '../points';
import { stagesReducers } from '../stages';
import { teamPointVisitedReducers } from '../teamPointVisited';
import { teamsReducers } from '../teams';
import { racesReducer } from './races.reducer';
import { selectedRaceReducer } from './selected-race.reducer';
import { teamResultsReducer } from './team-results.reducer';

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
    teamPointVisited: teamPointVisitedReducers,
    stages: stagesReducers,
    points: pointsReducers,
    teamResults: teamResultsReducer
});

export const racesReducers = combineReducers(<ActionReducerMap<IRacesState>>{
    races: racesCombinedReducers,
    selectedRace: selectedRaceCombinedReducer,
});
