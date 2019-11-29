import { createSelector, MemoizedSelector } from '@ngrx/store';
import { IBase } from 'src/app/store/base.interface';
import { StageStoreModel } from '../shared/models';
import { stagesStateSelector } from './selected-race.interface';

export interface IStages {
    stages: StageStoreModel[];
    add: IBase;
    edit: IBase;
    delete: IBase;
}

export const stagesSelector: MemoizedSelector<object, StageStoreModel[]> = createSelector(stagesStateSelector, s => s.stages);
export const addStageSelector: MemoizedSelector<object, IBase> = createSelector(stagesStateSelector, s => s.add);
export const editStageSelector: MemoizedSelector<object, IBase> = createSelector(stagesStateSelector, s => s.edit);
export const deleteStageSelector: MemoizedSelector<object, IBase> = createSelector(stagesStateSelector, s => s.delete);
