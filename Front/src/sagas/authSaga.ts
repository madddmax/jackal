import { call, put, takeEvery } from 'redux-saga/effects';
import { errorsWrapper, sagaActions } from './constants';
import axios from 'axios';
import config from '/app/config';
import { CheckResponse } from '/redux/authSlice.types';
import { setAuth } from '/redux/authSlice';

export function* authCheck(action: any) {
    let result: { data: CheckResponse } = yield call(
        async () =>
            await axios({
                url: `${config.BaseApi}v1/auth/check`,
                method: 'post',
                data: action.payload,
            }),
    );
    yield put(setAuth(result.data));
}

export default function* rootSaga() {
    yield takeEvery(sagaActions.AUTH_CHECK, errorsWrapper(authCheck));
}
