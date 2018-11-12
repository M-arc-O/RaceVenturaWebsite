import { RaceDetailViewModel } from '../shared';
import * as racesActions from './../actions/race.actions';

export function SelectedRaceReducer(state: RaceDetailViewModel, action: racesActions.Action) {
    switch (action.type) {
        case racesActions.LOAD_RACE_DETAILS_SUCCES: {
            return action.payload;
        }
        default: {
            return state;
        }
    }
}
