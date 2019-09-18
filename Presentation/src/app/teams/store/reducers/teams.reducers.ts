import { combineReducers, ActionReducerMap } from '@ngrx/store';
import { ITeamsState } from '../teamsState.interface';
import { teamsReducer } from './teams.reducer';
import { selectedTeamReducer } from './selectedTeam.reducer';
import { ITeams } from '../teams.interface';
import { ISelectedTeam } from '../selectedTeam.interface';
import { createBaseReducer } from 'src/app/store/base.reducer';
import { TeamActions } from '../actions/team.actions';
import { pointsReducers } from 'src/app/points/store/reducers';

export const teamsCombinedReducers = combineReducers(<ActionReducerMap<ITeams>>{
    teams: teamsReducer,
    load: createBaseReducer(TeamActions.LOAD_TEAMS, TeamActions.LOAD_TEAMS_SUCCES, TeamActions.LOAD_TEAMS_ERROR),
    add: createBaseReducer(TeamActions.ADD_TEAM, TeamActions.ADD_TEAM_SUCCES, TeamActions.ADD_TEAM_ERROR),
    delete: createBaseReducer(TeamActions.DELETE_TEAM, TeamActions.DELETE_TEAM_SUCCES, TeamActions.DELETE_TEAM_ERROR)
});

export const selectedTeamCombinedReducer = combineReducers(<ActionReducerMap<ISelectedTeam>>{
    selectedTeam: selectedTeamReducer,
    load: createBaseReducer(TeamActions.LOAD_TEAM_DETAILS, TeamActions.LOAD_TEAM_DETAILS_SUCCES,
        TeamActions.LOAD_TEAM_DETAILS_ERROR),
    edit: createBaseReducer(TeamActions.EDIT_TEAM, TeamActions.EDIT_TEAM_SUCCES, TeamActions.EDIT_TEAM_ERROR,
        TeamActions.LOAD_TEAMS),
    pointsState: pointsReducers
});

export const teamsReducers = combineReducers(<ActionReducerMap<ITeamsState>>{
    teams: teamsCombinedReducers,
    selectedTeam: selectedTeamCombinedReducer
});
