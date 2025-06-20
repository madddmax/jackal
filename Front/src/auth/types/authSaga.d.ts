export interface CheckResponse {
    user: UserInfo;
}

export interface AuthResponse extends CheckResponse {
    token: string;
}

export interface AuthLoginRequest {
    login: string;
}
