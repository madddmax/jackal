import { call, takeEvery, put, select } from 'redux-saga/effects';
import { setCurrentHumanTeam, highlightHumanMoves, applyPirateChanges, applyStat, initGame } from '../redux/gameSlice';
import { GameStartResponse, GameState, GameTurnResponse, TeamState } from '../redux/types';
import { axiosInstance, errorsWrapper, sagaActions } from './constants';
import { animateQueue } from '/app/global';

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

    const currentTeam = result.data.stats.teams.find((it) => it.id === result.data.stats.currentTeamId);
    if (!currentTeam!.isHuman || result.data.moves?.length == 0) {
        yield call(gameTurn, {
            type: sagaActions.GAME_TURN,
            payload: { gameName: result.data.gameName },
        });
    }
}

export function* applyStartData(action: any) {
    const data: GameStartResponse = action.payload;
    yield put(initGame(data));
    yield put(
        applyPirateChanges({
            moves: data.moves,
            changes: data.pirates,
        }),
    );

    const currentTeam = data.stats.teams.find((it) => it.id === data.stats.currentTeamId);
    if (currentTeam!.isHuman) {
        yield put(setCurrentHumanTeam(data.stats.currentTeamId));
        yield put(highlightHumanMoves({ moves: data.moves }));
    }
    yield put(applyStat(data.stats));
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

    const currentTeam: TeamState = yield select((state: { game: GameState }) =>
        state.game.teams.find((it) => it.id === result.data.stats.currentTeamId),
    );
    return !currentTeam!.isHuman || result.data.moves?.length == 0;
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
    yield takeEvery(sagaActions.GAME_START, errorsWrapper(gameStart));
    yield takeEvery(sagaActions.GAME_START_APPLY_DATA, errorsWrapper(applyStartData));
    yield takeEvery(sagaActions.GAME_TURN, errorsWrapper(gameTurn));
    yield takeEvery(sagaActions.GAME_TURN_APPLY_DATA, errorsWrapper(applyTurnData));
}
