import { createSelector, MemoizedSelector, createFeatureSelector } from '@ngrx/store';
import { IStages } from './stages.interface';
import { ISelectedStage } from './selectedStage.interface';
import { stagesStateSelectedRaceSelector } from 'src/app/races/store';

export interface IStagesState {
    stages: IStages;
    selectedStage: ISelectedStage;
}

export const stagesSelector: MemoizedSelector<object, IStages> = createSelector(stagesStateSelectedRaceSelector, s => s.stages);
export const selectedStageStateSelector: MemoizedSelector<object, ISelectedStage> = createSelector(stagesStateSelectedRaceSelector,
    s => s.selectedStage);
