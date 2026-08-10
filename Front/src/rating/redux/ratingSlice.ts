import { PayloadAction, createSlice } from '@reduxjs/toolkit';

import { LeaderBoardsInfo, RatingState } from '../types/rating';
import { LeaderBoardItemResponse } from '../types/ratingSaga';

export const ratingSlice = createSlice({
    name: 'rating',
    initialState: {
        leaders: {
            localLeaders: [],
            netLeaders: [],
            botLeaders: [],
            timestamp: Date.now(),
        },
        usersOnline: [],
    } satisfies RatingState as RatingState,
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
        applyUsersOnline: (state, action: PayloadAction<number[]>) => {
            state.usersOnline = action.payload;
        },
    },
    selectors: {
        getLeaders: (state): LeaderBoardsInfo => state.leaders,
        getUsersOnline: (state): number[] => state.usersOnline,
    },
});

export const {
    applyLeaderBoard,
    applyNetLeaderBoard,
    applyBotLeaderBoard,
    applyUsersOnline,
} = ratingSlice.actions;

export const { getLeaders, getUsersOnline } = ratingSlice.selectors;

export default ratingSlice.reducer;
