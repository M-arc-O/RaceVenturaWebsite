import { ActionReducerMap, combineReducers } from '@ngrx/store';
import { createBaseReducer } from 'src/app/store/base.reducer';
import { PointActions } from '../../actions';
import { IPoints } from '../../points.interface';
import { pointsReducer } from './points.reducer';

export const pointsReducers = combineReducers(<ActionReducerMap<IPoints>>{
    points: pointsReducer,
    add: createBaseReducer(PointActions.ADD_POINT, PointActions.ADD_POINT_SUCCES, PointActions.ADD_POINT_ERROR),
    edit: createBaseReducer(PointActions.EDIT_POINT, PointActions.EDIT_POINT_SUCCES, PointActions.EDIT_POINT_ERROR),
    delete: createBaseReducer(PointActions.DELETE_POINT, PointActions.DELETE_POINT_SUCCES, PointActions.DELETE_POINT_ERROR)
});
