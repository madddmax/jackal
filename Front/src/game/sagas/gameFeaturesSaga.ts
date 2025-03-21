import { call, put, takeEvery } from 'redux-saga/effects';
import { axiosInstance, errorsWrapper, sagaActions } from '/common/sagas';
import { setMapInfo, setTilesPackNames } from '../redux/gameSlice';
import { CheckMapInfo } from '../../common/redux.types';

export function* getTilesPackNames() {
    let result: { data: string[] } = yield call(
        async () =>
            await axiosInstance({
                url: 'v1/map/tiles-pack-names',
                method: 'get',
            }),
    );
    yield put(setTilesPackNames(result.data));
}

export function* checkMap(action: any) {
    let result: { data: CheckMapInfo[] } = yield call(
        async () =>
            await axiosInstance({
                url: 'v1/map/check-landing',
                method: 'get',
                params: action.payload,
            }),
    );
    yield put(setMapInfo(result.data.map((it) => it.difficulty)));
}

export default function* rootSaga() {
    yield takeEvery(sagaActions.GET_TILES_PACK_NAMES, errorsWrapper(getTilesPackNames));
    yield takeEvery(sagaActions.CHECK_MAP, errorsWrapper(checkMap));
}
