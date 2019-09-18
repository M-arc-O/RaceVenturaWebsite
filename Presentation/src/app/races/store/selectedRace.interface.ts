import { createSelector, MemoizedSelector } from '@ngrx/store';
import { IBase } from 'src/app/store/base.interface';
import { RaceDetailViewModel } from '../shared';
import { selectedRaceStateSelector } from './racesState.interface';
import { IStagesState } from 'src/app/stages/store';
import { ITeamsState } from 'src/app/teams/store';

export interface ISelectedRace {
    selectedRace: RaceDetailViewModel;
    load: IBase;
    edit: IBase;
    stagesState: IStagesState;
    teamsState: ITeamsState;
}

export const selectedRaceSelector: MemoizedSelector<object, RaceDetailViewModel> = createSelector(selectedRaceStateSelector,
    s => s.selectedRace);
export const loadSelectedRaceSelector: MemoizedSelector<object, IBase> = createSelector(selectedRaceStateSelector, s => s.load);
export const editSelectedRaceSelector: MemoizedSelector<object, IBase> = createSelector(selectedRaceStateSelector, s => s.edit);
export const stagesStateSelectedRaceSelector: MemoizedSelector<object, IStagesState> = createSelector(selectedRaceStateSelector,
    s => s.stagesState);
export const teamsStateSelectedRaceSelector: MemoizedSelector<object, ITeamsState> = createSelector(selectedRaceStateSelector,
    s => s.teamsState);
