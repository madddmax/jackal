export interface AuthState {
    user?: UserInfo;
    isAuthorised: boolean;
}

export interface CheckResponse {
    user?: UserInfo;
    isAuthorised: boolean;
}

export interface UserInfo {
    id: number;
    userName: string;
}
