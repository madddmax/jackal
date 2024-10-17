import { call, put, takeEvery } from 'redux-saga/effects';
import { errorsWrapper, sagaActions } from './constants';
import { history } from '/app/global';
import axios from 'axios';
import config from '/app/config';
import { LobbyCreateResponse, LobbyJoinResponse } from '/redux/types';
import { updateLobby } from '/redux/lobbySlice';

export function* lobbyCreate(action: any) {
    let result: { data: LobbyCreateResponse } = yield call(
        async () =>
            await axios({
                url: `${config.BaseApi}v1/lobby/create-lobby`,
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
            await axios({
                url: `${config.BaseApi}v1/lobby/join-lobby`,
                method: 'post',
                data: action.payload,
            }),
    );
    yield put(updateLobby(result.data.lobby));
    history.navigate && history.navigate('/lobby/' + result.data.lobby.id);
}

export default function* rootSaga() {
    yield takeEvery(sagaActions.LOBBY_CREATE, errorsWrapper(lobbyCreate));
    yield takeEvery(sagaActions.LOBBY_JOIN, errorsWrapper(lobbyJoin));
}
