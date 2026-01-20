import { PayloadAction, createSlice } from '@reduxjs/toolkit';

import { LeaderBoardsInfo, LobbyState } from '../types/lobby';
import { LeaderBoardItemResponse } from '../types/lobbySaga';
import { GameInfo, LobbyGameInfo, LobbyGamesEntriesList, NetGameInfo } from '../types/lobbySlice';

export const lobbySlice = createSlice({
    name: 'lobby',
    initialState: {
        gamelist: [],
        netgamelist: [],
        leaders: {
            localLeaders: [],
            netLeaders: [],
            botLeaders: [],
            timestamp: Date.now(),
        },
        usersOnline: [],
    } satisfies LobbyState as LobbyState,
    reducers: {
        applyLeaderBoard: (state, action: PayloadAction<LeaderBoardItemResponse[]>) => {
            state.leaders.localLeaders = action.payload;
        },
        applyNetLeaderBoard: (state, action: PayloadAction<LeaderBoardItemResponse[]>) => {
            state.leaders.netLeaders = action.payload;
        },
        applyBotLeaderBoard: (state, action: PayloadAction<LeaderBoardItemResponse[]>) => {
            state.leaders.botLeaders = action.payload;
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
        applyUsersOnline: (state, action: PayloadAction<number[]>) => {
            state.usersOnline = action.payload;
        },
    },
    selectors: {
        getLeaders: (state): LeaderBoardsInfo => state.leaders,
        getGames: (state): GameInfo[] => state.gamelist,
        getNetGames: (state): GameInfo[] => state.netgamelist,
        getNetGame: (state): NetGameInfo | undefined => state.netGame,
        getUsersOnline: (state): number[] | undefined => state.usersOnline,
    },
});

export const {
    applyLeaderBoard,
    applyNetLeaderBoard,
    applyBotLeaderBoard,
    applyGamesList,
    applyNetGamesList,
    applyNetGame,
    applyUsersOnline,
} = lobbySlice.actions;

export const { getLeaders, getGames, getNetGames, getNetGame, getUsersOnline } = lobbySlice.selectors;

export default lobbySlice.reducer;
