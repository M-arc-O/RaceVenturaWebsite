import { createSelector, MemoizedSelector } from '@ngrx/store';
import { IBase } from 'src/app/store/base.interface';
import { TeamDetailViewModel } from '../shared';
import { selectedTeamStateSelector } from './teamsState.interface';

export interface ISelectedTeam {
    selectedTeam: TeamDetailViewModel;
    load: IBase;
    edit: IBase;
}

export const selectedTeamSelector: MemoizedSelector<object, TeamDetailViewModel> = createSelector(selectedTeamStateSelector,
    t => t.selectedTeam);
export const loadSelectedTeamSelector: MemoizedSelector<object, IBase> = createSelector(selectedTeamStateSelector, t => t.load);
export const editSelectedTeamSelector: MemoizedSelector<object, IBase> = createSelector(selectedTeamStateSelector, t => t.edit);
