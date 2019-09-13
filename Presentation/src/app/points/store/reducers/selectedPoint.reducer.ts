import { PointDetailViewModel } from '../../shared';
import * as pointsActions from './../actions/point.actions';

export function selectedPointReducer(state: PointDetailViewModel, action: pointsActions.PointActionsUnion) {
    switch (action.type) {
        case pointsActions.PointActions.LOAD_POINT_DETAILS_SUCCES: {
            return action.payload;
        }
        case pointsActions.PointActions.EDIT_POINT_SUCCES: {
            return action.payload;
        }
        default: {
            return state;
        }
    }
}
