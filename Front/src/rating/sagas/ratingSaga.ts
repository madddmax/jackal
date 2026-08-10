import { PayloadAction } from '@reduxjs/toolkit';
import { call, put, takeEvery } from 'redux-saga/effects';

import {
    applyBotLeaderBoard,
    applyLeaderBoard,
    applyNetLeaderBoard,
    applyUsersOnline,
} from '../redux/ratingSlice';
import {
    LeaderBoardItemResponse,
    NetGameUsersOnlineResponse,
} from '../types/ratingSaga';
import { axiosInstance, errorsWrapper, sagaActions } from '/common/sagas';

export function* applyNetGameUsersOnline(action: PayloadAction<NetGameUsersOnlineResponse>) {
    const data = action.payload;
    yield put(applyUsersOnline(data.users));
}

export function* getLeaderBoardData() {
    const result: { data: { leaderboard: LeaderBoardItemResponse[] } } = yield call(
        async () =>
            await axiosInstance({
                url: 'v1/leaderboard',
                method: 'get',
            }),
    );
    yield put(applyLeaderBoard(result.data.leaderboard));
}

export function* getNetLeaderBoardData() {
    const result: { data: { leaderboard: LeaderBoardItemResponse[] } } = yield call(
        async () =>
            await axiosInstance({
                url: 'v1/leaderboard/two-human-in-team',
                method: 'get',
            }),
    );
    yield put(applyNetLeaderBoard(result.data.leaderboard));
}

export function* getBotLeaderBoardData() {
    const result: { data: { leaderboard: LeaderBoardItemResponse[] } } = yield call(
        async () =>
            await axiosInstance({
                url: 'v1/leaderboard/bot-all',
                method: 'get',
            }),
    );
    yield put(applyBotLeaderBoard(result.data.leaderboard));
}

export default function* rootSaga() {
    yield takeEvery(sagaActions.NET_GAME_USERS_ONLINE, errorsWrapper(applyNetGameUsersOnline));
    yield takeEvery(sagaActions.LOBBY_GET_LEADERBOARD, errorsWrapper(getLeaderBoardData));
    yield takeEvery(sagaActions.LOBBY_GET_NET_LEADERBOARD, errorsWrapper(getNetLeaderBoardData));
    yield takeEvery(sagaActions.LOBBY_GET_BOT_LEADERBOARD, errorsWrapper(getBotLeaderBoardData));
}
