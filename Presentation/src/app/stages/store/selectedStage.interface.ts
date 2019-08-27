import { createSelector, MemoizedSelector } from '@ngrx/store';
import { IBase } from 'src/app/store/base.interface';
import { StageDetailViewModel } from '../shared';
import { selectedStageStateSelector } from './stagesState.interface';

export interface ISelectedStage {
    selectedStage: StageDetailViewModel;
    load: IBase;
    edit: IBase;
}

export const selectedStageSelector: MemoizedSelector<object, StageDetailViewModel> = createSelector(selectedStageStateSelector,
    s => s.selectedStage);
export const loadSelectedStageSelector: MemoizedSelector<object, IBase> = createSelector(selectedStageStateSelector, s => s.load);
export const editSelectedStageSelector: MemoizedSelector<object, IBase> = createSelector(selectedStageStateSelector, s => s.edit);
