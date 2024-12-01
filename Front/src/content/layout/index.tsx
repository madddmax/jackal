import { Route, Routes } from 'react-router-dom';
import Newgame from './pages/newgame';
import Login from './pages/login';
import Playground from './pages/playground';
import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ReduxState, StorageState } from '/redux/types';
import { initMySettings } from '/redux/gameSlice';
import LobbyCard from './pages/lobbyCard';
import LobbyJoin from './pages/lobbyJoin';
import MessageNotifier from './MessageNotifier';
import { sagaActions } from '/sagas/constants';
import useHub from '/hubs/useHub';
import useClientMethod from '/hubs/useClientMethod';
import { showMessage } from '/redux/commonSlice';
import { debugLog, hubConnection } from '/app/global';

const Layout = () => {
    const dispatch = useDispatch();
    const useSockets = useSelector<ReduxState, boolean>((state) => state.common.useSockets);

    useClientMethod(useSockets, hubConnection, 'Notify', (text) => {
        dispatch(
            showMessage({
                isError: false,
                errorCode: 'success',
                messageText: JSON.stringify(text),
            }),
        );
    });
    useClientMethod(useSockets, hubConnection, 'GetStartData', (data) => {
        debugLog(data);
        dispatch({ type: sagaActions.GAME_START_APPLY_DATA, payload: data });
    });
    useClientMethod(useSockets, hubConnection, 'GetMoveChanges', (data) => {
        debugLog(data);
        dispatch({ type: sagaActions.GAME_TURN_APPLY_DATA, payload: data });
    });
    useHub(useSockets, hubConnection);

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
