import { call, takeEvery, put } from 'redux-saga/effects';
import {
    initMap,
    setCurrentHumanTeam,
    highlightHumanMoves,
    applyPirateChanges,
    applyChanges,
    applyStat,
    initGame,
    setTilesPackNames,
} from '../redux/gameSlice';
import { GameStartResponse, GameTurnResponse } from '../redux/types';
import { axiosInstance, errorsWrapper, sagaActions } from './constants';

export function* gameReset() {
    let result: GameStartResponse = yield call(
        async () =>
            await axiosInstance({
                url: 'Game/Reset',
                method: 'post',
            }),
    );
    console.log(result);
    // yield put(initMap(result.data.));
}

export function* gameStart(action: any) {
    let result: { data: GameStartResponse } = yield call(
        async () =>
            await axiosInstance({
                url: 'v1/game/start',
                method: 'post',
                data: action.payload,
            }),
    );
    yield put(initMap(result.data.map));
    yield put(initGame(result.data));
    yield put(
        applyPirateChanges({
            moves: result.data.moves,
            changes: result.data.pirates,
            isHumanPlayer: result.data.stats.isHumanPlayer,
        }),
    );
    if (result.data.stats.isHumanPlayer) {
        yield put(setCurrentHumanTeam(result.data.stats.currentTeamId));
        yield put(highlightHumanMoves({ moves: result.data.moves }));
    }
    yield put(applyStat(result.data.stats));
    if (!result.data.stats.isHumanPlayer || result.data.moves?.length == 0) {
        yield call(gameTurn, {
            type: sagaActions.GAME_TURN,
            payload: { gameName: result.data.gameName },
        });
    }
}

export function* gameTurn(action: any) {
    let mustContinue = true;
    while (mustContinue) {
        mustContinue = yield call(oneTurn, action);
    }
}

export function* oneTurn(action: any) {
    let result: { data: GameTurnResponse } = yield call(
        async () =>
            await axiosInstance({
                url: 'v1/game/move',
                method: 'post',
                data: action.payload,
            }),
    );
    if (result.data.stats.isGameOver) {
        yield put(applyStat(result.data.stats));
        return false;
    }
    yield put(applyChanges(result.data.changes));
    yield put(
        applyPirateChanges({
            moves: result.data.moves,
            changes: result.data.pirateChanges,
            isHumanPlayer: result.data.stats.isHumanPlayer,
        }),
    );
    if (result.data.stats.isHumanPlayer) {
        yield put(setCurrentHumanTeam(result.data.stats.currentTeamId));
        yield put(highlightHumanMoves({ moves: result.data.moves }));
    }
    yield put(applyStat(result.data.stats));

    return !result.data.stats.isHumanPlayer || result.data.moves?.length == 0;
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

export default function* rootSaga() {
    yield takeEvery(sagaActions.GAME_RESET, errorsWrapper(gameReset));
    yield takeEvery(sagaActions.GAME_START, errorsWrapper(gameStart));
    yield takeEvery(sagaActions.GAME_TURN, errorsWrapper(gameTurn));
    yield takeEvery(sagaActions.GET_TILES_PACK_NAMES, errorsWrapper(getTilesPackNames));
}
