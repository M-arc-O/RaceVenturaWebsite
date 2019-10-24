import { HttpErrorResponse } from '@angular/common/http';
import { BaseErrorAction } from 'src/app/store/BaseErrorAction';
import { PointDetailViewModel } from '../../shared/models';

export enum PointActions {
    ADD_POINT = 'ADD_POINT',
    ADD_POINT_SUCCES = 'ADD_POINT_SUCCES',
    ADD_POINT_ERROR = 'ADD_POINT_ERROR',
    EDIT_POINT = 'EDIT_POINT',
    EDIT_POINT_SUCCES = 'EDIT_POINT_SUCCES',
    EDIT_POINT_ERROR = 'EDIT_POINT_ERROR',
    DELETE_POINT = 'DELETE_POINT',
    DELETE_POINT_SUCCES = 'DELETE_POINT_SUCCES',
    DELETE_POINT_ERROR = 'DELETE_POINT_ERROR'
}

export class AddPointAction {
    public readonly type = PointActions.ADD_POINT;
    constructor(public readonly payload: PointDetailViewModel) { }
}

export class AddPointSuccesAction {
    public readonly type = PointActions.ADD_POINT_SUCCES;
    constructor(public readonly payload: PointDetailViewModel) { }
}

export class AddPointErrorAction implements BaseErrorAction {
    public readonly type = PointActions.ADD_POINT_ERROR;
    constructor(public readonly payload: { error: HttpErrorResponse; }) { }
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
    constructor(public readonly payload: { error: HttpErrorResponse; }) { }
}

export class DeletePointAction {
    public readonly type = PointActions.DELETE_POINT;
    constructor(public readonly payload: PointDetailViewModel) { }
}

export class DeletePointSuccesAction {
    public readonly type = PointActions.DELETE_POINT_SUCCES;
    constructor(public readonly payload: string) { }
}

export class DeletePointErrorAction implements BaseErrorAction {
    public readonly type = PointActions.DELETE_POINT_ERROR;
    constructor(public readonly payload: { error: HttpErrorResponse; }) { }
}

export type PointActionsUnion
    = AddPointAction
    | AddPointSuccesAction
    | AddPointErrorAction
    | EditPointAction
    | EditPointSuccesAction
    | EditPointErrorAction
    | DeletePointAction
    | DeletePointSuccesAction
    | DeletePointErrorAction;
