import { RaceStoreModel } from 'src/app/races/shared/models';
import * as racesActions from '../../actions';

export function selectedRaceReducer(state: RaceStoreModel, action: racesActions.RaceActionsUnion) {
    switch (action.type) {
        case racesActions.RaceActions.LOAD_RACE_DETAILS_SUCCES: {
            return action.payload;
        }
        case racesActions.RaceActions.EDIT_RACE_SUCCES: {
            return action.payload;
        }
        default: {
            return state;
        }
    }
}
