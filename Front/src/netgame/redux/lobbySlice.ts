import { PayloadAction, createSlice } from '@reduxjs/toolkit';

import { GameInfo, LobbyInfo, LobbyState, NetgameListResponse } from '../../common/redux.types';

export const lobbySlice = createSlice({
    name: 'lobby',
    initialState: {
        gamelist: [],
    } satisfies LobbyState as LobbyState,
    reducers: {
        updateLobby: (state, action: PayloadAction<LobbyInfo>) => {
            state.lobby = action.payload;
        },
        applyGamesList: (state, action: PayloadAction<NetgameListResponse>) => {
            state.gamelist = action.payload.games.map((it) => ({ id: it }));
        },
    },
    selectors: {
        getLobby: (state): LobbyInfo | undefined => state.lobby,
        getGames: (state): GameInfo[] => state.gamelist,
    },
});

export const { updateLobby, applyGamesList } = lobbySlice.actions;

export const { getLobby, getGames } = lobbySlice.selectors;

export default lobbySlice.reducer;
