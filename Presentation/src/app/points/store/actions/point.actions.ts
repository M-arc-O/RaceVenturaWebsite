import { BaseErrorAction } from 'src/app/store/BaseErrorAction';
import { PointDetailViewModel, PointViewModel, PointRequest } from '../../shared';

export enum PointActions {
    LOAD_POINTS = 'LOAD_POINTS',
    LOAD_POINTS_SUCCES = 'LOAD_POINTS_SUCCES',
    LOAD_POINTS_ERROR = 'LOAD_POINTS_ERROR',
    ADD_POINT = 'ADD_POINT',
    ADD_POINT_SUCCES = 'ADD_POINT_SUCCES',
    ADD_POINT_ERROR = 'ADD_POINT_ERROR',
    DELETE_POINT = 'DELETE_POINT',
    DELETE_POINT_SUCCES = 'DELETE_POINT_SUCCES',
    DELETE_POINT_ERROR = 'DELETE_POINT_ERROR',
    LOAD_POINT_DETAILS = 'LOAD_POINT_DETAILS',
    LOAD_POINT_DETAILS_SUCCES = 'LOAD_POINT_DETAILS_SUCCES',
    LOAD_POINT_DETAILS_ERROR = 'LOAD_POINT_DETAILS_ERROR',
    EDIT_POINT = 'EDIT_POINT',
    EDIT_POINT_SUCCES = 'EDIT_POINT_SUCCES',
    EDIT_POINT_ERROR = 'EDIT_POINT_ERROR',
}

export class LoadPointsAction {
    public readonly type = PointActions.LOAD_POINTS;
    constructor(public readonly payload: string) { }
}

export class LoadPointsSuccesAction {
    public readonly type = PointActions.LOAD_POINTS_SUCCES;
    constructor(public readonly payload: PointViewModel[]) { }
}

export class LoadPointsErrorAction implements BaseErrorAction {
    public readonly type = PointActions.LOAD_POINTS_ERROR;
    constructor(public readonly payload: { error: Response; }) { }
}

export class AddPointAction {
    public readonly type = PointActions.ADD_POINT;
    constructor(public readonly payload: PointDetailViewModel) { }
}

export class AddPointSuccesAction {
    public readonly type = PointActions.ADD_POINT_SUCCES;
    constructor(public readonly payload: PointViewModel) { }
}

export class AddPointErrorAction implements BaseErrorAction {
    public readonly type = PointActions.ADD_POINT_ERROR;
    constructor(public readonly payload: { error: Response; }) { }
}

export class DeletePointAction {
    public readonly type = PointActions.DELETE_POINT;
    constructor(public readonly payload: PointViewModel) { }
}

export class DeletePointSuccesAction {
    public readonly type = PointActions.DELETE_POINT_SUCCES;
    constructor(public readonly payload: string) { }
}

export class DeletePointErrorAction implements BaseErrorAction {
    public readonly type = PointActions.DELETE_POINT_ERROR;
    constructor(public readonly payload: { error: Response; }) { }
}

export class LoadPointDetailsAction {
    public readonly type = PointActions.LOAD_POINT_DETAILS;
    constructor(public readonly payload: PointRequest) { }
}

export class LoadPointDetailsSuccesAction {
    public readonly type = PointActions.LOAD_POINT_DETAILS_SUCCES;
    constructor(public readonly payload: PointDetailViewModel) { }
}

export class LoadPointDetailsErrorAction implements BaseErrorAction {
    public readonly type = PointActions.LOAD_POINT_DETAILS_ERROR;
    constructor(public readonly payload: { error: Response; }) { }
}

export class EditPointAction {
    public readonly type = PointActions.EDIT_POINT;
    constructor(public readonly payload: PointDetailViewModel) { }
}

export class EditPointSuccesAction {
    public readonly type = PointActions.EDIT_POINT_SUCCES;
    constructor(public readonly payload: PointDetailViewModel) { }
}

export class EditPointErrorAction implements BaseErrorAction {
    public readonly type = PointActions.EDIT_POINT_ERROR;
    constructor(public readonly payload: { error: Response; }) { }
}

export type PointActionsUnion
    = LoadPointsAction
    | LoadPointsSuccesAction
    | LoadPointsErrorAction
    | AddPointAction
    | AddPointSuccesAction
    | AddPointErrorAction
    | DeletePointAction
    | DeletePointSuccesAction
    | DeletePointErrorAction
    | LoadPointDetailsAction
    | LoadPointDetailsSuccesAction
    | LoadPointDetailsErrorAction
    | EditPointAction
    | EditPointSuccesAction
    | EditPointErrorAction;
