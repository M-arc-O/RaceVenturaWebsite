import { createSelector, MemoizedSelector } from '@ngrx/store';
import { IPointsState } from 'src/app/points/store';
import { IBase } from 'src/app/store/base.interface';
import { StageDetailViewModel } from '../shared';
import { selectedStageStateSelector } from './stagesState.interface';

export interface ISelectedStage {
    selectedStage: StageDetailViewModel;
    load: IBase;
    edit: IBase;
    pointsState: IPointsState;
}

export const selectedStageSelector: MemoizedSelector<object, StageDetailViewModel> = createSelector(selectedStageStateSelector,
    s => s.selectedStage);
export const loadSelectedStageSelector: MemoizedSelector<object, IBase> = createSelector(selectedStageStateSelector, s => s.load);
export const editSelectedStageSelector: MemoizedSelector<object, IBase> = createSelector(selectedStageStateSelector, s => s.edit);
export const pointsStateSelectedStageSelector: MemoizedSelector<object, IPointsState> = createSelector(selectedStageStateSelector,
    s => s.pointsState);
