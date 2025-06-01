import { PayloadAction, createSlice } from '@reduxjs/toolkit';

import { GameInfo, LobbyInfo, LobbyState, NetGameListResponse } from '../../common/redux.types';

export const lobbySlice = createSlice({
    name: 'lobby',
    initialState: {
        gamelist: [],
        netgamelist: [],
    } satisfies LobbyState as LobbyState,
    reducers: {
        updateLobby: (state, action: PayloadAction<LobbyInfo>) => {
            state.lobby = action.payload;
        },
        applyGamesList: (state, action: PayloadAction<NetGameListResponse>) => {
            state.gamelist = action.payload.gamesEntries.map((it) => ({
                id: it.gameId,
                creator: it.creator,
                timeStamp: it.timeStamp,
            }));
        },
        applyNetGamesList: (state, action: PayloadAction<NetGameListResponse>) => {
            state.netgamelist = action.payload.gamesEntries.map((it) => ({
                id: it.gameId,
                creator: it.creator,
                timeStamp: it.timeStamp,
            }));
        },
    },
    selectors: {
        getLobby: (state): LobbyInfo | undefined => state.lobby,
        getGames: (state): GameInfo[] => state.gamelist,
        getNetGames: (state): GameInfo[] => state.netgamelist,
    },
});

export const { updateLobby, applyGamesList, applyNetGamesList } = lobbySlice.actions;

export const { getLobby, getGames, getNetGames } = lobbySlice.selectors;

export default lobbySlice.reducer;
