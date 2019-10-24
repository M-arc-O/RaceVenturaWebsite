import { HttpErrorResponse } from '@angular/common/http';
import { BaseErrorAction } from 'src/app/store/BaseErrorAction';
import { StageStoreModel } from '../../shared/models';

export enum StageActions {
    ADD_STAGE = 'ADD_STAGE',
    ADD_STAGE_SUCCES = 'ADD_STAGE_SUCCES',
    ADD_STAGE_ERROR = 'ADD_STAGE_ERROR',
    EDIT_STAGE = 'EDIT_STAGE',
    EDIT_STAGE_SUCCES = 'EDIT_STAGE_SUCCES',
    EDIT_STAGE_ERROR = 'EDIT_STAGE_ERROR',
    DELETE_STAGE = 'DELETE_STAGE',
    DELETE_STAGE_SUCCES = 'DELETE_STAGE_SUCCES',
    DELETE_STAGE_ERROR = 'DELETE_STAGE_ERROR'
}

export class AddStageAction {
    public readonly type = StageActions.ADD_STAGE;
    constructor(public readonly payload: StageStoreModel) { }
}

export class AddStageSuccesAction {
    public readonly type = StageActions.ADD_STAGE_SUCCES;
    constructor(public readonly payload: StageStoreModel) { }
}

export class AddStageErrorAction implements BaseErrorAction {
    public readonly type = StageActions.ADD_STAGE_ERROR;
    constructor(public readonly payload: { error: HttpErrorResponse; }) { }
}

export class EditStageAction {
    public readonly type = StageActions.EDIT_STAGE;
    constructor(public readonly payload: StageStoreModel) { }
}

export class EditStageSuccesAction {
    public readonly type = StageActions.EDIT_STAGE_SUCCES;
    constructor(public readonly payload: StageStoreModel) { }
}

export class EditStageErrorAction implements BaseErrorAction {
    public readonly type = StageActions.EDIT_STAGE_ERROR;
    constructor(public readonly payload: { error: HttpErrorResponse; }) { }
}

export class DeleteStageAction {
    public readonly type = StageActions.DELETE_STAGE;
    constructor(public readonly payload: StageStoreModel) { }
}

export class DeleteStageSuccesAction {
    public readonly type = StageActions.DELETE_STAGE_SUCCES;
    constructor(public readonly payload: string) { }
}

export class DeleteStageErrorAction implements BaseErrorAction {
    public readonly type = StageActions.DELETE_STAGE_ERROR;
    constructor(public readonly payload: { error: HttpErrorResponse; }) { }
}

export type StageActionsUnion
    = AddStageAction
    | AddStageSuccesAction
    | AddStageErrorAction
    | EditStageAction
    | EditStageSuccesAction
    | EditStageErrorAction
    | DeleteStageAction
    | DeleteStageSuccesAction
    | DeleteStageErrorAction;
