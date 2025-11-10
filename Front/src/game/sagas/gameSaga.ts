import { PayloadAction } from '@reduxjs/toolkit';
import { call, delay, fork, put, select, take, takeEvery } from 'redux-saga/effects';

import {
    applyChanges,
    applyPirateChanges,
    applyStat,
    checkBottles,
    getGameStatistics,
    getUserSettings,
    highlightHumanMoves,
    initGame,
    removeHumanMoves,
} from '../redux/gameSlice';
import { StorageState } from '../types';
import { GameStartResponse, GameTurnResponse } from '../types/gameSaga';
import { history } from '/app/global';
import { getAuth } from '/auth/redux/authSlice';
import { AuthState } from '/auth/types/auth';
import { errorsWrapper, sagaActions } from '/common/sagas';

const animateQueue: GameTurnResponse[] = [];

export function* applyStartData(action: { payload: GameStartResponse }) {
    const data = action.payload;
    const auth: AuthState = yield select(getAuth);
    data.stats.isCurrentUsersMove = data.stats.currentUserId === auth.user?.id;
    data.teams.forEach((it) => {
        if (it.userId === auth.user?.id) it.isCurrentUser = true;
    });
    yield put(initGame(data));
    yield put(applyStat(data));
    yield put(
        applyPirateChanges({
            moves: data.moves,
            changes: data.pirates,
        }),
    );
    yield put(highlightHumanMoves({ moves: data.moves }));
    history.navigate && history.navigate('/');
}

export function* applyTurn(action: { payload: GameTurnResponse }) {
    animateQueue.push(action.payload);
    yield put({
        type: sagaActions.START_ANIMATE,
    });
    return;
}

function* watchAnimation() {
    while (true) {
        yield take(sagaActions.START_ANIMATE);
        yield call(doAnimate);
    }
}

function* doAnimate() {
    let elm = animateQueue.shift();
    while (elm) {
        yield call(applyTurnData, {
            type: sagaActions.GAME_TURN_APPLY_DATA, // любой тип
            payload: elm,
        });
        elm = animateQueue.shift();
    }
    yield put({
        type: sagaActions.STOP_ANIMATE,
    });
}

export function* applyTurnData(action: PayloadAction<GameTurnResponse>) {
    const result = { data: action.payload };

    const prevStat: GameStat | undefined = yield select(getGameStatistics);
    const auth: AuthState = yield select(getAuth);
    result.data.stats.isCurrentUsersMove = result.data.stats.currentUserId === auth.user?.id;
    const { gameSpeed: speed }: StorageState = yield select(getUserSettings);

    yield put(removeHumanMoves());
    if (!prevStat?.isCurrentUsersMove) {
        if (speed > 0) {
            yield delay(speed * 100);
        }
    }

    yield put(checkBottles(result.data.teamScores));
    yield put(applyStat(result.data));
    yield put(applyChanges(result.data.changes));
    yield put(
        applyPirateChanges({
            moves: result.data.moves,
            changes: result.data.pirateChanges,
        }),
    );
    yield put(highlightHumanMoves({ moves: result.data.moves }));

    if (result.data.stats.isGameOver) {
        yield put(removeHumanMoves());
    }
}

export function* applyLookingData(action: { payload: GameStartResponse }) {
    const data = action.payload;
    const auth: AuthState = yield select(getAuth);
    data.stats.isCurrentUsersMove = data.stats.currentUserId === auth.user?.id;
    data.teams.forEach((it) => {
        if (it.userId === auth.user?.id) it.isCurrentUser = true;
    });
    yield put(initGame(data));
    yield put(applyStat(data));
    yield put(
        applyPirateChanges({
            moves: data.moves,
            changes: data.pirates,
        }),
    );
    yield put(highlightHumanMoves({ moves: data.moves }));
    history.navigate && history.navigate('/');
}

export default function* rootSaga() {
    yield takeEvery(sagaActions.GAME_START_APPLY_DATA, errorsWrapper(applyStartData));
    yield takeEvery(sagaActions.GAME_TURN_APPLY_DATA, errorsWrapper(applyTurn));
    yield takeEvery(sagaActions.GAME_START_LOOKING_DATA, errorsWrapper(applyLookingData));
    yield fork(watchAnimation);
}
