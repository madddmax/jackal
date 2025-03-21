import { Route, Routes } from 'react-router-dom';
import Newgame from './newgame';
import Login from '/auth/content/login';
import Playground from '/game/content/playground';
import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ReduxState, StorageState } from '/common/redux.types';
import { initMySettings } from '/game/redux/gameSlice';
import LobbyCard from '/netgame/content/lobbyCard';
import LobbyJoin from '/netgame/content/lobbyJoin';
import MessageNotifier from './MessageNotifier';
import { sagaActions } from '/common/sagas';
import useHub from '../../hubs/useHub';
import useClientMethod from '../../hubs/useClientMethod';
import { showMessage } from '/common/redux/commonSlice';
import { debugLog, hubConnection } from '/app/global';

const Layout = () => {
    const dispatch = useDispatch();
    const enableSockets = useSelector<ReduxState, boolean>((state) => state.common.enableSockets);

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
        let myStateStr = localStorage.getItem('state');
        if (myStateStr) {
            let myState: StorageState = JSON.parse(myStateStr);
            if (myState) {
                dispatch(initMySettings(myState));
            }
        }

        dispatch({ type: sagaActions.GET_TILES_PACK_NAMES, payload: {} });
        dispatch({ type: sagaActions.AUTH_CHECK, payload: {} });
    }, []);

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
