import { PayloadAction, createSlice } from '@reduxjs/toolkit';
import { AuthState, CheckResponse } from './authSlice.types';

export const authSlice = createSlice({
    name: 'auth',
    initialState: {
        isAuthorised: false,
    } satisfies AuthState as AuthState,
    reducers: {
        setAuth: (state, action: PayloadAction<CheckResponse>) => {
            state.user = action.payload.user;
            state.isAuthorised = action.payload.isAuthorised;
        },
    },
});

export const { setAuth } = authSlice.actions;

export default authSlice.reducer;
