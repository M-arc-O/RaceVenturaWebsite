import { createSelector, MemoizedSelector } from '@ngrx/store';
import { teamsStateSelectedRaceSelector } from 'src/app/races/store';
import { ISelectedTeam } from './selectedTeam.interface';
import { ITeams } from './teams.interface';

export interface ITeamsState {
    teams: ITeams;
    selectedTeam: ISelectedTeam;
}

export const teamsSelector: MemoizedSelector<object, ITeams> = createSelector(teamsStateSelectedRaceSelector, t => t.teams);
export const selectedTeamStateSelector: MemoizedSelector<object, ISelectedTeam> = createSelector(teamsStateSelectedRaceSelector,
    t => t.selectedTeam);
