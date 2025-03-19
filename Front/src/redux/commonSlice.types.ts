export interface CommonState {
    enableSockets: boolean;
    message?: MessageInfo;
    messageQueue: MessageInfo[];
}

export interface ErrorInfo {
    error: boolean;
    errorCode: string;
    errorMessage: string;
}

export interface MessageInfo {
    isError: boolean;
    errorCode: string;
    messageText: string;
}
