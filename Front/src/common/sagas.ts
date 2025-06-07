import { PayloadAction } from '@reduxjs/toolkit';
import axios from 'axios';
import { call, put } from 'redux-saga/effects';

import { setAuth } from '../auth/redux/authSlice';
import { showMessage } from './redux/commonSlice';
import { ErrorInfo } from './redux/commonSlice.types';
import config from '/app/config';
import { debugLog } from '/app/global';

export const sagaActions = {
    GAME_START_APPLY_DATA: 'GAME_START_APPLY_DATA',
    GAME_TURN_APPLY_DATA: 'GAME_TURN_APPLY_DATA',
    GAME_START_LOOKING_DATA: 'GAME_START_LOOKING_DATA',

    START_ANIMATE: 'START_ANIMATE',
    STOP_ANIMATE: 'STOP_ANIMATE',
    GET_TILES_PACK_NAMES: 'GET_TILES_PACK_NAMES',
    CHECK_MAP: 'CHECK_MAP',

    LOBBY_CREATE: 'LOBBY_CREATE',
    LOBBY_JOIN: 'LOBBY_JOIN',
    LOBBY_GET: 'LOBBY_GET',
    LOBBY_DO_POLLING: 'LOBBY_DO_POLLING',
    LOBBY_STOP_POLLING: 'LOBBY_STOP_POLLING',

    NET_GAME_CREATE: 'NET_GAME_CREATE',
    NET_GAME_APPLY_DATA: 'NET_GAME_APPLY_DATA',
    ACTIVE_GAMES_APPLY_DATA: 'ACTIVE_GAMES_APPLY_DATA',
    NET_GAMES_APPLY_DATA: 'NET_GAMES_APPLY_DATA',

    AUTH_CHECK: 'AUTH_CHECK',
    AUTH_LOGIN: 'AUTH_LOGIN',
    AUTH_LOGOUT: 'AUTH_LOGOUT',
};

export const axiosInstance = axios.create({
    baseURL: config.BaseApi,
    headers: {
        Authorization: `Bearer ${localStorage.auth}`,
    },
});

export const errorsWrapper = <T>(saga: (action: PayloadAction<T>) => void) =>
    function* (action: PayloadAction<T>) {
        try {
            yield call(saga, action);
        } catch (err) {
            if (axios.isAxiosError(err)) {
                const error = err.response?.data as ErrorInfo;

                debugLog(error, err);

                if (error) {
                    yield put(
                        showMessage({
                            isError: true,
                            errorCode: error.errorCode,
                            messageText: error.errorMessage,
                        }),
                    );
                } else if (err.response?.status == 401) {
                    yield put(
                        setAuth({
                            isAuthorised: false,
                        }),
                    );
                    yield put(
                        showMessage({
                            isError: true,
                            errorCode: err.response?.statusText,
                            messageText: 'Не авторизован',
                        }),
                    );
                } else {
                    yield put(
                        showMessage({
                            isError: true,
                            errorCode: 'InternalServerError',
                            messageText: 'Ошибка сервера',
                        }),
                    );
                }
            } else {
                debugLog(err);
            }
        }
    };
