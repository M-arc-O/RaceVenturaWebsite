import { createSelector, MemoizedSelector } from '@ngrx/store';
import { IBase } from 'src/app/store';
import { RaceStoreModel } from '../shared/models';
import { selectedRaceStateSelector } from './racesState.interface';
import { IStages } from './stages.interface';
import { ITeams } from './teams.interface';
import { IPoints } from './points.interface';

export interface ISelectedRace {
    selectedRace: RaceStoreModel;
    load: IBase;
    edit: IBase;
    teams: ITeams;
    stages: IStages;
    points: IPoints;
}

export const selectedRaceSelector: MemoizedSelector<object, RaceStoreModel> = createSelector(selectedRaceStateSelector,
    s => s.selectedRace);
export const loadSelectedRaceSelector: MemoizedSelector<object, IBase> = createSelector(selectedRaceStateSelector, s => s.load);
export const editSelectedRaceSelector: MemoizedSelector<object, IBase> = createSelector(selectedRaceStateSelector, s => s.edit);
export const teamsStateSelector: MemoizedSelector<object, ITeams> = createSelector(selectedRaceStateSelector, s => s.teams);
export const stagesStateSelector: MemoizedSelector<object, IStages> = createSelector(selectedRaceStateSelector, s => s.stages);
export const pointsStateSelector: MemoizedSelector<object, IPoints> = createSelector(selectedRaceStateSelector, s => s.points);
