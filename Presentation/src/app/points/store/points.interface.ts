import { createSelector, MemoizedSelector } from '@ngrx/store';
import { IBase } from 'src/app/store/base.interface';
import { pointsSelector } from './pointsState.interface';
import { PointViewModel } from '../shared';

export interface IPoints {
    points: PointViewModel[];
    load: IBase;
    add: IBase;
    delete: IBase;
}

export const pointsListSelector: MemoizedSelector<object, PointViewModel[]> = createSelector(pointsSelector, s => s.points);
export const loadPointsSelector: MemoizedSelector<object, IBase> = createSelector(pointsSelector, s => s.load);
export const addPointSelector: MemoizedSelector<object, IBase> = createSelector(pointsSelector, s => s.add);
export const deletePointSelector: MemoizedSelector<object, IBase> = createSelector(pointsSelector, s => s.delete);
