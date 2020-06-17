import { PointDetailViewModel } from 'src/app/races/shared/models';
import * as pointsActions from '../../actions';

export function pointsReducer(state: PointDetailViewModel[] = [],
    action: pointsActions.PointActionsUnion | pointsActions.RaceActionsUnion) {
    switch (action.type) {
        case pointsActions.RaceActions.LOAD_RACE_DETAILS_SUCCES: {
            state = new Array();
            action.payload.stages.forEach(stage => {
                state = state.concat(stage.points);
            });
            return Object.assign([], state);
        }
        case pointsActions.PointActions.ADD_POINT_SUCCES: {
            var newState = Object.assign([], state);
            newState.push(action.payload);
            return newState;
        }
        case pointsActions.PointActions.EDIT_POINT_SUCCES: {
            var newState = Object.assign([], state);
            const index = newState.findIndex(s => s.pointId === action.payload.pointId);
            newState[index] = action.payload;
            return newState;
        }
        case pointsActions.PointActions.DELETE_POINT_SUCCES: {
            var newState = Object.assign([], state);
            newState.splice(newState.findIndex(item => item.pointId === action.payload), 1);
            return newState;
        }
        default: {
            return state;
        }
    }
}
