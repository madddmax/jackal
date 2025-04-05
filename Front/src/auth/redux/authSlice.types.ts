export interface AuthState {
    user?: UserInfo;
    isAuthorised: boolean;
}

export interface AuthInfo {
    token?: string;
    user?: UserInfo;
    isAuthorised: boolean;
}

export interface CheckResponse {
    user: UserInfo;
}

export interface AuthResponse extends CheckResponse {
    token: string;
}

export interface UserInfo {
    id: number;
    login: string;
}
