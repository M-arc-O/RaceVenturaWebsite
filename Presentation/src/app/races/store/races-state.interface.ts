import { createFeatureSelector, createSelector, MemoizedSelector } from '@ngrx/store';
import { IRaces } from '.';
import { ISelectedRace } from '.';
import { IRaceAccess } from './access.interface';

export interface IRacesState {
    races: IRaces;
    selectedRace: ISelectedRace;
    access: IRaceAccess;
}

export const racesFeatureSelector: MemoizedSelector<object, IRacesState> = createFeatureSelector<IRacesState>('racesFeature');

export const racesStateSelector: MemoizedSelector<object, IRaces> = createSelector(racesFeatureSelector, s => s.races);
export const selectedRaceStateSelector: MemoizedSelector<object, ISelectedRace> = createSelector(racesFeatureSelector, s => s.selectedRace);
export const raceAccessSelector: MemoizedSelector<object, IRaceAccess> = createSelector(racesFeatureSelector, s => s.access);
