import { PayloadAction, createSlice } from '@reduxjs/toolkit';
import { CommonState, MessageInfo } from './commonSlice.types';

export const commonSlice = createSlice({
    name: 'common',
    initialState: {
        useSockets: false,
        messageQueue: [],
    } satisfies CommonState as CommonState,
    reducers: {
        activateSockets: (state, action: PayloadAction<boolean>) => {
            state.useSockets = action.payload;
        },
        showMessage: (state, action: PayloadAction<MessageInfo>) => {
            if (state.message) {
                state.messageQueue.push(action.payload);
            } else {
                state.message = action.payload;
            }
        },
        processError: (state) => {
            state.message = state.messageQueue.shift();
        },
        hideError: (state) => {
            state.message = undefined;
        },
    },
});

export const { activateSockets, showMessage, processError, hideError } = commonSlice.actions;

export default commonSlice.reducer;
