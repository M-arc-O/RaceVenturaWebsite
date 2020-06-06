import { TeamPointVisitedViewModel } from 'src/app/races/shared/models';
import * as teamsActions from '../../actions';
import { RaceActions } from '../../actions';

export function pointVisitedReducer(state: TeamPointVisitedViewModel[] = [], action: teamsActions.TeamPointVisitedActionsUnion |
    teamsActions.RaceActionsUnion) {
    switch (action.type) {
        case RaceActions.LOAD_RACE_DETAILS_SUCCES: {
            state = new Array();
            action.payload.teams.forEach(team => {
                state = state.concat(team.pointsVisited);
            });
            return Object.assign([], state);
        }
        case teamsActions.TeamPointVisitedActions.ADD_TEAM_POINT_VISITED_SUCCES: {
            var newState = Object.assign([], state);
            newState.push(action.payload);
            return newState;
        }
        case teamsActions.TeamPointVisitedActions.DELETE_TEAM_POINT_VISITED_SUCCES: {
            var newState = Object.assign([], state);
            newState.splice(newState.findIndex(item => item.teamPointVisitedId === action.payload), 1);
            return newState;
        }
        default: {
            return state;
        }
    }
}
