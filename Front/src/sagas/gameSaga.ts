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
    setMapInfo,
} from '../redux/gameSlice';
import { CheckMapInfo, GameStartResponse, GameTurnResponse } from '../redux/types';
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

    yield call(applyStartData, {
        type: sagaActions.GAME_START_APPLY_DATA,
        payload: result.data,
    });

    if (!result.data.stats.isHumanPlayer || result.data.moves?.length == 0) {
        yield call(gameTurn, {
            type: sagaActions.GAME_TURN,
            payload: { gameName: result.data.gameName },
        });
    }
}

export function* applyStartData(action: any) {
    const result = { data: action.payload };
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

    yield call(applyTurnData, {
        type: sagaActions.GAME_TURN_APPLY_DATA,
        payload: result.data,
    });

    if (result.data.stats.isGameOver) {
        return false;
    }

    return !result.data.stats.isHumanPlayer || result.data.moves?.length == 0;
}

export function* applyTurnData(action: any) {
    const result = { data: action.payload };
    yield put(applyStat(result.data.stats));
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

    if (result.data.stats.isGameOver) {
        yield put(highlightHumanMoves({ moves: [] }));
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
    yield takeEvery(sagaActions.GAME_RESET, errorsWrapper(gameReset));
    yield takeEvery(sagaActions.GAME_START, errorsWrapper(gameStart));
    yield takeEvery(sagaActions.GAME_START_APPLY_DATA, errorsWrapper(applyStartData));
    yield takeEvery(sagaActions.GAME_TURN, errorsWrapper(gameTurn));
    yield takeEvery(sagaActions.GAME_TURN_APPLY_DATA, errorsWrapper(applyTurnData));
    yield takeEvery(sagaActions.GET_TILES_PACK_NAMES, errorsWrapper(getTilesPackNames));
    yield takeEvery(sagaActions.CHECK_MAP, errorsWrapper(checkMap));
}
