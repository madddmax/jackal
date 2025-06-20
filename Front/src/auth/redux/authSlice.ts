import { PayloadAction, createSlice } from '@reduxjs/toolkit';

import { AuthState } from '../types/auth';
import { AuthInfo } from '../types/authSlice';
import { axiosInstance } from '/common/sagas';

export const authSlice = createSlice({
    name: 'auth',
    initialState: {} satisfies AuthState as AuthState,
    reducers: {
        setAuth: (state, action: PayloadAction<AuthInfo>) => {
            if (action.payload.token) {
                localStorage.auth = action.payload.token;
                axiosInstance.defaults.headers.common['Authorization'] = localStorage.auth;
            } else {
                localStorage.removeItem('auth');
                delete axiosInstance.defaults.headers.common['Authorization'];
            }

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
