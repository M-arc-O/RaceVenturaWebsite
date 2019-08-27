import { createSelector, MemoizedSelector } from '@ngrx/store';
import { IBase } from 'src/app/store/base.interface';
import { RaceDetailViewModel } from '../shared';
import { selectedRaceStateSelector } from './racesState.interface';

export interface ISelectedRace {
    selectedRace: RaceDetailViewModel;
    load: IBase;
    edit: IBase;
}

export const selectedRaceSelector: MemoizedSelector<object, RaceDetailViewModel> = createSelector(selectedRaceStateSelector,
    s => s.selectedRace);
export const loadSelectedRaceSelector: MemoizedSelector<object, IBase> = createSelector(selectedRaceStateSelector, s => s.load);
export const editSelectedRaceSelector: MemoizedSelector<object, IBase> = createSelector(selectedRaceStateSelector, s => s.edit);
