import { RaceViewModel } from '../shared';
import { IBase } from 'src/app/store/base.interface';
import { MemoizedSelector, createSelector } from '@ngrx/store';
import { racesStateSelector } from './racesState.interface';

export interface IRaces {
    races: RaceViewModel[];
    racesBase: IBase;
}

export const racesSelector: MemoizedSelector<object, RaceViewModel[]> = createSelector(racesStateSelector, s => s.races);
export const racesBaseSelector: MemoizedSelector<object, IBase> = createSelector(racesStateSelector, s => s.racesBase);
