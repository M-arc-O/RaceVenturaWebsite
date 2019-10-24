import { IBase } from 'src/app/store';
import { TeamDetailViewModel } from '../shared/models';
import { teamsStateSelector } from './selectedRace.interface';
import { MemoizedSelector, createSelector } from '@ngrx/store';

export interface ITeams {
    teams: TeamDetailViewModel[];
    add: IBase;
    edit: IBase;
    delete: IBase;
}

export const teamsSelector: MemoizedSelector<object, TeamDetailViewModel[]> = createSelector(teamsStateSelector, s => s.teams);
export const addTeamSelector: MemoizedSelector<object, IBase> = createSelector(teamsStateSelector, s => s.add);
export const editTeamSelector: MemoizedSelector<object, IBase> = createSelector(teamsStateSelector, s => s.edit);
export const deleteTeamSelector: MemoizedSelector<object, IBase> = createSelector(teamsStateSelector, s => s.delete);
