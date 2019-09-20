import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { PointsService } from '../../shared';
import * as pointsActions from './../actions/point.actions';
import { PointActions } from './../actions/point.actions';


@Injectable()
export class PointsEffects {

    constructor(
        private pointsService: PointsService,
        private actions$: Actions
    ) { }

    @Effect() loadPoints$ = this.actions$.pipe(
        ofType(PointActions.LOAD_POINTS),
        switchMap(action => this.pointsService.getPoints((action as pointsActions.LoadPointsAction).payload).pipe(
            map(points => new pointsActions.LoadPointsSuccesAction(points)),
            catchError((error: HttpErrorResponse) => of(new pointsActions.LoadPointsErrorAction({ error: error }))))));

    @Effect() addPoint$ = this.actions$.pipe(
        ofType(PointActions.ADD_POINT),
        switchMap(action => this.pointsService.addPoint((action as pointsActions.AddPointAction).payload).pipe(
            map(point => new pointsActions.AddPointSuccesAction(point)),
            catchError((error: HttpErrorResponse) => of(new pointsActions.AddPointErrorAction({ error: error }))))));

    @Effect() deletePoint$ = this.actions$.pipe(
        ofType(PointActions.DELETE_POINT),
        switchMap(action => this.pointsService.deletePoint((action as pointsActions.DeletePointAction).payload).pipe(
            map(id => new pointsActions.DeletePointSuccesAction(id)),
            catchError((error: HttpErrorResponse) => of(new pointsActions.DeletePointErrorAction({ error: error }))))));

    @Effect() loadPointDetails$ = this.actions$.pipe(
        ofType(PointActions.LOAD_POINT_DETAILS),
        switchMap(action => this.pointsService.getPointDetails((action as pointsActions.LoadPointDetailsAction).payload).pipe(
            map(pointDetails => new pointsActions.LoadPointDetailsSuccesAction(pointDetails)),
            catchError((error: HttpErrorResponse) => of(new pointsActions.LoadPointDetailsErrorAction({ error: error }))))));

    @Effect() editPoint$ = this.actions$.pipe(
        ofType(PointActions.EDIT_POINT),
        switchMap(action => this.pointsService.editPoint((action as pointsActions.EditPointAction).payload).pipe(
            map(point => new pointsActions.EditPointSuccesAction(point)),
            catchError((error: HttpErrorResponse) => of(new pointsActions.EditPointErrorAction({ error: error }))))));
}
