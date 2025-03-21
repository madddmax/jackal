import { takeEvery, put, take, call, fork, select, delay } from 'redux-saga/effects';
import {
    highlightHumanMoves,
    applyPirateChanges,
    applyStat,
    initGame,
    getCurrentTeam,
    removeHumanMoves,
    applyChanges,
} from '../redux/gameSlice';
import { GameStartResponse, GameTurnResponse, ReduxState, TeamState } from '../../common/redux.types';
import { errorsWrapper, sagaActions } from '/common/sagas';
import { animateQueue } from '/app/global';
import { PayloadAction } from '@reduxjs/toolkit';

export function* applyStartData(action: any) {
    const data: GameStartResponse = action.payload;
    yield put(initGame(data));
    yield put(applyStat(data));
    yield put(
        applyPirateChanges({
            moves: data.moves,
            changes: data.pirates,
        }),
    );
    yield put(highlightHumanMoves({ moves: data.moves }));
}

export function* applyTurn(action: any) {
    const result = { data: action.payload };

    animateQueue.push(result.data);
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

export default function* rootSaga() {
    yield takeEvery(sagaActions.GAME_START_APPLY_DATA, errorsWrapper(applyStartData));
    yield takeEvery(sagaActions.GAME_TURN_APPLY_DATA, errorsWrapper(applyTurn));
    yield fork(watchAnimation);
}
