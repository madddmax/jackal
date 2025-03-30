import { call, put, takeEvery } from 'redux-saga/effects';

import { CheckMapInfo } from '../../common/redux.types';
import { setMapForecasts, setTilesPackNames } from '../redux/gameSlice';
import { axiosInstance, errorsWrapper, sagaActions } from '/common/sagas';

export function* getTilesPackNames() {
    const result: { data: string[] } = yield call(
        async () =>
            await axiosInstance({
                url: 'v1/map/tiles-pack-names',
                method: 'get',
            }),
    );
    yield put(setTilesPackNames(result.data));
}

export function* checkMap(action: { payload: unknown }) {
    const result: { data: CheckMapInfo[] } = yield call(
        async () =>
            await axiosInstance({
                url: 'v1/map/check-landing',
                method: 'get',
                params: action.payload,
            }),
    );
    yield put(setMapForecasts(result.data.map((it) => it.difficulty)));
}

export default function* rootSaga() {
    yield takeEvery(sagaActions.GET_TILES_PACK_NAMES, errorsWrapper(getTilesPackNames));
    yield takeEvery(sagaActions.CHECK_MAP, errorsWrapper(checkMap));
}
