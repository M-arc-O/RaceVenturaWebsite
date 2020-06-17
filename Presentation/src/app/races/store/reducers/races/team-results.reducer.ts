import { TeamResultViewModel } from 'src/app/races/shared/models';
import * as racesActions from '../../actions';

export function teamResultsReducer(state: TeamResultViewModel[], action: racesActions.RaceActionsUnion) {
    switch (action.type) {
        case racesActions.RaceActions.GET_RACE_RESULT_SUCCES: {
            return action.payload;
        }
        default: {
            return state;
        }
    }
}
