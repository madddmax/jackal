import { Route, Routes } from 'react-router-dom';
import Newgame from './pages/newgame';
import Playground from './pages/playground';
import { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { StorageState } from '/redux/types';
import { initMySettings } from '/redux/gameSlice';
import LobbyCard from './pages/lobbyCard';

const Layout = () => {
    const dispatch = useDispatch();

    useEffect(() => {
        let myStateStr = localStorage.getItem('state');
        if (myStateStr) {
            let myState: StorageState = JSON.parse(myStateStr);
            if (myState) {
                dispatch(initMySettings(myState));
            }
        }
    }, []);

    return (
        <Routes>
            <Route path="/newgame" element={<Newgame />}></Route>
            <Route path="/lobby/:id" element={<LobbyCard />}></Route>
            <Route path="/" element={<Playground />}></Route>
        </Routes>
    );
};

export default Layout;
