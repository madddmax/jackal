import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Navigate, Outlet, Route, Routes, useLocation } from 'react-router-dom';

import Rating from '../../../rating/content';
import GameDocuments from '../../../docs/content';
import useClientMethod from '../../hubs/useClientMethod';
import useHub from '../../hubs/useHub';
import MessageNotifier from './messNotifier';
import Newgame from './newgame';
import Quickstart from './quickstart';
import Madstart from './madstart';
import { hubConnection } from '/app/global';
import useClientMethods from '/app/hubs/useClientMethods';
import Login from '/auth/content/login';
import { getAuth } from '/auth/redux/authSlice';
import { getEnableSockets, showMessage } from '/common/redux/commonSlice';
import { sagaActions } from '/common/sagas';
import Playground from '/game/content/playground';
import gameHub from '/game/hub/gameHub';
import { initMySettings } from '/game/redux/gameSlice';
import { BrowserStorage } from '/game/types';
import NetGameCreate from '/lobby/content/gameCreate';
import GameList from '/lobby/content/gameList';

const ProtectedRoute = () => {
    const auth = useSelector(getAuth);
    const location = useLocation();

    // Redirect to login, storing the original target location in state
    if (auth.isAuthorised === false) {
        return <Navigate to="/login" state={{ from: location }} replace />;
    }

    // If logged in, render the child routes
    return <Outlet />;
};

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
    useClientMethods(enableSockets, hubConnection, dispatch, gameHub.getEventHandlers);
    useHub(enableSockets && auth.isAuthorised === true, hubConnection);

    useEffect(() => {
        const myStateStr = localStorage.getItem('state');
        if (myStateStr) {
            const myState: BrowserStorage = JSON.parse(myStateStr);
            if (myState) {
                dispatch(initMySettings(myState));
            }
        }

        dispatch({ type: sagaActions.GET_TILES_PACK_NAMES, payload: {} });
        dispatch({ type: sagaActions.LOBBY_GET_LEADERBOARD, payload: {} });
        dispatch({ type: sagaActions.LOBBY_GET_NET_LEADERBOARD, payload: {} });
        dispatch({ type: sagaActions.LOBBY_GET_BOT_LEADERBOARD, payload: {} });
        dispatch({ type: sagaActions.AUTH_CHECK, payload: {} });

        const intervalId = setInterval(() => {
            dispatch({ type: sagaActions.LOBBY_GET_LEADERBOARD, payload: {} });
            dispatch({ type: sagaActions.LOBBY_GET_NET_LEADERBOARD, payload: {} });
            dispatch({ type: sagaActions.LOBBY_GET_BOT_LEADERBOARD, payload: {} });
            dispatch({ type: sagaActions.AUTH_CHECK, payload: {} });
        }, 300000); // 5 minutes in ms

        return () => {
            clearInterval(intervalId);
        };
    }, [dispatch]);

    return (
        <>
            <Routes>
                {/* Public Routes */}
                <Route path="/rating" element={<Rating />} />
                <Route path="/docs/:tabId" element={<GameDocuments />} />
                <Route path="/docs" element={<Navigate to="/docs/rules" replace />} />
                <Route path="/login" element={<Login />}></Route>

                {/* Protected Routes Wrapper */}
                <Route element={<ProtectedRoute />}>
                    <Route path="/quickstart" element={<Quickstart />}></Route>
                    <Route path="/madstart" element={<Madstart />}></Route>
                    <Route path="/newgame" element={<Newgame />}></Route>
                    <Route path="/newpublic" element={<NetGameCreate />}></Route>
                    <Route path="/netgame" element={<GameList />}></Route>
                    <Route path="/" element={<Playground />}></Route>
                </Route>
            </Routes>
            <MessageNotifier />
        </>
    );
};

export default Layout;
