import { createSelector, MemoizedSelector } from '@ngrx/store';
import { IBase } from 'src/app/store';
import { RaceAccessViewModel } from '../shared/models';
import { raceAccessSelector } from './races-state.interface';

export interface IRaceAccess {
    accesses: RaceAccessViewModel[];
    load: IBase;
    add: IBase;
    edit: IBase;
    delete: IBase;
}

export const accessesSelector: MemoizedSelector<object, RaceAccessViewModel[]> = createSelector(raceAccessSelector, s => s.accesses);
export const loadAccessSelector: MemoizedSelector<object, IBase> = createSelector(raceAccessSelector, s => s.load);
export const addAccessSelector: MemoizedSelector<object, IBase> = createSelector(raceAccessSelector, s => s.add);
export const editAccessSelector: MemoizedSelector<object, IBase> = createSelector(raceAccessSelector, s => s.edit);
export const deleteAccessSelector: MemoizedSelector<object, IBase> = createSelector(raceAccessSelector, s => s.delete);
