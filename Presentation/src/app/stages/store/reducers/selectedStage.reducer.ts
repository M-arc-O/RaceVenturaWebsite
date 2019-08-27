import { StageDetailViewModel } from '../../shared';
import * as stagesActions from './../actions/stage.actions';

export function selectedStageReducer(state: StageDetailViewModel, action: stagesActions.StageActionsUnion) {
    switch (action.type) {
        case stagesActions.StageActions.LOAD_STAGE_DETAILS_SUCCES: {
            return action.payload;
        }
        case stagesActions.StageActions.EDIT_STAGE_SUCCES: {
            return action.payload;
        }
        default: {
            return state;
        }
    }
}
