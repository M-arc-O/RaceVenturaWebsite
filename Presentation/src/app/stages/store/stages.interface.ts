import { StageViewModel } from '../shared';
import { IBase } from 'src/app/store/base.interface';
import { MemoizedSelector, createSelector } from '@ngrx/store';
import { stagesSelector } from './stagesState.interface';

export interface IStages {
    stages: StageViewModel[];
    load: IBase;
    add: IBase;
    delete: IBase;
}

export const stagesListSelector: MemoizedSelector<object, StageViewModel[]> = createSelector(stagesSelector, s => s.stages);
export const loadStagesSelector: MemoizedSelector<object, IBase> = createSelector(stagesSelector, s => s.load);
export const addStageSelector: MemoizedSelector<object, IBase> = createSelector(stagesSelector, s => s.add);
export const deleteStageSelector: MemoizedSelector<object, IBase> = createSelector(stagesSelector, s => s.delete);
