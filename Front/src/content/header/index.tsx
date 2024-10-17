import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';

import { useDispatch, useSelector } from 'react-redux';
import { sagaActions } from '/sagas/constants';
import './header.css';
import { Link } from 'react-router-dom';
import { uuidGen } from '/app/global';
import { ReduxState, StorageState } from '/redux/types';
import config from '/app/config';

function Header() {
    const dispatch = useDispatch();

    const userSettings = useSelector<ReduxState, StorageState>((state) => state.game.userSettings);

    const quickStart = () =>
        dispatch({
            type: sagaActions.GAME_START,
            payload: {
                gameName: uuidGen(),
                settings: {
                    players: ['human', 'robot2'],
                    mapId: userSettings.mapId,
                    mapSize: 11,
                },
            },
        });

    return (
        <Navbar bg="light" data-bs-theme="light" className="header">
            <Container>
                <Navbar.Brand>
                    <Nav.Link as={Link} to="/">
                        React-Jackal
                    </Nav.Link>
                </Navbar.Brand>
                <Navbar.Toggle aria-controls="basic-navbar-nav" />
                <Navbar.Collapse id="basic-navbar-nav" className="ms-3">
                    <Nav className="me-auto">
                        <Nav.Link as={Link} to="/" onClick={quickStart}>
                            Быстрый старт
                        </Nav.Link>
                        <Nav.Link as={Link} to="/newgame">
                            Новая игра
                        </Nav.Link>
                        <Nav.Link as={Link} to="/joinlobby">
                            Лобби
                        </Nav.Link>
                    </Nav>
                </Navbar.Collapse>
                {process.env.NODE_ENV && process.env.NODE_ENV === 'development' && (
                    <Navbar.Collapse id="basic-navbar-nav" className="d-flex">
                        <Nav className="me-auto">
                            <Nav.Link
                                as={Link}
                                to={`${config.BaseApi.substring(0, config.BaseApi.length - 4)}swagger`}
                                target="_blank"
                            >
                                Swagger
                            </Nav.Link>
                        </Nav>
                    </Navbar.Collapse>
                )}
            </Container>
        </Navbar>
    );
}

export default Header;
