import { TeamViewModel } from '../../shared';
import * as teamsActions from './../actions/team.actions';

export function teamsReducer(state: TeamViewModel[], action: teamsActions.TeamActionsUnion) {
    switch (action.type) {
        case teamsActions.TeamActions.LOAD_TEAMS_SUCCES: {
            return action.payload;
        }
        case teamsActions.TeamActions.ADD_TEAM_SUCCES: {
            state.push(action.payload);
            return Object.assign([], state);
        }
        case teamsActions.TeamActions.DELETE_TEAM_SUCCES: {
            state.splice(state.findIndex(item => item.id === action.payload));
            return Object.assign([], state);
        }
        default: {
            return state;
        }
    }
}
