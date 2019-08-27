import { createSelector, MemoizedSelector } from '@ngrx/store';
import { IStages } from './stages.interface';
import { ISelectedStage } from './selectedStage.interface';
import { stagesStateSelector } from 'src/app/races/store';

export interface IStagesState {
    stages: IStages;
    selectedStage: ISelectedStage;
}

export const stagesSelector: MemoizedSelector<object, IStages> = createSelector(stagesStateSelector, s => s.stages);
export const selectedStageStateSelector: MemoizedSelector<object, ISelectedStage> = createSelector(stagesStateSelector,
    s => s.selectedStage);
