import { fork } from 'redux-saga/effects';
import gameSaga from './gameSaga';
import lobbySaga from './lobbySaga';

export default function* rootSaga() {
    yield fork(gameSaga);
    yield fork(lobbySaga);
}
