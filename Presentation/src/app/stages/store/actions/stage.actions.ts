import { BaseErrorAction } from 'src/app/store/BaseErrorAction';
import { StageDetailViewModel, StageRequest, StageViewModel } from '../../shared';

export enum StageActions {
    LOAD_STAGES = 'LOAD_STAGES',
    LOAD_STAGES_SUCCES = 'LOAD_STAGES_SUCCES',
    LOAD_STAGES_ERROR = 'LOAD_STAGES_ERROR',
    ADD_STAGE = 'ADD_STAGE',
    ADD_STAGE_SUCCES = 'ADD_STAGE_SUCCES',
    ADD_STAGE_ERROR = 'ADD_STAGE_ERROR',
    DELETE_STAGE = 'DELETE_STAGE',
    DELETE_STAGE_SUCCES = 'DELETE_STAGE_SUCCES',
    DELETE_STAGE_ERROR = 'DELETE_STAGE_ERROR',
    LOAD_STAGE_DETAILS = 'LOAD_STAGE_DETAILS',
    LOAD_STAGE_DETAILS_SUCCES = 'LOAD_STAGE_DETAILS_SUCCES',
    LOAD_STAGE_DETAILS_ERROR = 'LOAD_STAGE_DETAILS_ERROR',
    EDIT_STAGE = 'EDIT_STAGE',
    EDIT_STAGE_SUCCES = 'EDIT_STAGE_SUCCES',
    EDIT_STAGE_ERROR = 'EDIT_STAGE_ERROR',
}

export class LoadStagesAction {
    public readonly type = StageActions.LOAD_STAGES;
    constructor(public readonly payload: string) { }
}

export class LoadStagesSuccesAction {
    public readonly type = StageActions.LOAD_STAGES_SUCCES;
    constructor(public readonly payload: StageViewModel[]) { }
}

export class LoadStagesErrorAction implements BaseErrorAction {
    public readonly type = StageActions.LOAD_STAGES_ERROR;
    constructor(public readonly payload: { error: Response; }) { }
}

export class AddStageAction {
    public readonly type = StageActions.ADD_STAGE;
    constructor(public readonly payload: StageDetailViewModel) { }
}

export class AddStageSuccesAction {
    public readonly type = StageActions.ADD_STAGE_SUCCES;
    constructor(public readonly payload: StageViewModel) { }
}

export class AddStageErrorAction implements BaseErrorAction {
    public readonly type = StageActions.ADD_STAGE_ERROR;
    constructor(public readonly payload: { error: Response; }) { }
}

export class DeleteStageAction {
    public readonly type = StageActions.DELETE_STAGE;
    constructor(public readonly payload: StageViewModel) { }
}

export class DeleteStageSuccesAction {
    public readonly type = StageActions.DELETE_STAGE_SUCCES;
    constructor(public readonly payload: string) { }
}

export class DeleteStageErrorAction implements BaseErrorAction {
    public readonly type = StageActions.DELETE_STAGE_ERROR;
    constructor(public readonly payload: { error: Response; }) { }
}

export class LoadStageDetailsAction {
    public readonly type = StageActions.LOAD_STAGE_DETAILS;
    constructor(public readonly payload: StageRequest) { }
}

export class LoadStageDetailsSuccesAction {
    public readonly type = StageActions.LOAD_STAGE_DETAILS_SUCCES;
    constructor(public readonly payload: StageDetailViewModel) { }
}

export class LoadStageDetailsErrorAction implements BaseErrorAction {
    public readonly type = StageActions.LOAD_STAGE_DETAILS_ERROR;
    constructor(public readonly payload: { error: Response; }) { }
}

export class EditStageAction {
    public readonly type = StageActions.EDIT_STAGE;
    constructor(public readonly payload: StageDetailViewModel) { }
}

export class EditStageSuccesAction {
    public readonly type = StageActions.EDIT_STAGE_SUCCES;
    constructor(public readonly payload: StageDetailViewModel) { }
}

export class EditStageErrorAction implements BaseErrorAction {
    public readonly type = StageActions.EDIT_STAGE_ERROR;
    constructor(public readonly payload: { error: Response; }) { }
}

export type StageActionsUnion
    = LoadStagesAction
    | LoadStagesSuccesAction
    | LoadStagesErrorAction
    | AddStageAction
    | AddStageSuccesAction
    | AddStageErrorAction
    | DeleteStageAction
    | DeleteStageSuccesAction
    | DeleteStageErrorAction
    | LoadStageDetailsAction
    | LoadStageDetailsSuccesAction
    | LoadStageDetailsErrorAction
    | EditStageAction
    | EditStageSuccesAction
    | EditStageErrorAction;
