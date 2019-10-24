import { StageStoreModel } from 'src/app/races/shared/models';
import * as stagesActions from '../../actions';

export function stagesReducer(state: StageStoreModel[] = [],
    action: stagesActions.StageActionsUnion | stagesActions.RaceActionsUnion) {
    switch (action.type) {
        case stagesActions.RaceActions.LOAD_RACE_DETAILS_SUCCES: {
            state = action.payload.stages;
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
        case stagesActions.StageActions.DELETE_STAGE_SUCCES: {
            state.splice(state.findIndex(item => item.stageId === action.payload), 1);
            return Object.assign([], state);
        }
        default: {
            return state;
        }
    }
}
