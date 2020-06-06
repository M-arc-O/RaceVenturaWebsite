import { RaceViewModel } from '../../../shared/models';
import * as racesActions from '../../actions';

export function racesReducer(state: RaceViewModel[], action: racesActions.RaceActionsUnion) {
    switch (action.type) {
        case racesActions.RaceActions.LOAD_RACES_SUCCES: {
            return action.payload;
        }
        case racesActions.RaceActions.ADD_RACE_SUCCES: {
            var newState = Object.assign([], state);
            newState.push(action.payload);
            return newState;
        }
        case racesActions.RaceActions.EDIT_RACE_SUCCES: {
            var newState = Object.assign([], state);
            const index = newState.findIndex(s => s.raceId === action.payload.raceId);
            newState[index] = action.payload;
            return newState;
        }
        case racesActions.RaceActions.DELETE_RACE_SUCCES: {
            var newState = Object.assign([], state);
            newState.splice(newState.findIndex(item => item.raceId === action.payload), 1);
            return newState;
        }
        default: {
            return state;
        }
    }
}
