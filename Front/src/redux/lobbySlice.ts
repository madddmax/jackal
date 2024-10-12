import { PayloadAction, createSlice } from '@reduxjs/toolkit';
import { LobbyInfo, LobbyState } from './types';

export const lobbySlice = createSlice({
    name: 'lobby',
    initialState: {
        lobbies: [],
    } satisfies LobbyState as LobbyState,
    reducers: {
        addLobby: (state, action: PayloadAction<LobbyInfo>) => {
            state.lobbies.push(action.payload);
        },
    },
});

export const { addLobby } = lobbySlice.actions;

export default lobbySlice.reducer;
