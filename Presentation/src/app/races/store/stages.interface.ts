import { createSelector, MemoizedSelector } from '@ngrx/store';
import { IBase } from 'src/app/store/base.interface';
import { StageDetailViewModel } from '../shared/models';
import { stagesStateSelector } from './selectedRace.interface';

export interface IStages {
    stages: StageDetailViewModel[];
    add: IBase;
    edit: IBase;
    delete: IBase;
}

export const stagesSelector: MemoizedSelector<object, StageDetailViewModel[]> = createSelector(stagesStateSelector, s => s.stages);
export const addStageSelector: MemoizedSelector<object, IBase> = createSelector(stagesStateSelector, s => s.add);
export const editStageSelector: MemoizedSelector<object, IBase> = createSelector(stagesStateSelector, s => s.edit);
export const deleteStageSelector: MemoizedSelector<object, IBase> = createSelector(stagesStateSelector, s => s.delete);
