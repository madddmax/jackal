import { call, put, takeEvery } from 'redux-saga/effects';
import { axiosInstance, errorsWrapper, sagaActions } from './constants';
import { history } from '/app/global';
import { AuthResponse, CheckResponse } from '/redux/authSlice.types';
import { setAuth } from '/redux/authSlice';

export function* authCheck(action: any) {
    let result: { data: CheckResponse } = yield call(
        async () =>
            await axiosInstance({
                url: 'v1/auth/check',
                method: 'post',
                data: action.payload,
            }),
    );
    yield put(setAuth(result.data));
}

export function* authLogin(action: any) {
    let result: { data: AuthResponse } = yield call(
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
            user: result.data.user,
            isAuthorised: true,
        }),
    );
}

export function* authLogout(action: any) {
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
