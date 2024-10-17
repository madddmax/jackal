import { configureStore } from '@reduxjs/toolkit';
import createSagaMiddleware from 'redux-saga';

import commonReducer from '../redux/commonSlice';
import gameReducer from '../redux/gameSlice';
import lobbyReducer from '../redux/lobbySlice';
import saga from '../sagas';

let sagaMiddleware = createSagaMiddleware();

const store = configureStore({
    reducer: {
        common: commonReducer,
        game: gameReducer,
        lobby: lobbyReducer,
    },
    middleware: (getDefaultMiddleware) => getDefaultMiddleware({ thunk: false }).concat(sagaMiddleware),
});

sagaMiddleware.run(saga);

export default store;
