import { PayloadAction, createSlice } from '@reduxjs/toolkit';
import { LobbyInfo, LobbyState } from './types';

export const lobbySlice = createSlice({
    name: 'lobby',
    initialState: {} satisfies LobbyState as LobbyState,
    reducers: {
        updateLobby: (state, action: PayloadAction<LobbyInfo>) => {
            state.lobby = action.payload;
        },
    },
});

export const { updateLobby } = lobbySlice.actions;

export default lobbySlice.reducer;
