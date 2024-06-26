import { configureStore } from '@reduxjs/toolkit'
import createSagaMiddleware from "redux-saga";

import gameReducer from '../redux/gameSlice'
import saga from "../redux/saga";

let sagaMiddleware = createSagaMiddleware();

const store = configureStore({
  reducer: {
    game: gameReducer
  },
  middleware: (getDefaultMiddleware) => getDefaultMiddleware({ thunk: false }).concat(sagaMiddleware),
});

sagaMiddleware.run(saga);

export default store;