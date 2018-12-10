import { RaceDetailViewModel } from '../../shared';
import * as racesActions from './../actions/race.actions';

export function selectedRaceReducer(state: RaceDetailViewModel, action: racesActions.RaceActionsUnion) {
    switch (action.type) {
        case racesActions.RaceActions.LOAD_RACE_DETAILS_SUCCES: {
            return action.payload;
        }
        default: {
            return state;
        }
    }
}
