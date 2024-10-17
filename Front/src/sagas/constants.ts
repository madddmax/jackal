import axios from 'axios';
import { call, put } from 'redux-saga/effects';
import { ErrorInfo } from '/redux/commonSlice.d';
import { showError } from '/redux/commonSlice';
import { debugLog } from '/app/global';

export const sagaActions = {
    GAME_RESET: 'GAME_RESET',
    GAME_START: 'GAME_START',
    GAME_TURN: 'GAME_TURN',

    LOBBY_CREATE: 'LOBBY_CREATE',
    LOBBY_JOIN: 'LOBBY_JOIN',
};

export const errorsWrapper = (saga: (action: any) => void) =>
    function* (action: any) {
        try {
            yield call(saga, action);
        } catch (err) {
            if (axios.isAxiosError(err)) {
                let error = err.response?.data as ErrorInfo;
                if (error) {
                    yield put(showError(error));
                } else {
                    yield put(
                        showError({
                            error: true,
                            errorCode: 'InternalServerError',
                            errorMessage: 'Ошибка сервера',
                        }),
                    );
                }
            } else {
                debugLog(err);
            }
        }
    };
