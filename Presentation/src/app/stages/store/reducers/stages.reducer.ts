import { StageViewModel } from '../../shared';
import * as stagesActions from './../actions/stage.actions';

export function stagesReducer(state: StageViewModel[] = [], action: stagesActions.StageActionsUnion) {
    switch (action.type) {
        case stagesActions.StageActions.LOAD_STAGES_SUCCES: {
            return action.payload;
        }
        case stagesActions.StageActions.DELETE_STAGE_SUCCES: {
            state.splice(state.findIndex(item => item.stageId === action.payload), 1);
            return Object.assign([], state);
        }
        case stagesActions.StageActions.ADD_STAGE_SUCCES: {
            state.push(action.payload);
            return Object.assign([], state);
        }
        case stagesActions.StageActions.EDIT_STAGE_SUCCES: {
            const index = state.findIndex(s => s.stageId === action.payload.stageId);
            state[index] = action.payload;
            return Object.assign([], state);
        }
        default: {
            return state;
        }
    }
}
