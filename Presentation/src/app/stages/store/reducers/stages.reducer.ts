import { StageViewModel } from '../../shared';
import * as stagesActions from './../actions/stage.actions';

export function stagesReducer(state: StageViewModel[], action: stagesActions.StageActionsUnion) {
    switch (action.type) {
        case stagesActions.StageActions.LOAD_STAGES_SUCCES: {
            return action.payload;
        }
        case stagesActions.StageActions.ADD_STAGE_SUCCES: {
            state.push(action.payload);
            return Object.assign([], state);
        }
        case stagesActions.StageActions.DELETE_STAGE_SUCCES: {
            state.splice(state.findIndex(item => item.id === action.payload));
            return Object.assign([], state);
        }
        default: {
            return state;
        }
    }
}
