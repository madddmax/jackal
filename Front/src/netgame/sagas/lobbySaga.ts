import { PayloadAction } from '@reduxjs/toolkit';
//import { call, delay, fork, put, race, select, take, takeEvery } from 'redux-saga/effects';
import { call, put, select, takeEvery } from 'redux-saga/effects';

//import { LobbyCreateResponse, LobbyGetResponse, LobbyInfo, LobbyJoinResponse, NetGameListResponse } from '../../common/redux.types';
import {
    LobbyCreateResponse,
    LobbyGetResponse,
    LobbyJoinResponse,
    NetGameInfoResponse,
    NetGameListResponse,
} from '../../common/redux.types';
import { applyGamesList, applyLeaderBoard, applyNetGame, applyNetGamesList, updateLobby } from '../redux/lobbySlice';
import { LeaderBoardItemResponse } from '../types/sagaContracts';
import { history } from '/app/global';
import { getAuth } from '/auth/redux/authSlice';
import { AuthState } from '/auth/redux/authSlice.types';
import { axiosInstance, errorsWrapper, sagaActions } from '/common/sagas';
import gameHub from '/game/hub/gameHub';

export function* lobbyCreate(action: PayloadAction<{ lobbyId: string }>) {
    const result: { data: LobbyCreateResponse } = yield call(
        async () =>
            await axiosInstance({
                url: 'v1/lobby/create-lobby',
                method: 'post',
                data: action.payload,
            }),
    );
    yield put(updateLobby(result.data.lobby));
    history.navigate && history.navigate('/lobby/' + result.data.lobby.id);
}

export function* lobbyJoin(action: PayloadAction<{ lobbyId: string }>) {
    const result: { data: LobbyJoinResponse } = yield call(
        async () =>
            await axiosInstance({
                url: 'v1/lobby/join-lobby',
                method: 'post',
                data: action.payload,
            }),
    );
    yield put(updateLobby(result.data.lobby));
    history.navigate && history.navigate('/lobby/' + result.data.lobby.id);
}

export function* lobbyGet(action: PayloadAction<{ lobbyId: string }>) {
    const result: { data: LobbyGetResponse } = yield call(
        async () =>
            await axiosInstance({
                url: 'v1/lobby/get-lobby',
                method: 'post',
                data: action.payload,
            }),
    );
    yield put(updateLobby(result.data.lobby));
    yield put({ type: sagaActions.LOBBY_DO_POLLING });
}

// function* lobbyPolling() {
//     yield delay(3000);
//     const lobby: LobbyInfo = yield select((state) => state.lobby.lobby);
//     if (!lobby) {
//         yield put({ type: sagaActions.LOBBY_STOP_POLLING });
//         return;
//     }

//     yield put({
//         type: sagaActions.LOBBY_GET,
//         payload: {
//             lobbyId: lobby.id,
//         },
//     });
// }

// function* watchLobbyPolling() {
//     while (true) {
//         yield take([sagaActions.LOBBY_DO_POLLING]);
//         yield race([call(lobbyPolling), take(sagaActions.LOBBY_STOP_POLLING)]);
//     }
// }

export function* applyActiveGamesData(action: { payload: NetGameListResponse }) {
    const auth: AuthState = yield select(getAuth);
    const data = action.payload;
    yield put(applyGamesList({ currentUserId: auth.user?.id, gamesEntries: data.gamesEntries }));
}

export function* applyNetGamesData(action: { payload: NetGameListResponse }) {
    const auth: AuthState = yield select(getAuth);
    const data = action.payload;
    yield put(applyNetGamesList({ currentUserId: auth.user?.id, gamesEntries: data.gamesEntries }));
}

export function* applyNetGameData(action: { payload: NetGameInfoResponse }) {
    const auth: AuthState = yield select(getAuth);
    const data = action.payload;
    yield put(applyNetGame({ currentUserId: auth.user?.id, gameInfo: data }));
    if (data.gameId) {
        gameHub.loadGame(data.gameId);
    }
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

export default function* rootSaga() {
    // yield fork(watchLobbyPolling), yield takeEvery(sagaActions.LOBBY_CREATE, errorsWrapper(lobbyCreate));
    yield takeEvery(sagaActions.LOBBY_JOIN, errorsWrapper(lobbyJoin));
    yield takeEvery(sagaActions.LOBBY_GET, errorsWrapper(lobbyGet));
    yield takeEvery(sagaActions.ACTIVE_GAMES_APPLY_DATA, errorsWrapper(applyActiveGamesData));
    yield takeEvery(sagaActions.NET_GAMES_APPLY_DATA, errorsWrapper(applyNetGamesData));
    yield takeEvery(sagaActions.NET_GAME_APPLY_DATA, errorsWrapper(applyNetGameData));
    yield takeEvery(sagaActions.LOBBY_GET_LEADERBOARD, errorsWrapper(getLeaderBoardData));
}
