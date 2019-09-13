import { createSelector, MemoizedSelector } from '@ngrx/store';
import { stagesStateSelectedRaceSelector } from 'src/app/races/store';
import { ISelectedStage } from './selectedStage.interface';
import { IStages } from './stages.interface';

export interface IStagesState {
    stages: IStages;
    selectedStage: ISelectedStage;
}

export const stagesSelector: MemoizedSelector<object, IStages> = createSelector(stagesStateSelectedRaceSelector, s => s.stages);
export const selectedStageStateSelector: MemoizedSelector<object, ISelectedStage> = createSelector(stagesStateSelectedRaceSelector,
    s => s.selectedStage);
