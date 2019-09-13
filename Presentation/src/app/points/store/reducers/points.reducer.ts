import * as pointsActions from './../actions/point.actions';
import { PointViewModel } from '../../shared';

export function pointsReducer(state: PointViewModel[], action: pointsActions.PointActionsUnion) {
    switch (action.type) {
        case pointsActions.PointActions.LOAD_POINTS_SUCCES: {
            return action.payload;
        }
        case pointsActions.PointActions.ADD_POINT_SUCCES: {
            state.push(action.payload);
            return Object.assign([], state);
        }
        case pointsActions.PointActions.DELETE_POINT_SUCCES: {
            state.splice(state.findIndex(item => item.id === action.payload));
            return Object.assign([], state);
        }
        default: {
            return state;
        }
    }
}
