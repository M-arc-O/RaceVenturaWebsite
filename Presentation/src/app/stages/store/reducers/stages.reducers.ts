import { combineReducers, ActionReducerMap } from '@ngrx/store';
import { IStagesState } from '../stagesState.interface';
import { stagesReducer } from './stages.reducer';
import { selectedStageReducer } from './selectedStage.reducer';
import { IStages } from '../stages.interface';
import { ISelectedStage } from '../selectedStage.interface';
import { createBaseReducer } from 'src/app/store/base.reducer';
import { StageActions } from '../actions/stage.actions';
import { pointsReducers } from 'src/app/points/store/reducers';

export const stagesCombinedReducers = combineReducers(<ActionReducerMap<IStages>>{
    stages: stagesReducer,
    load: createBaseReducer(StageActions.LOAD_STAGES, StageActions.LOAD_STAGES_SUCCES, StageActions.LOAD_STAGES_ERROR),
    add: createBaseReducer(StageActions.ADD_STAGE, StageActions.ADD_STAGE_SUCCES, StageActions.ADD_STAGE_ERROR),
    delete: createBaseReducer(StageActions.DELETE_STAGE, StageActions.DELETE_STAGE_SUCCES, StageActions.DELETE_STAGE_ERROR)
});

export const selectedStageCombinedReducer = combineReducers(<ActionReducerMap<ISelectedStage>>{
    selectedStage: selectedStageReducer,
    load: createBaseReducer(StageActions.LOAD_STAGE_DETAILS, StageActions.LOAD_STAGE_DETAILS_SUCCES,
        StageActions.LOAD_STAGE_DETAILS_ERROR),
    edit: createBaseReducer(StageActions.EDIT_STAGE, StageActions.EDIT_STAGE_SUCCES, StageActions.EDIT_STAGE_ERROR,
        StageActions.LOAD_STAGES),
    pointsState: pointsReducers
});

export const stagesReducers = combineReducers(<ActionReducerMap<IStagesState>>{
    stages: stagesCombinedReducers,
    selectedStage: selectedStageCombinedReducer
});
