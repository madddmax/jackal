import { call, takeEvery, put } from 'redux-saga/effects';
import axios from 'axios';
import {BaseApi } from '/app/config';
import {initMap, highlightMoves } from './gameSlice';
import { GameStartResponse, GameTurnResponse } from './types';

export const sagaActions = {
    GAME_RESET: "GAME_RESET",
    GAME_START: "GAME_START",
    GAME_TURN: "GAME_TURN"
};

export function* gameReset() {
  try {
    const s = 0;
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
          await axios({ url: `${BaseApi}Game/MakeStart`, method: 'post', data: action.payload})
      );
      console.log(result.data.map);
      yield put(initMap(result.data.map));

      yield call(gameTurn, { type: sagaActions.GAME_TURN, payload: { gameName:	"afc9847e-dce9-497d-bac8-767c3d571b48"} });

    } catch (e) {
      yield put({ type: "TODO_FETCH_FAILED" });
    }
  }

  
export function* gameTurn(action: any) {

  let mustContinue = true;
  while (mustContinue) {
    mustContinue = yield call(oneTurn, action);
    console.log('mustContinue', mustContinue);
  }
}


export function* oneTurn(action: any) {
  try {
    let result: GameTurnResponse = yield call(async () =>
        await axios({ url: `${BaseApi}Game/MakeTurn`, method: 'post', data: action.payload})
    );

    if (result.data.stat.IsHumanPlayer) {
      yield put(highlightMoves(result.data.moves));
      return false;
    }
    return true;
  } catch (e) {
    yield put({ type: "TODO_FETCH_FAILED" });
  }
}


export default function* rootSaga() {
  yield takeEvery(sagaActions.GAME_RESET, gameReset);
  yield takeEvery(sagaActions.GAME_START, gameStart);
  yield takeEvery(sagaActions.GAME_TURN, gameTurn);
}