import { ActionReducerMap, combineReducers } from '@ngrx/store';
import { createBaseReducer } from 'src/app/store';
import { TeamPointVisitedActions } from '../../actions';
import { ITeamPointVisited } from '../../team-point-visited.interface';
import { pointVisitedReducer } from './team-point-visited.reducer';

export const teamPointVisitedReducers = combineReducers(<ActionReducerMap<ITeamPointVisited>>{
    pointsVisited: pointVisitedReducer,
    add: createBaseReducer(TeamPointVisitedActions.ADD_TEAM_POINT_VISITED,
        TeamPointVisitedActions.ADD_TEAM_POINT_VISITED_SUCCES, TeamPointVisitedActions.ADD_TEAM_POINT_VISITED_ERROR),
    delete: createBaseReducer(TeamPointVisitedActions.DELETE_TEAM_POINT_VISITED,
        TeamPointVisitedActions.DELETE_TEAM_POINT_VISITED_SUCCES, TeamPointVisitedActions.DELETE_TEAM_POINT_VISITED_ERROR)
});
