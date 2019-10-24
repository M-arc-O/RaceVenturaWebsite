import { RaceViewModel } from '../../../shared/models';
import * as racesActions from '../../actions';

export function racesReducer(state: RaceViewModel[], action: racesActions.RaceActionsUnion) {
    switch (action.type) {
        case racesActions.RaceActions.LOAD_RACES_SUCCES: {
            return action.payload;
        }
        case racesActions.RaceActions.ADD_RACE_SUCCES: {
            state.push(action.payload);
            return Object.assign([], state);
        }
        case racesActions.RaceActions.EDIT_RACE_SUCCES: {
            const index = state.findIndex(s => s.raceId === action.payload.raceId);
            state[index] = action.payload;
            return Object.assign([], state);
        }
        case racesActions.RaceActions.DELETE_RACE_SUCCES: {
            state.splice(state.findIndex(item => item.raceId === action.payload), 1);
            return Object.assign([], state);
        }
        default: {
            return state;
        }
    }
}
