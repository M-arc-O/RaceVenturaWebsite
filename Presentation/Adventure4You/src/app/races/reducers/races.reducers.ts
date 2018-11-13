import { RaceViewModel } from '../shared';
import * as racesActions from './../actions/race.actions';

export function racesReducer(state: RaceViewModel[], action: racesActions.Action) {
    switch (action.type) {
        case racesActions.LOAD_RACES_SUCCES: {
            return action.payload;
        }
        case racesActions.ADD_RACE_SUCCES: {
            state.push(action.payload);
            return Object.assign([], state);
        }
        default: {
            return state;
        }
    }
}
