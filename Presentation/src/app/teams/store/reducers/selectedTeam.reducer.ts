import { TeamDetailViewModel } from '../../shared';
import * as teamsActions from './../actions/team.actions';

export function selectedTeamReducer(state: TeamDetailViewModel, action: teamsActions.TeamActionsUnion) {
    switch (action.type) {
        case teamsActions.TeamActions.LOAD_TEAM_DETAILS_SUCCES: {
            return action.payload;
        }
        case teamsActions.TeamActions.EDIT_TEAM_SUCCES: {
            return action.payload;
        }
        default: {
            return state;
        }
    }
}
