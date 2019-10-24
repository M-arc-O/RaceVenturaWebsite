import { ActionReducerMap, combineReducers } from '@ngrx/store';
import { ITeams } from '../../teams.interface';
import { teamsReducer } from './teams.reducer';
import { TeamActions } from '../../actions';
import { createBaseReducer } from 'src/app/store';

export const teamsReducers = combineReducers(<ActionReducerMap<ITeams>>{
    teams: teamsReducer,
    add: createBaseReducer(TeamActions.ADD_TEAM, TeamActions.ADD_TEAM_SUCCES, TeamActions.ADD_TEAM_ERROR),
    edit: createBaseReducer(TeamActions.EDIT_TEAM, TeamActions.EDIT_TEAM_SUCCES, TeamActions.EDIT_TEAM_ERROR),
    delete: createBaseReducer(TeamActions.DELETE_TEAM, TeamActions.DELETE_TEAM_SUCCES, TeamActions.DELETE_TEAM_ERROR),
});
