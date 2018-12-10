
export interface BaseErrorAction {
    readonly type: string;
    readonly payload: {
        message: string;
    };
}
