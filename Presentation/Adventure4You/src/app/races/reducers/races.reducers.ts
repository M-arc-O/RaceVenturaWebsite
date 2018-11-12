import { RaceViewModel } from '../shared';
import * as racesActions from './../actions/race.actions';

export function racesReducer(state: RaceViewModel[], action: racesActions.Action) {
    switch (action.type) {
        case racesActions.LOAD_RACES_SUCCES: {
            return action.payload;
        }
        case racesActions.ADD_RACE_SUCCES: {
            const race = new RaceViewModel();
            race.id = action.payload.id;
            race.name = action.payload.name;
            state.push(race);
            return Object.assign({}, state);
        }
        default: {
            return state;
        }
    }
}
