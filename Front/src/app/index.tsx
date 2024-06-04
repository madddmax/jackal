import './App.css'
import store from './store'
import { Provider } from 'react-redux'

import Header from '../content/header';
import Layout from '../content/layout';

import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
  return (
    <Provider store={store}>
      <Header />
      <Layout />
    </Provider>
  )
}

export default App
