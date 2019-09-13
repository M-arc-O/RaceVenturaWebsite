import { createSelector, MemoizedSelector } from '@ngrx/store';
import { IPoints } from './points.interface';
import { ISelectedPoint } from './selectedPoint.interface';
import { pointsStateSelectedStageSelector } from 'src/app/stages/store';

export interface IPointsState {
    points: IPoints;
    selectedPoint: ISelectedPoint;
}

export const pointsSelector: MemoizedSelector<object, IPoints> = createSelector(pointsStateSelectedStageSelector, s => s.points);
export const selectedPointStateSelector: MemoizedSelector<object, ISelectedPoint> = createSelector(pointsStateSelectedStageSelector,
    s => s.selectedPoint);
