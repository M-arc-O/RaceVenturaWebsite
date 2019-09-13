import { combineReducers, ActionReducerMap } from '@ngrx/store';
import { createBaseReducer } from 'src/app/store/base.reducer';
import { IPoints } from '../points.interface';
import { pointsReducer } from './points.reducer';
import { PointActions } from '../actions/point.actions';
import { ISelectedPoint, IPointsState } from '..';
import { selectedPointReducer } from './selectedPoint.reducer';

export const pointsCombinedReducers = combineReducers(<ActionReducerMap<IPoints>>{
    points: pointsReducer,
    load: createBaseReducer(PointActions.LOAD_POINTS, PointActions.LOAD_POINTS_SUCCES, PointActions.LOAD_POINTS_ERROR),
    add: createBaseReducer(PointActions.ADD_POINT, PointActions.ADD_POINT_SUCCES, PointActions.ADD_POINT_ERROR),
    delete: createBaseReducer(PointActions.DELETE_POINT, PointActions.DELETE_POINT_SUCCES, PointActions.DELETE_POINT_ERROR)
});

export const selectedPointCombinedReducer = combineReducers(<ActionReducerMap<ISelectedPoint>>{
    selectedPoint: selectedPointReducer,
    load: createBaseReducer(PointActions.LOAD_POINT_DETAILS, PointActions.LOAD_POINT_DETAILS_SUCCES,
        PointActions.LOAD_POINT_DETAILS_ERROR),
    edit: createBaseReducer(PointActions.EDIT_POINT, PointActions.EDIT_POINT_SUCCES, PointActions.EDIT_POINT_ERROR,
        PointActions.LOAD_POINTS)
});

export const pointsReducers = combineReducers(<ActionReducerMap<IPointsState>>{
    points: pointsCombinedReducers,
    selectedPoint: selectedPointCombinedReducer
});
