import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { StagesService } from '../../shared';
import * as stagesActions from './../actions';


@Injectable()
export class StageEffects {

    constructor(
        private stagesService: StagesService,
        private actions$: Actions
    ) { }

    @Effect() addStage$ = this.actions$.pipe(
        ofType(stagesActions.StageActions.ADD_STAGE),
        switchMap(action => this.stagesService.addStage((action as stagesActions.AddStageAction).payload).pipe(
            map(stage => new stagesActions.AddStageSuccesAction(stage)),
            catchError((error: HttpErrorResponse) => of(new stagesActions.AddStageErrorAction({ error: error }))))));

    @Effect() editStage$ = this.actions$.pipe(
        ofType(stagesActions.StageActions.EDIT_STAGE),
        switchMap(action => this.stagesService.editStage((action as stagesActions.EditStageAction).payload).pipe(
            map(stage => new stagesActions.EditStageSuccesAction(stage)),
            catchError((error: HttpErrorResponse) => of(new stagesActions.EditStageErrorAction({ error: error }))))));

    @Effect() deleteStage$ = this.actions$.pipe(
        ofType(stagesActions.StageActions.DELETE_STAGE),
        switchMap(action => this.stagesService.deleteStage((action as stagesActions.DeleteStageAction).payload).pipe(
            map(id => new stagesActions.DeleteStageSuccesAction(id)),
            catchError((error: HttpErrorResponse) => of(new stagesActions.DeleteStageErrorAction({ error: error }))))));
}
