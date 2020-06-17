import { StageActions } from '../../actions';
import { combineReducers, ActionReducerMap } from '@ngrx/store';
import { IStages } from '../../stages.interface';
import { createBaseReducer } from 'src/app/store';
import { stagesReducer } from './stages.reducer';

export const stagesReducers = combineReducers(<ActionReducerMap<IStages>>{
    stages: stagesReducer,
    add: createBaseReducer(StageActions.ADD_STAGE, StageActions.ADD_STAGE_SUCCES, StageActions.ADD_STAGE_ERROR),
    edit: createBaseReducer(StageActions.EDIT_STAGE, StageActions.EDIT_STAGE_SUCCES, StageActions.EDIT_STAGE_ERROR),
    delete: createBaseReducer(StageActions.DELETE_STAGE, StageActions.DELETE_STAGE_SUCCES, StageActions.DELETE_STAGE_ERROR)
});
