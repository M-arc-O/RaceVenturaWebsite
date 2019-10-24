import { createSelector, MemoizedSelector } from '@ngrx/store';
import { IBase } from 'src/app/store/base.interface';
import { PointDetailViewModel } from '../shared/models';
import { pointsStateSelector } from './selectedRace.interface';

export interface IPoints {
    points: PointDetailViewModel[];
    add: IBase;
    edit: IBase;
    delete: IBase;
}

export const pointsSelector: MemoizedSelector<object, PointDetailViewModel[]> = createSelector(pointsStateSelector, s => s.points);
export const addPointSelector: MemoizedSelector<object, IBase> = createSelector(pointsStateSelector, s => s.add);
export const editPointSelector: MemoizedSelector<object, IBase> = createSelector(pointsStateSelector, s => s.edit);
export const deletePointSelector: MemoizedSelector<object, IBase> = createSelector(pointsStateSelector, s => s.delete);
