import { PayloadAction } from '@reduxjs/toolkit';
import { call, put, takeEvery } from 'redux-saga/effects';

import { checkAuth, setAuth } from '../redux/authSlice';
import { AuthLoginRequest, AuthResponse, CheckResponse } from '../types/authSaga';
import { history } from '/app/global';
import { axiosInstance, errorsWrapper, sagaActions } from '/common/sagas';

export function* authCheck(action: PayloadAction<object>) {
    const result: { data: CheckResponse } = yield call(
        async () =>
            await axiosInstance({
                url: 'v1/auth/check',
                method: 'post',
                data: action.payload,
            }),
    );
    yield put(
        checkAuth({
            user: result.data.user,
            isAuthorised: !!result.data.user,
        }),
    );
}

export function* authLogin(action: { payload: AuthLoginRequest }) {
    const result: { data: AuthResponse } = yield call(
        async () =>
            await axiosInstance({
                url: 'v1/auth/register',
                method: 'post',
                data: action.payload,
            }),
    );
    history.navigate && history.navigate('/');
    yield put(
        setAuth({
            token: result.data.token,
            user: result.data.user,
            isAuthorised: true,
        }),
    );
}

export function* authLogout(action: { payload: object }) {
    yield call(
        async () =>
            await axiosInstance({
                url: 'v1/auth/logout',
                method: 'post',
                data: action.payload,
            }),
    );
    yield put(
        setAuth({
            isAuthorised: false,
        }),
    );
}

export default function* rootSaga() {
    yield takeEvery(sagaActions.AUTH_CHECK, errorsWrapper(authCheck));
    yield takeEvery(sagaActions.AUTH_LOGIN, errorsWrapper(authLogin));
    yield takeEvery(sagaActions.AUTH_LOGOUT, errorsWrapper(authLogout));
}
