import { configureStore } from '@reduxjs/toolkit';
import createSagaMiddleware from 'redux-saga';

import authReducer from '../auth/redux/authSlice';
import commonReducer from '../common/redux/commonSlice';
import gameReducer from '../game/redux/gameSlice';
import lobbyReducer from '../netgame/redux/lobbySlice';
import saga from './sagas';

const sagaMiddleware = createSagaMiddleware();

const store = configureStore({
    reducer: {
        auth: authReducer,
        common: commonReducer,
        game: gameReducer,
        lobby: lobbyReducer,
    },
    middleware: (getDefaultMiddleware) => getDefaultMiddleware({ thunk: false }).concat(sagaMiddleware),
});

sagaMiddleware.run(saga);

export default store;
