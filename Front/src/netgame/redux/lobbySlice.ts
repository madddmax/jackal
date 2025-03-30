import { PayloadAction, createSlice } from '@reduxjs/toolkit';

import { LobbyInfo, LobbyState } from '../../common/redux.types';

export const lobbySlice = createSlice({
    name: 'lobby',
    initialState: {} satisfies LobbyState as LobbyState,
    reducers: {
        updateLobby: (state, action: PayloadAction<LobbyInfo>) => {
            state.lobby = action.payload;
        },
    },
    selectors: {
        getLobby: (state): LobbyInfo | undefined => state.lobby,
    },
});

export const { updateLobby } = lobbySlice.actions;

export const { getLobby } = lobbySlice.selectors;

export default lobbySlice.reducer;
