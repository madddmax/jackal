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
        updateLobby: (state, action: PayloadAction<LobbyInfo>) => {
            let lobby = state.lobbies.find((it) => it.id === action.payload.id);
            if (lobby !== undefined) {
                Object.assign(lobby, action.payload);
            }
        },
    },
});

export const { addLobby, updateLobby } = lobbySlice.actions;

export default lobbySlice.reducer;
