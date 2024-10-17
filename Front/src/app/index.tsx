import './App.css';
import store from './store';
import { Provider } from 'react-redux';

import Header from '../content/header';
import Layout from '../content/layout';
import NavigateSetter from '../content/layout/navigateSetter';

import 'bootstrap/dist/css/bootstrap.min.css';
import { BrowserRouter } from 'react-router-dom';

function App() {
    return (
        <Provider store={store}>
            <BrowserRouter>
                <NavigateSetter />
                <Header />
                <Layout />
            </BrowserRouter>
        </Provider>
    );
}

export default App;
