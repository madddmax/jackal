import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Route, Routes } from 'react-router-dom';

import useClientMethod from '../../hubs/useClientMethod';
import useHub from '../../hubs/useHub';
import MessageNotifier from './MessageNotifier';
import Newgame from './newgame';
import { debugLog, hubConnection } from '/app/global';
import Login from '/auth/content/login';
import { StorageState } from '/common/redux.types';
import { getEnableSockets, showMessage } from '/common/redux/commonSlice';
import { sagaActions } from '/common/sagas';
import Playground from '/game/content/playground';
import { initMySettings } from '/game/redux/gameSlice';
import LobbyCard from '/netgame/content/lobbyCard';
import LobbyJoin from '/netgame/content/lobbyJoin';

const Layout = () => {
    const dispatch = useDispatch();
    const enableSockets = useSelector(getEnableSockets);

    useClientMethod(enableSockets, hubConnection, 'Notify', (text) => {
        dispatch(
            showMessage({
                isError: false,
                errorCode: 'success',
                messageText: JSON.stringify(text),
            }),
        );
    });
    useClientMethod(enableSockets, hubConnection, 'GetStartData', (data) => {
        debugLog(data);
        dispatch({ type: sagaActions.GAME_START_APPLY_DATA, payload: data });
    });
    useClientMethod(enableSockets, hubConnection, 'GetMoveChanges', (data) => {
        debugLog(data);
        dispatch({ type: sagaActions.GAME_TURN_APPLY_DATA, payload: data });
    });
    useHub(enableSockets, hubConnection);

    useEffect(() => {
        const myStateStr = localStorage.getItem('state');
        if (myStateStr) {
            const myState: StorageState = JSON.parse(myStateStr);
            if (myState) {
                dispatch(initMySettings(myState));
            }
        }

        dispatch({ type: sagaActions.GET_TILES_PACK_NAMES, payload: {} });
        dispatch({ type: sagaActions.AUTH_CHECK, payload: {} });
    }, [dispatch]);

    return (
        <>
            <Routes>
                <Route path="/newgame" element={<Newgame />}></Route>
                <Route path="/login" element={<Login />}></Route>
                <Route path="/joinlobby" element={<LobbyJoin />}></Route>
                <Route path="/lobby/:id" element={<LobbyCard />}></Route>
                <Route path="/" element={<Playground />}></Route>
            </Routes>
            <MessageNotifier />
        </>
    );
};

export default Layout;
