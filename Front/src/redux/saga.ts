import { call, takeEvery, put } from 'redux-saga/effects';
import axios from 'axios';
import { BaseApi } from '/app/config';

import { initMap } from './gameSlice';
import { GameStartResponse } from './types';

export const sagaActions = {
    GAME_RESET: "GAME_RESET",
    GAME_START: "GAME_START"
};

export function* gameReset() {
  try {
    let result: GameStartResponse = yield call(async () =>
        await axios({ url: `${BaseApi}Game/Reset`, method: 'post'})
    );
    console.log(result);
    // yield put(initMap(result.data.));
  } catch (e) {
    yield put({ type: "TODO_FETCH_FAILED" });
  }
}


export function* gameStart(action: any) {
    try {
      let result: GameStartResponse = yield call(async () =>
          await axios({ url: `${BaseApi}Game/Starting`, method: 'post', data: action.payload})
      );
      console.log(result.data.map);
      yield put(initMap(result.data.map));
    } catch (e) {
      yield put({ type: "TODO_FETCH_FAILED" });
    }
  }
  
export default function* rootSaga() {
  yield takeEvery(sagaActions.GAME_RESET, gameReset);
  yield takeEvery(sagaActions.GAME_START, gameStart);
}