export interface AuthState {
    user?: UserInfo;
    isAuthorised: boolean;
}

export interface CheckResponse {
    user?: UserInfo;
    isAuthorised: boolean;
}

export interface AuthResponse {
    user: UserInfo;
}

export interface UserInfo {
    id: number;
    userName: string;
}
