import { call, delay, fork, put, race, select, take, takeEvery } from 'redux-saga/effects';
import { axiosInstance, errorsWrapper, sagaActions } from './constants';
import { history } from '/app/global';
import { LobbyCreateResponse, LobbyGetResponse, LobbyInfo, LobbyJoinResponse } from '/redux/types';
import { updateLobby } from '/redux/lobbySlice';

export function* lobbyCreate(action: any) {
    let result: { data: LobbyCreateResponse } = yield call(
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

export function* lobbyJoin(action: any) {
    let result: { data: LobbyJoinResponse } = yield call(
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

export function* lobbyGet(action: any) {
    let result: { data: LobbyGetResponse } = yield call(
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

function* lobbyPolling() {
    yield delay(3000);
    const lobby: LobbyInfo = yield select((state) => state.lobby.lobby);
    if (!lobby) {
        yield put({ type: sagaActions.LOBBY_STOP_POLLING });
        return;
    }

    yield put({
        type: sagaActions.LOBBY_GET,
        payload: {
            lobbyId: lobby.id,
        },
    });
}

function* watchLobbyPolling() {
    while (true) {
        yield take([sagaActions.LOBBY_DO_POLLING]);
        yield race([call(lobbyPolling), take(sagaActions.LOBBY_STOP_POLLING)]);
    }
}

export default function* rootSaga() {
    yield fork(watchLobbyPolling), yield takeEvery(sagaActions.LOBBY_CREATE, errorsWrapper(lobbyCreate));
    yield takeEvery(sagaActions.LOBBY_JOIN, errorsWrapper(lobbyJoin));
    yield takeEvery(sagaActions.LOBBY_GET, errorsWrapper(lobbyGet));
}
