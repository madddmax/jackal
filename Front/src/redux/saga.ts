import { call, takeEvery, put } from 'redux-saga/effects';
import axios from 'axios';
import config from '/app/config';
import {
    initMap,
    setTeam,
    highlightMoves,
    applyPirateChanges,
    applyChanges,
    applyStat,
    initGame,
} from './gameSlice';
import { GameStartResponse, GameTurnResponse } from './types';

export const sagaActions = {
    GAME_RESET: 'GAME_RESET',
    GAME_START: 'GAME_START',
    GAME_TURN: 'GAME_TURN',
};

export function* gameReset() {
    try {
        let result: GameStartResponse = yield call(
            async () =>
                await axios({
                    url: `${config.BaseApi}Game/Reset`,
                    method: 'post',
                }),
        );
        console.log(result);
        // yield put(initMap(result.data.));
    } catch (e) {
        yield put({ type: 'TODO_FETCH_FAILED' });
    }
}

export function* gameStart(action: any) {
    try {
        let result: { data: GameStartResponse } = yield call(
            async () =>
                await axios({
                    url: `${config.BaseApi}Game/MakeStart`,
                    method: 'post',
                    data: action.payload,
                }),
        );
        yield put(initMap(result.data.map));
        console.log('gameStart');
        yield put(initGame(result.data));
        if (result.data.stat.isHumanPlayer) {
            yield put(setTeam(result.data.stat.currentTeamId));
            yield put(highlightMoves({ moves: result.data.moves }));
        }
        yield put(applyStat(result.data.stat));
        if (!result.data.stat.isHumanPlayer || result.data.moves?.length == 0) {
            yield call(gameTurn, {
                type: sagaActions.GAME_TURN,
                payload: { gameName: result.data.gameName },
            });
        }
    } catch (e) {
        yield put({ type: 'TODO_FETCH_FAILED' });
    }
}

export function* gameTurn(action: any) {
    let mustContinue = true;
    while (mustContinue) {
        mustContinue = yield call(oneTurn, action);
    }
}

export function* oneTurn(action: any) {
    try {
        let result: { data: GameTurnResponse } = yield call(
            async () =>
                await axios({
                    url: `${config.BaseApi}Game/MakeTurn`,
                    method: 'post',
                    data: action.payload,
                }),
        );
        if (result.data.stat.isGameOver) {
            yield put(applyStat(result.data.stat));
            return false;
        }

        yield put(
            applyPirateChanges({
                moves: result.data.moves,
                changes: result.data.pirateChanges,
                isHumanPlayer: result.data.stat.isHumanPlayer,
            }),
        );
        if (result.data.stat.isHumanPlayer) {
            yield put(setTeam(result.data.stat.currentTeamId));
            yield put(highlightMoves({ moves: result.data.moves }));
        }
        yield put(applyChanges(result.data.changes));
        yield put(applyStat(result.data.stat));

        return (
            !result.data.stat.isHumanPlayer || result.data.moves?.length == 0
        );
    } catch (e) {
        yield put({ type: 'TODO_FETCH_FAILED' });
    }
}

export default function* rootSaga() {
    yield takeEvery(sagaActions.GAME_RESET, gameReset);
    yield takeEvery(sagaActions.GAME_START, gameStart);
    yield takeEvery(sagaActions.GAME_TURN, gameTurn);
}
