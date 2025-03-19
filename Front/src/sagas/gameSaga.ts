import { takeEvery, put } from 'redux-saga/effects';
import { highlightHumanMoves, applyPirateChanges, applyStat, initGame } from '../redux/gameSlice';
import { GameStartResponse } from '../redux/types';
import { errorsWrapper, sagaActions } from './constants';
import { animateQueue } from '/app/global';

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

export function* applyTurnData(action: any) {
    const result = { data: action.payload };

    animateQueue.push(result.data);
    yield put({
        type: sagaActions.START_ANIMATE,
    });
    return;
}

export default function* rootSaga() {
    yield takeEvery(sagaActions.GAME_START_APPLY_DATA, errorsWrapper(applyStartData));
    yield takeEvery(sagaActions.GAME_TURN_APPLY_DATA, errorsWrapper(applyTurnData));
}
