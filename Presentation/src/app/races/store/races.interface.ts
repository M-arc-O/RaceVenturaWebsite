import { RaceViewModel } from '../shared/models';
import { IBase } from 'src/app/store';
import { MemoizedSelector, createSelector } from '@ngrx/store';
import { racesStateSelector } from './races-state.interface';

export interface IRaces {
    races: RaceViewModel[];
    load: IBase;
    add: IBase;
    delete: IBase;
}

export const racesSelector: MemoizedSelector<object, RaceViewModel[]> = createSelector(racesStateSelector, s => s.races);
export const loadRacesSelector: MemoizedSelector<object, IBase> = createSelector(racesStateSelector, s => s.load);
export const addRaceSelector: MemoizedSelector<object, IBase> = createSelector(racesStateSelector, s => s.add);
export const deleteRaceSelector: MemoizedSelector<object, IBase> = createSelector(racesStateSelector, s => s.delete);
