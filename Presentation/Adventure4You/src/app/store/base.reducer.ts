import { IBase } from './base.interface';
import { Action } from '@ngrx/store';
import { BaseErrorAction } from './BaseErrorAction';

function getInitialState(): IBase {
  return {
    isActive: false,
    success: false,
    error: undefined
  };
}

export const createBaseReducer = (requestAction: string, successAction: string, errorAction: string,
  ...resetActions: string[]) => {

  const initialState = getInitialState();

  return (state: IBase = initialState, action: Action): IBase => {
    switch (action.type) {
      case requestAction:
        return {
          isActive: true,
          success: false,
          error: undefined
        };
      case successAction:
        return {
          isActive: false,
          success: true,
          error: undefined
        };
      case errorAction:
        const error = action as BaseErrorAction;
        return {
          isActive: false,
          success: false,
          error: error.payload.error
        };
      default:
        if (resetActions && resetActions.indexOf(action.type) !== -1) {
          return { ...initialState };
        }
        return state;
    }
  };
};
