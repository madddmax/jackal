import { fork } from 'redux-saga/effects';
import authSaga from '/auth/sagas/authSaga';
import gameSaga from '/game/sagas/gameSaga';
import gameFeaturesSaga from '/game/sagas/gameFeaturesSaga';
import lobbySaga from '/netgame/sagas/lobbySaga';

export default function* rootSaga() {
    yield fork(authSaga);
    yield fork(gameSaga);
    yield fork(gameFeaturesSaga);
    yield fork(lobbySaga);
}
