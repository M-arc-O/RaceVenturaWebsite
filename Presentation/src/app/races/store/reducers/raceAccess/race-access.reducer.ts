import { RaceAccessViewModel } from '../../../shared/models';
import * as raceAccessActions from '../../actions';

export function raceAccessReducer(state: RaceAccessViewModel[], action: raceAccessActions.RaceAccessActionsUnion) {
    switch (action.type) {
        case raceAccessActions.RaceAccessActions.LOAD_RACE_ACCESS_SUCCES: {
            return action.payload;
        }
        case raceAccessActions.RaceAccessActions.ADD_RACE_ACCESS_SUCCES: {
            var newState = Object.assign([], state);
            newState.push(action.payload);
            return newState;
        }
        case raceAccessActions.RaceAccessActions.EDIT_RACE_ACCESS_SUCCES: {
            var newState = Object.assign([], state);
            const index = newState.findIndex(s => s.raceId === action.payload.raceId);
            newState[index] = action.payload;
            return newState;
        }
        case raceAccessActions.RaceAccessActions.DELETE_RACE_ACCESS_SUCCES: {
            var newState = Object.assign([], state);
            newState.splice(newState.findIndex(item => item.userEmail === action.payload), 1);
            return newState;
        }
        default: {
            return state;
        }
    }
}