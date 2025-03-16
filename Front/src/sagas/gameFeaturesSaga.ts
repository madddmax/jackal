import { call, delay, fork, put, select, take, takeEvery } from 'redux-saga/effects';
import { axiosInstance, errorsWrapper, sagaActions } from './constants';
import {
    applyChanges,
    applyPirateChanges,
    applyStat,
    getCurrentTeam,
    highlightHumanMoves,
    removeHumanMoves,
    setMapInfo,
    setTilesPackNames,
} from '/redux/gameSlice';
import { CheckMapInfo, GameTurnResponse, ReduxState, TeamState } from '/redux/types';
import { animateQueue } from '/app/global';
import { PayloadAction } from '@reduxjs/toolkit';

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
            type: sagaActions.GAME_TURN_APPLY_DATA,
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

    const currentTeam: TeamState = yield select((state: ReduxState) => getCurrentTeam(state));
    const speed: number = yield select((state) => state.game.userSettings.gameSpeed);

    if (!currentTeam.isHuman) {
        yield put(removeHumanMoves());
        if (speed > 0) {
            yield delay(speed * 100);
        }
    }

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

export function* getTilesPackNames() {
    let result: { data: string[] } = yield call(
        async () =>
            await axiosInstance({
                url: 'v1/map/tiles-pack-names',
                method: 'get',
            }),
    );
    yield put(setTilesPackNames(result.data));
}

export function* checkMap(action: any) {
    let result: { data: CheckMapInfo[] } = yield call(
        async () =>
            await axiosInstance({
                url: 'v1/map/check-landing',
                method: 'get',
                params: action.payload,
            }),
    );
    yield put(setMapInfo(result.data.map((it) => it.difficulty)));
}

export default function* rootSaga() {
    yield fork(watchAnimation), yield takeEvery(sagaActions.GET_TILES_PACK_NAMES, errorsWrapper(getTilesPackNames));
    yield takeEvery(sagaActions.CHECK_MAP, errorsWrapper(checkMap));
}
