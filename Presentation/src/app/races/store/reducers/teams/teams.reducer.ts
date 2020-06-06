import { TeamStoreModel } from 'src/app/races/shared/models';
import * as teamsActions from '../../actions';
import { RaceActions } from '../../actions';

export function teamsReducer(state: TeamStoreModel[] = [], action: teamsActions.TeamActionsUnion | teamsActions.RaceActionsUnion) {
    switch (action.type) {
        case RaceActions.LOAD_RACE_DETAILS_SUCCES: {
            var newState = Object.assign([], state);
            newState = action.payload.teams;
            return newState;
        }
        case teamsActions.TeamActions.ADD_TEAM_SUCCES: {
            var newState = Object.assign([], state);
            newState.push(action.payload);
            return newState;
        }
        case teamsActions.TeamActions.EDIT_TEAM_SUCCES: {
            var newState = Object.assign([], state);
            const index = newState.findIndex(s => s.teamId === action.payload.teamId);
            newState[index] = action.payload;
            return newState;
        }
        case teamsActions.TeamActions.DELETE_TEAM_SUCCES: {
            var newState = Object.assign([], state);
            newState.splice(newState.findIndex(item => item.teamId === action.payload), 1);
            return newState;
        }
        default: {
            return state;
        }
    }
}
