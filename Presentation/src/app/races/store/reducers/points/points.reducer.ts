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
            state.push(action.payload);
            return Object.assign([], state);
        }
        case pointsActions.PointActions.EDIT_POINT_SUCCES: {
            const index = state.findIndex(s => s.pointId === action.payload.pointId);
            state[index] = action.payload;
            return Object.assign([], state);
        }
        case pointsActions.PointActions.DELETE_POINT_SUCCES: {
            state.splice(state.findIndex(item => item.pointId === action.payload), 1);
            return Object.assign([], state);
        }
        default: {
            return state;
        }
    }
}
