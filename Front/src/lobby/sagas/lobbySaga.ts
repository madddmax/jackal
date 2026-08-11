import { PayloadAction } from '@reduxjs/toolkit';
import { put, select, takeEvery } from 'redux-saga/effects';

import {
    applyGamesList,
    applyNetGame,
    applyNetGamesList,
} from '../redux/lobbySlice';
import {
    NetGameInfoResponse,
    NetGameListResponse,
} from '../types/lobbySaga';
import { getAuth } from '/auth/redux/authSlice';
import { AuthState } from '/auth/types/auth';
import { errorsWrapper, sagaActions } from '/common/sagas';
import gameHub from '/game/hub/gameHub';

export function* applyActiveGamesData(action: PayloadAction<NetGameListResponse>) {
    const auth: AuthState = yield select(getAuth);
    const data = action.payload;
    yield put(applyGamesList({ currentUserId: auth.user?.id, gamesEntries: data.gamesEntries }));
}

export function* applyNetGamesData(action: PayloadAction<NetGameListResponse>) {
    const auth: AuthState = yield select(getAuth);
    const data = action.payload;
    yield put(applyNetGamesList({ currentUserId: auth.user?.id, gamesEntries: data.gamesEntries }));
}

export function* applyNetGameData(action: PayloadAction<NetGameInfoResponse>) {
    const auth: AuthState = yield select(getAuth);
    const data = action.payload;
    yield put(applyNetGame({ currentUserId: auth.user?.id, gameInfo: data }));
    if (data.gameId) {
        gameHub.loadGame(data.gameId);
    }
}

export default function* rootSaga() {
    yield takeEvery(sagaActions.ACTIVE_GAMES_APPLY_DATA, errorsWrapper(applyActiveGamesData));
    yield takeEvery(sagaActions.NET_GAMES_APPLY_DATA, errorsWrapper(applyNetGamesData));
    yield takeEvery(sagaActions.NET_GAME_APPLY_DATA, errorsWrapper(applyNetGameData));
}
