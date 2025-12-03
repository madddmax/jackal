import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import { FaBookSkull, FaGamepad, FaNetworkWired } from 'react-icons/fa6';
import { GiWingfoot } from 'react-icons/gi';
import { HiLogin, HiLogout } from 'react-icons/hi';
import { ImFire } from 'react-icons/im';
import { MdWaterDrop } from 'react-icons/md';
import { useDispatch, useSelector } from 'react-redux';
import { Link } from 'react-router-dom';

import { activateSockets, getEnableSockets } from '../../../common/redux/commonSlice';
import './header.css';
import config from '/app/config';
import { Constants } from '/app/constants';
import { getAuth } from '/auth/redux/authSlice';
import { sagaActions } from '/common/sagas';
import gameHub from '/game/hub/gameHub';
import { getUserSettings } from '/game/redux/gameSlice';

const Header = () => {
    const dispatch = useDispatch();

    const userSettings = useSelector(getUserSettings);
    const authInfo = useSelector(getAuth);
    const enableSockets = useSelector(getEnableSockets);

    const quickStart = () => {
        gameHub.startGame({
            players: [
                { userId: authInfo.user?.id ?? 0, type: 'human', position: Constants.positions[0] },
                { userId: 0, type: 'robot2', position: Constants.positions[2] },
            ],
            mapId: userSettings.mapId,
            mapSize: 11,
            tilesPackName: userSettings.tilesPackName,
        });
    };

    const doLogout = () =>
        dispatch({
            type: sagaActions.AUTH_LOGOUT,
            payload: {},
        });

    const useSocketsToggle = () => dispatch(activateSockets(!enableSockets));

    return (
        <Navbar bg="light" data-bs-theme="light" className="header p-0">
            <Container>
                <Navbar.Brand>
                    <Nav.Link as={Link} to="/">
                        <img
                            alt=""
                            src="/pictures/girls/logo.png"
                            width="40"
                            height="40"
                            className="d-inline-block align-top me-2"
                        />
                        <span className="align-middle">React-Jackal</span>
                    </Nav.Link>
                </Navbar.Brand>
                <Navbar.Toggle aria-controls="basic-navbar-nav" />
                <Navbar.Collapse id="basic-navbar-nav" className="ms-3">
                    <Nav className="me-auto">
                        <Nav.Link as={Link} to="/" onClick={quickStart}>
                            <GiWingfoot size={20} className="d-block mx-auto" />
                            Быстрый старт
                        </Nav.Link>
                        <Nav.Link as={Link} to="/newgame">
                            <FaGamepad size={20} className="d-block mx-auto" />
                            Новая игра
                        </Nav.Link>
                        <Nav.Link as={Link} to="/netgame">
                            <FaNetworkWired size={20} className="d-block mx-auto" />
                            Лобби
                        </Nav.Link>
                        <Nav.Link as={Link} to="/docs">
                            <FaBookSkull size={20} className="d-block mx-auto" />
                            Обучение
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
                        <Nav.Item>
                            <Nav.Link as={Link} to="/" onClick={useSocketsToggle}>
                                {enableSockets ? <ImFire /> : <MdWaterDrop />}
                            </Nav.Link>
                        </Nav.Item>
                    </Nav>
                </Navbar.Collapse>
                <Navbar.Toggle />
                <Navbar.Collapse className="justify-content-end">
                    {authInfo.isAuthorised && (
                        <Nav style={{ marginRight: '5px' }}>
                            <div className="login-wrapper">
                                <img src={`ranks/${authInfo.user?.rank}.webp`} alt={authInfo.user?.rank} />
                            </div>
                        </Nav>
                    )}
                    <Nav className="me-auto">
                        <Navbar.Text>
                            {authInfo.isAuthorised ? (
                                <div className="login-wrapper">
                                    <span className="login-text">{authInfo.user?.login}</span>
                                </div>
                            ) : (
                                <span style={{ color: 'red' }}>Не авторизован</span>
                            )}
                        </Navbar.Text>
                    </Nav>
                    <Nav className="me-auto">
                        {authInfo.isAuthorised ? (
                            <Nav.Item>
                                <Nav.Link as={Link} to="/" onClick={doLogout}>
                                    <HiLogout size={20} className="d-block mx-auto" />
                                    Выйти
                                </Nav.Link>
                            </Nav.Item>
                        ) : (
                            <Nav.Item>
                                <Nav.Link as={Link} to="/login">
                                    <HiLogin size={20} className="d-block mx-auto" />
                                    Войти
                                </Nav.Link>
                            </Nav.Item>
                        )}
                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
};

export default Header;
