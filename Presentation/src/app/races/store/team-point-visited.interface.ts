import { createSelector, MemoizedSelector } from '@ngrx/store';
import { IBase } from 'src/app/store';
import { TeamPointVisitedViewModel } from '../shared/models';
import { pointVisitedStateSelector } from './selectedRace.interface';

export interface ITeamPointVisited {
    pointsVisited: TeamPointVisitedViewModel[];
    add: IBase;
    delete: IBase;
}

export const pointsVisitedSelector: MemoizedSelector<object, TeamPointVisitedViewModel[]> = createSelector(pointVisitedStateSelector,
    s => s.pointsVisited);
export const addPointVisitedSelector: MemoizedSelector<object, IBase> = createSelector(pointVisitedStateSelector, s => s.add);
export const deletePointVisitedSelector: MemoizedSelector<object, IBase> = createSelector(pointVisitedStateSelector, s => s.delete);
