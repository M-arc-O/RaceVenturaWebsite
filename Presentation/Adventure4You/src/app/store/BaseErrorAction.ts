
export interface BaseErrorAction {
    readonly type: string;
    readonly payload: {
        error: Response;
    };
}
