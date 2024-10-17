export interface CommonState {
    error?: ErrorInfo;
    errorQueue: ErrorInfo[];
}

export interface ErrorInfo {
    error: boolean;
    errorCode: string;
    errorMessage: string;
}
