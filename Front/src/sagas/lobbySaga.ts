import { call, put, takeEvery } from 'redux-saga/effects';
import { sagaActions } from './constants';
import axios from 'axios';
import config from '/app/config';
import { LobbyCreateResponse } from '/redux/types';
import { addLobby } from '/redux/lobbySlice';

export function* lobbyCreate(action: any) {
    try {
        let result: { data: LobbyCreateResponse } = yield call(
            async () =>
                await axios({
                    url: `${config.BaseApi}v1/lobby/create-lobby`,
                    method: 'post',
                    data: action.payload,
                }),
        );
        yield put(addLobby(result.data.lobby));
    } catch (e) {
        yield put({ type: 'TODO_CREATE_FAILED' });
    }
}

export default function* rootSaga() {
    yield takeEvery(sagaActions.LOBBY_CREATE, lobbyCreate);
}
