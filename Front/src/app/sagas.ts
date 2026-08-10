import { fork } from 'redux-saga/effects';

import authSaga from '/auth/sagas/authSaga';
import gameFeaturesSaga from '/game/sagas/gameFeaturesSaga';
import gameSaga from '/game/sagas/gameSaga';
import lobbySaga from '/lobby/sagas/lobbySaga';
import ratingSaga from '/rating/sagas/ratingSaga';

export default function* rootSaga() {
    yield fork(authSaga);
    yield fork(gameSaga);
    yield fork(gameFeaturesSaga);
    yield fork(lobbySaga);
    yield fork(ratingSaga);
}
