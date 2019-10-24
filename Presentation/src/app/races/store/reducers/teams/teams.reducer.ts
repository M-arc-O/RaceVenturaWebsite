import { TeamDetailViewModel } from 'src/app/races/shared/models';
import * as teamsActions from '../../actions';
import { RaceActions } from '../../actions';

export function teamsReducer(state: TeamDetailViewModel[], action: teamsActions.TeamActionsUnion | teamsActions.RaceActionsUnion) {
    switch (action.type) {
        case RaceActions.LOAD_RACE_DETAILS_SUCCES: {
            state = action.payload.teams;
            return Object.assign([], state);
        }
        case teamsActions.TeamActions.ADD_TEAM_SUCCES: {
            state.push(action.payload);
            return Object.assign([], state);
        }
        case teamsActions.TeamActions.EDIT_TEAM_SUCCES: {
            const index = state.findIndex(s => s.raceId === action.payload.raceId);
            state[index] = action.payload;
            return Object.assign([], state);
        }
        case teamsActions.TeamActions.DELETE_TEAM_SUCCES: {
            state.splice(state.findIndex(item => item.raceId === action.payload), 1);
            return Object.assign([], state);
        }
        default: {
            return state;
        }
    }
}
