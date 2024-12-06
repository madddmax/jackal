import { fork } from 'redux-saga/effects';
import authSaga from './authSaga';
import gameSaga from './gameSaga';
import gameFeaturesSaga from './gameFeaturesSaga';
import lobbySaga from './lobbySaga';

export default function* rootSaga() {
    yield fork(authSaga);
    yield fork(gameSaga);
    yield fork(gameFeaturesSaga);
    yield fork(lobbySaga);
}
