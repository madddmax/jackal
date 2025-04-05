import { PayloadAction, createSlice } from '@reduxjs/toolkit';

import { AuthInfo, AuthState } from './authSlice.types';

export const authSlice = createSlice({
    name: 'auth',
    initialState: {
        isAuthorised: false,
    } satisfies AuthState as AuthState,
    reducers: {
        setAuth: (state, action: PayloadAction<AuthInfo>) => {
            if (action.payload.token) localStorage.auth = action.payload.token;
            else localStorage.removeItem('auth');
            authSlice.caseReducers.checkAuth(state, checkAuth(action.payload));
        },
        checkAuth: (state, action: PayloadAction<AuthInfo>) => {
            state.user = action.payload.user;
            state.isAuthorised = action.payload.isAuthorised;
        },
    },
    selectors: {
        getAuth: (state): AuthState => state,
    },
});

export const { setAuth, checkAuth } = authSlice.actions;

export const { getAuth } = authSlice.selectors;

export default authSlice.reducer;
