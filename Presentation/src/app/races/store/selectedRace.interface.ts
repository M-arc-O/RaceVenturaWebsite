import { createSelector, MemoizedSelector } from '@ngrx/store';
import { IStagesState } from 'src/app/stages/store';
import { IBase } from 'src/app/store';
import { RaceStoreModel } from '../shared/models';
import { ITeams } from './teams.interface';
import { selectedRaceStateSelector } from './racesState.interface';

export interface ISelectedRace {
    selectedRace: RaceStoreModel;
    load: IBase;
    edit: IBase;
    stagesState: IStagesState;
    teams: ITeams;
}

export const selectedRaceSelector: MemoizedSelector<object, RaceStoreModel> = createSelector(selectedRaceStateSelector,
    s => s.selectedRace);
export const loadSelectedRaceSelector: MemoizedSelector<object, IBase> = createSelector(selectedRaceStateSelector, s => s.load);
export const editSelectedRaceSelector: MemoizedSelector<object, IBase> = createSelector(selectedRaceStateSelector, s => s.edit);
export const stagesStateSelectedRaceSelector: MemoizedSelector<object, IStagesState> = createSelector(selectedRaceStateSelector,
    s => s.stagesState);
export const teamsStateSelector: MemoizedSelector<object, ITeams> = createSelector(selectedRaceStateSelector, s => s.teams);
