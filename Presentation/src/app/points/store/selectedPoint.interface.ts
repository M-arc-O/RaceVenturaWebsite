import { IBase } from 'src/app/store/base.interface';
import { MemoizedSelector, createSelector } from '@ngrx/store';
import { PointDetailViewModel } from '../shared';
import { selectedPointStateSelector } from './pointsState.interface';

export interface ISelectedPoint {
    selectedPoint: PointDetailViewModel;
    load: IBase;
    edit: IBase;
}

export const selectedPointSelector: MemoizedSelector<object, PointDetailViewModel> = createSelector(selectedPointStateSelector,
    s => s.selectedPoint);
export const loadSelectedPointSelector: MemoizedSelector<object, IBase> = createSelector(selectedPointStateSelector, s => s.load);
export const editSelectedPointSelector: MemoizedSelector<object, IBase> = createSelector(selectedPointStateSelector, s => s.edit);
