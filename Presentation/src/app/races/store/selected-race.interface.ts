import { createSelector, MemoizedSelector } from '@ngrx/store';
import { IBase } from 'src/app/store';
import { RaceStoreModel, TeamResultViewModel } from '../shared/models';
import { IPoints } from './points.interface';
import { selectedRaceStateSelector } from './races-state.interface';
import { IStages } from './stages.interface';
import { ITeamPointVisited } from './team-point-visited.interface';
import { ITeams } from './teams.interface';

export interface ISelectedRace {
    selectedRace: RaceStoreModel;
    load: IBase;
    edit: IBase;
    teams: ITeams;
    stages: IStages;
    points: IPoints;
    teamPointVisited: ITeamPointVisited;
    teamResults: TeamResultViewModel[];
}

export const selectedRaceSelector: MemoizedSelector<object, RaceStoreModel> = createSelector(selectedRaceStateSelector,
    s => s.selectedRace);
export const loadSelectedRaceSelector: MemoizedSelector<object, IBase> = createSelector(selectedRaceStateSelector, s => s.load);
export const editSelectedRaceSelector: MemoizedSelector<object, IBase> = createSelector(selectedRaceStateSelector, s => s.edit);
export const teamsStateSelector: MemoizedSelector<object, ITeams> = createSelector(selectedRaceStateSelector, s => s.teams);
export const pointVisitedStateSelector: MemoizedSelector<object, ITeamPointVisited> = createSelector(selectedRaceStateSelector,
    s => s.teamPointVisited);
export const stagesStateSelector: MemoizedSelector<object, IStages> = createSelector(selectedRaceStateSelector, s => s.stages);
export const pointsStateSelector: MemoizedSelector<object, IPoints> = createSelector(selectedRaceStateSelector, s => s.points);
export const resultStateSelector: MemoizedSelector<object, TeamResultViewModel[]> = createSelector(selectedRaceStateSelector,
    s => s.teamResults);
