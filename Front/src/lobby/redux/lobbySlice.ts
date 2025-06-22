import { PayloadAction, createSlice } from '@reduxjs/toolkit';

import { GameInfo } from '../../common/redux.types';
import { LeaderBoardItemResponse } from '../types/sagaContracts';
import { LobbyGameInfo, LobbyGamesEntriesList, LobbyState, NetGameInfo } from './lobbySlice.types';

export const lobbySlice = createSlice({
    name: 'lobby',
    initialState: {
        gamelist: [],
        netgamelist: [],
    } satisfies LobbyState as LobbyState,
    reducers: {
        applyLeaderBoard: (state, action: PayloadAction<LeaderBoardItemResponse[]>) => {
            state.leaderBoard = action.payload;
        },
        applyGamesList: (state, action: PayloadAction<LobbyGamesEntriesList>) => {
            state.gamelist = action.payload.gamesEntries.map((it) => ({
                id: it.gameId,
                creatorName: it.creator.name,
                isCreator: it.creator.id === action.payload.currentUserId,
                isPlayer: it.players.some((it) => it.id === action.payload.currentUserId),
                isPublic: it.players.length > 1,
                timeStamp: it.timeStamp,
            }));
        },
        applyNetGamesList: (state, action: PayloadAction<LobbyGamesEntriesList>) => {
            state.netgamelist = action.payload.gamesEntries.map((it) => ({
                id: it.gameId,
                creatorName: it.creator.name,
                isCreator: it.creator.id === action.payload.currentUserId,
                isPlayer: it.players.some((it) => it.id === action.payload.currentUserId),
                isPublic: true,
                timeStamp: it.timeStamp,
            }));
        },
        applyNetGame: (state, action: PayloadAction<LobbyGameInfo>) => {
            state.netGame = {
                ...action.payload.gameInfo,
                isCreator: action.payload.gameInfo.creatorId === action.payload.currentUserId,
            };
        },
    },
    selectors: {
        getLeaderBoard: (state): LeaderBoardItemResponse[] | undefined => state.leaderBoard,
        getGames: (state): GameInfo[] => state.gamelist,
        getNetGames: (state): GameInfo[] => state.netgamelist,
        getNetGame: (state): NetGameInfo | undefined => state.netGame,
    },
});

export const { applyLeaderBoard, applyGamesList, applyNetGamesList, applyNetGame } = lobbySlice.actions;

export const { getLeaderBoard, getGames, getNetGames, getNetGame } = lobbySlice.selectors;

export default lobbySlice.reducer;
