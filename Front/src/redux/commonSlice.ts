import { PayloadAction, createSlice } from '@reduxjs/toolkit';
import { CommonState, ErrorInfo } from './commonSlice.d';

export const commonSlice = createSlice({
    name: 'common',
    initialState: {
        errorQueue: [],
    } satisfies CommonState as CommonState,
    reducers: {
        showError: (state, action: PayloadAction<ErrorInfo>) => {
            if (state.error) {
                state.errorQueue.push(action.payload);
            } else {
                state.error = action.payload;
            }
        },
        processError: (state) => {
            state.error = state.errorQueue.shift();
        },
        hideError: (state) => {
            state.error = undefined;
        },
    },
});

export const { showError, processError, hideError } = commonSlice.actions;

export default commonSlice.reducer;
