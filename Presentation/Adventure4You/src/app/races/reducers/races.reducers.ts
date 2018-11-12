import { RaceViewModel } from '../shared';
import * as racesActions from './../actions/race.actions';

export function racesReducer(state: RaceViewModel[], action: racesActions.Action) {
    switch (action.type) {
        case racesActions.LOAD_RACES_SUCCES: {
            console.log('Race reducer load races succes');
            return action.payload;
        }
        case racesActions.ADD_RACE_SUCCES: {
            console.log('Race reducer add race succes');
            const race = new RaceViewModel();
            race.id = action.payload.id;
            race.name = action.payload.name;
            state.push(race);
            return Object.assign({}, state);
        }
        default: {
            console.log('Races reducer default');
            return state;
        }
    }
}
