import { combineReducers, ActionReducerMap } from '@ngrx/store';
import { IRacesState } from '../racesState.interface';
import { racesReducer } from './races.reducer';
import { selectedRaceReducer } from './selectedRace.reducer';
import { IRaces } from '../races.interface';
import { ISelectedRace } from '../selectedRace.interface';
import { createBaseReducer } from 'src/app/store/base.reducer';
import { RaceActions } from '../actions/race.actions';

export const racesCombinedReducers = combineReducers(<ActionReducerMap<IRaces>> {
    races: racesReducer,
    racesBase: createBaseReducer(RaceActions.LOAD_RACES, RaceActions.LOAD_RACES_SUCCES, RaceActions.LOAD_RACES_ERROR)
});

export const selectedRaceCombinedReducer = combineReducers(<ActionReducerMap<ISelectedRace>> {
    selectedRace: selectedRaceReducer,
    selectedRaceBase: createBaseReducer(RaceActions.LOAD_RACE_DETAILS, RaceActions.LOAD_RACE_DETAILS_SUCCES,
        RaceActions.LOAD_RACE_DETAILS_ERROR)
});

export const racesReducers = combineReducers(<ActionReducerMap<IRacesState>> {
    races: racesCombinedReducers,
    selectedRace: selectedRaceCombinedReducer
});
