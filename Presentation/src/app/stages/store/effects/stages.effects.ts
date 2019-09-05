import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { StagesService } from '../../shared';
import * as stagesActions from './../actions/stage.actions';
import { StageActions } from './../actions/stage.actions';


@Injectable()
export class StagesEffects {

    constructor(
        private stagesService: StagesService,
        private actions$: Actions
    ) { }

    @Effect() loadRaces$ = this.actions$.pipe(
        ofType(StageActions.LOAD_STAGES),
        switchMap(action => this.stagesService.getStages((action as stagesActions.LoadStagesAction).payload)),
        map(stages => new stagesActions.LoadStagesSuccesAction(stages)),
        catchError((error: Response) => of(new stagesActions.LoadStagesErrorAction({ error: error }))));

    @Effect() addRace$ = this.actions$.pipe(
        ofType(StageActions.ADD_STAGE),
        switchMap(action => this.stagesService.addStage((action as stagesActions.AddStageAction).payload)),
        map(race => new stagesActions.AddStageSuccesAction(race)),
        catchError((error: Response) => of(new stagesActions.AddStageErrorAction({ error: error }))));

    @Effect() deleteRace$ = this.actions$.pipe(
        ofType(StageActions.DELETE_STAGE),
        switchMap(action => this.stagesService.deleteStage((action as stagesActions.DeleteStageAction).payload)),
        map(id => new stagesActions.DeleteStageSuccesAction(id)),
        catchError((error: Response) => of(new stagesActions.DeleteStageErrorAction({ error: error }))));

    @Effect() loadRaceDetails$ = this.actions$.pipe(
        ofType(StageActions.LOAD_STAGE_DETAILS),
        switchMap(action => this.stagesService.getStageDetails((action as stagesActions.LoadStageDetailsAction).payload)),
        map(stageDetails => new stagesActions.LoadStageDetailsSuccesAction(stageDetails)),
        catchError((error: Response) => of(new stagesActions.LoadStageDetailsErrorAction({ error: error }))));

    @Effect() editRace$ = this.actions$.pipe(
        ofType(StageActions.EDIT_STAGE),
        switchMap(action => this.stagesService.editStage((action as stagesActions.EditStageAction).payload)),
        map(stage => new stagesActions.EditStageSuccesAction(stage)),
        catchError((error: Response) => of(new stagesActions.EditStageErrorAction({ error: error }))));
}
