import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';

import { useDispatch, useSelector } from 'react-redux';
import { sagaActions } from '/common/sagas';
import './header.css';
import { Link } from 'react-router-dom';
import { debugLog, hubConnection, uuidGen } from '/app/global';
import { ReduxState, StorageState } from '../../../common/redux.types';
import config from '/app/config';
import { AuthState } from '../../../auth/redux/authSlice.types';
import { HiLogin, HiLogout } from 'react-icons/hi';
import { Constants } from '/app/constants';
import { ImFire } from 'react-icons/im';
import { MdWaterDrop } from 'react-icons/md';
import { activateSockets } from '../../../common/redux/commonSlice';

const Header = () => {
    const dispatch = useDispatch();

    const userSettings = useSelector<ReduxState, StorageState>((state) => state.game.userSettings);
    const authInfo = useSelector<ReduxState, AuthState>((state) => state.auth);
    const enableSockets = useSelector<ReduxState, boolean>((state) => state.common.enableSockets);

    const quickStart = () => {
        hubConnection
            .invoke('start', {
                gameName: uuidGen(),
                settings: {
                    players: [
                        { userId: 0, type: 'human', position: Constants.positions[0] },
                        { userId: 0, type: 'robot2', position: Constants.positions[2] },
                    ],
                    mapId: userSettings.mapId,
                    mapSize: 11,
                    tilesPackName: userSettings.tilesPackName,
                },
            })
            .catch((err) => {
                debugLog(err);
            });
    };

    const doLogout = () =>
        dispatch({
            type: sagaActions.AUTH_LOGOUT,
            payload: {},
        });

    const useSocketsToggle = () => dispatch(activateSockets(!enableSockets));

    return (
        <Navbar bg="light" data-bs-theme="light" className="header">
            <Container>
                <Navbar.Brand>
                    <Nav.Link as={Link} to="/">
                        <img
                            alt=""
                            src="/pictures/girls/logo.png"
                            width="30"
                            height="30"
                            className="d-inline-block align-top me-2"
                        />
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
                    <>
                        <Navbar.Toggle aria-controls="basic-navbar-nav" />
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
                    </>
                )}
                <Navbar.Toggle aria-controls="basic-navbar-nav" />
                <Navbar.Collapse id="basic-navbar-nav" className="d-flex">
                    <Nav className="me-auto">
                        <Nav.Link as={Link} to="/" onClick={useSocketsToggle}>
                            {enableSockets ? <ImFire /> : <MdWaterDrop />}
                        </Nav.Link>
                    </Nav>
                </Navbar.Collapse>
                <Navbar.Toggle />
                <Navbar.Collapse className="justify-content-end">
                    <Nav className="me-auto">
                        <Navbar.Text>
                            {authInfo.isAuthorised ? (
                                <span style={{ color: 'dark-red' }}>{authInfo.user?.login}</span>
                            ) : (
                                <span style={{ color: 'red' }}>Не авторизован</span>
                            )}
                        </Navbar.Text>
                        {authInfo.isAuthorised ? (
                            <Nav.Link as={Link} to="/" onClick={doLogout}>
                                <HiLogout />
                            </Nav.Link>
                        ) : (
                            <Nav.Link as={Link} to="/login">
                                <HiLogin />
                            </Nav.Link>
                        )}
                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
};

export default Header;
