import { IBase } from 'src/app/store';
import { TeamStoreModel } from '../shared/models';
import { teamsStateSelector } from './selected-race.interface';
import { MemoizedSelector, createSelector } from '@ngrx/store';

export interface ITeams {
    teams: TeamStoreModel[];
    add: IBase;
    edit: IBase;
    delete: IBase;
}

export const teamsSelector: MemoizedSelector<object, TeamStoreModel[]> = createSelector(teamsStateSelector, s => s.teams);
export const addTeamSelector: MemoizedSelector<object, IBase> = createSelector(teamsStateSelector, s => s.add);
export const editTeamSelector: MemoizedSelector<object, IBase> = createSelector(teamsStateSelector, s => s.edit);
export const deleteTeamSelector: MemoizedSelector<object, IBase> = createSelector(teamsStateSelector, s => s.delete);
