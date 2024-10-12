import { configureStore } from '@reduxjs/toolkit';
import createSagaMiddleware from 'redux-saga';

import gameReducer from '../redux/gameSlice';
import lobbyReducer from '../redux/lobbySlice';
import saga from '../sagas';

let sagaMiddleware = createSagaMiddleware();

const store = configureStore({
    reducer: {
        game: gameReducer,
        lobby: lobbyReducer,
    },
    middleware: (getDefaultMiddleware) => getDefaultMiddleware({ thunk: false }).concat(sagaMiddleware),
});

sagaMiddleware.run(saga);

export default store;
