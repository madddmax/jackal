import { PayloadAction } from '@reduxjs/toolkit';
//import { call, delay, fork, put, race, select, take, takeEvery } from 'redux-saga/effects';
import { call, put, takeEvery } from 'redux-saga/effects';

//import { LobbyCreateResponse, LobbyGetResponse, LobbyInfo, LobbyJoinResponse, NetGameListResponse } from '../../common/redux.types';
import {
    LobbyCreateResponse,
    LobbyGetResponse,
    LobbyJoinResponse,
    NetGameInfo,
    NetGameListResponse,
} from '../../common/redux.types';
import { applyGamesList, applyNetGame, applyNetGamesList, updateLobby } from '../redux/lobbySlice';
import { history } from '/app/global';
import { axiosInstance, errorsWrapper, sagaActions } from '/common/sagas';

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
    const data = action.payload;
    yield put(applyGamesList(data));
}

export function* applyNetGamesData(action: { payload: NetGameListResponse }) {
    const data = action.payload;
    yield put(applyNetGamesList(data));
}

export function* applyNetGameData(action: { payload: NetGameInfo }) {
    const data = action.payload;
    yield put(applyNetGame(data));
}

export default function* rootSaga() {
    // yield fork(watchLobbyPolling), yield takeEvery(sagaActions.LOBBY_CREATE, errorsWrapper(lobbyCreate));
    yield takeEvery(sagaActions.LOBBY_JOIN, errorsWrapper(lobbyJoin));
    yield takeEvery(sagaActions.LOBBY_GET, errorsWrapper(lobbyGet));
    yield takeEvery(sagaActions.ACTIVE_GAMES_APPLY_DATA, errorsWrapper(applyActiveGamesData));
    yield takeEvery(sagaActions.NET_GAMES_APPLY_DATA, errorsWrapper(applyNetGamesData));
    yield takeEvery(sagaActions.NET_GAME_APPLY_DATA, errorsWrapper(applyNetGameData));
}
