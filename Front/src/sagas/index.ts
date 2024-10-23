import { fork } from 'redux-saga/effects';
import authSaga from './authSaga';
import gameSaga from './gameSaga';
import lobbySaga from './lobbySaga';

export default function* rootSaga() {
    yield fork(authSaga);
    yield fork(gameSaga);
    yield fork(lobbySaga);
}
