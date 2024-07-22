import { Route, Routes } from 'react-router-dom';
import Newgame from './pages/newgame';
import Playground from './pages/playground';

function Layout() {
    return (
        <Routes>
            <Route path="/newgame" element={<Newgame />}></Route>
            <Route path="/" element={<Playground />}></Route>
        </Routes>
    );
}

export default Layout;
