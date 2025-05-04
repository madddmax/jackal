import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Route, Routes } from 'react-router-dom';

import useClientMethod from '../../hubs/useClientMethod';
import useHub from '../../hubs/useHub';
import Logon from './logon';
import MessageNotifier from './messNotifier';
import Newgame from './newgame';
import { debugLog, hubConnection } from '/app/global';
import Login from '/auth/content/login';
import { getAuth } from '/auth/redux/authSlice';
import { getEnableSockets, showMessage } from '/common/redux/commonSlice';
import { sagaActions } from '/common/sagas';
import Playground from '/game/content/playground';
import { initMySettings } from '/game/redux/gameSlice';
import { StorageState } from '/game/types';
import GameList from '/netgame/content/gameList';
import LobbyCard from '/netgame/content/lobbyCard';
import LobbyJoin from '/netgame/content/lobbyJoin';

const Layout = () => {
    const dispatch = useDispatch();
    const enableSockets = useSelector(getEnableSockets);
    const auth = useSelector(getAuth);

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
    useClientMethod(enableSockets, hubConnection, 'GetActiveGames', (data) => {
        debugLog(data);
        dispatch({ type: sagaActions.NET_GAMES_APPLY_DATA, payload: data });
    });
    useHub(enableSockets && auth.isAuthorised === true, hubConnection);

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
                <Route path="/netgame" element={<GameList />}></Route>
                <Route path="/lobby/:id" element={<LobbyCard />}></Route>
                <Route path="/" element={<Playground />}></Route>
            </Routes>
            {auth.isAuthorised === false && <Logon />}
            <MessageNotifier />
        </>
    );
};

export default Layout;
