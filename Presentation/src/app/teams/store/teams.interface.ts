import { createSelector, MemoizedSelector } from '@ngrx/store';
import { IBase } from 'src/app/store/base.interface';
import { TeamViewModel } from '../shared';
import { teamsSelector } from './teamsState.interface';

export interface ITeams {
    teams: TeamViewModel[];
    load: IBase;
    add: IBase;
    delete: IBase;
}

export const teamsListSelector: MemoizedSelector<object, TeamViewModel[]> = createSelector(teamsSelector, t => t.teams);
export const loadTeamsSelector: MemoizedSelector<object, IBase> = createSelector(teamsSelector, t => t.load);
export const addTeamSelector: MemoizedSelector<object, IBase> = createSelector(teamsSelector, t => t.add);
export const deleteTeamSelector: MemoizedSelector<object, IBase> = createSelector(teamsSelector, t => t.delete);
