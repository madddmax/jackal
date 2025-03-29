import { PayloadAction, createSlice } from '@reduxjs/toolkit';

import { CommonState, MessageInfo } from './commonSlice.types';

export const commonSlice = createSlice({
    name: 'common',
    initialState: {
        enableSockets: true,
        messageQueue: [],
    } satisfies CommonState as CommonState,
    reducers: {
        activateSockets: (state, action: PayloadAction<boolean>) => {
            state.enableSockets = action.payload;
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
    selectors: {
        getMessage: (state) => state.message,
        getEnableSockets: (state) => state.enableSockets,
    },
});

export const { activateSockets, showMessage, processError, hideError } = commonSlice.actions;

export const { getMessage, getEnableSockets } = commonSlice.selectors;

export default commonSlice.reducer;
