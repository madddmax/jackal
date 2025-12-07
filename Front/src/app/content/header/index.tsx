import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import Offcanvas from 'react-bootstrap/esm/Offcanvas';
import { FaBookSkull, FaGamepad, FaNetworkWired } from 'react-icons/fa6';
import { GiWingfoot } from 'react-icons/gi';
import { HiLogin, HiLogout } from 'react-icons/hi';
import { ImFire } from 'react-icons/im';
import { MdWaterDrop } from 'react-icons/md';
import { useDispatch, useSelector } from 'react-redux';
import { Link } from 'react-router-dom';

import { activateSockets, getEnableSockets } from '../../../common/redux/commonSlice';
import './header.less';
import config from '/app/config';
import { getAuth } from '/auth/redux/authSlice';
import { sagaActions } from '/common/sagas';

const Header = () => {
    const dispatch = useDispatch();

    const authInfo = useSelector(getAuth);
    const enableSockets = useSelector(getEnableSockets);

    const doLogout = () =>
        dispatch({
            type: sagaActions.AUTH_LOGOUT,
            payload: {},
        });

    const useSocketsToggle = () => dispatch(activateSockets(!enableSockets));

    return (
        <Navbar
            bg="light"
            data-bs-theme="light"
            className="header p-0 justify-content-between"
            expand="lg"
            collapseOnSelect
        >
            <Container>
                <Navbar.Toggle aria-controls="basic-navbar-nav" className="me-3" />
                <Navbar.Brand className="flex-grow-0 d-none d-md-inline">
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
                <Navbar.Offcanvas id="basic-navbar-nav" placement="start">
                    <Offcanvas.Header closeButton>
                        <img
                            alt=""
                            src="/pictures/girls/logo.png"
                            width="40"
                            height="40"
                            className="d-inline-block align-top me-2"
                        />
                        <span className="align-middle">React-Jackal</span>
                    </Offcanvas.Header>
                    <Offcanvas.Body>
                        <Nav className="me-auto" activeKey="/">
                            <Nav.Link as={Link} to="/quickstart" eventKey="quickstart">
                                <GiWingfoot size={20} className="menu-link" />
                                Быстрый старт
                            </Nav.Link>
                            <Nav.Link as={Link} to="/newgame" eventKey="newgame">
                                <FaGamepad size={20} className="menu-link" />
                                Новая игра
                            </Nav.Link>
                            <Nav.Link as={Link} to="/netgame" eventKey="netgame">
                                <FaNetworkWired size={20} className="menu-link" />
                                Лобби
                            </Nav.Link>
                            <Nav.Link as={Link} to="/docs" eventKey="docs">
                                <FaBookSkull size={20} className="menu-link" />
                                Обучение
                            </Nav.Link>
                        </Nav>
                    </Offcanvas.Body>
                </Navbar.Offcanvas>
                {process.env.NODE_ENV && process.env.NODE_ENV === 'development' && (
                    <Nav className="me-auto d-none d-md-inline">
                        <Nav.Link
                            as={Link}
                            to={`${config.BaseApi.substring(0, config.BaseApi.length - 4)}swagger`}
                            target="_blank"
                        >
                            Swagger
                        </Nav.Link>
                    </Nav>
                )}
                <Nav className="me-auto flex-grow-1 align-items-center d-none d-md-inline">
                    <Nav.Item>
                        <Nav.Link as={Link} to="/" onClick={useSocketsToggle}>
                            {enableSockets ? <ImFire /> : <MdWaterDrop />}
                        </Nav.Link>
                    </Nav.Item>
                </Nav>
                <Navbar className="p-0">
                    <Container className="justify-content-end">
                        {authInfo.isAuthorised && (
                            <Nav style={{ marginRight: '5px' }}>
                                <div className="login-wrapper">
                                    <img src={`ranks/${authInfo.user?.rank}.webp`} alt={authInfo.user?.rank} />
                                </div>
                            </Nav>
                        )}
                        <Nav className="me-3">
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
                    </Container>
                </Navbar>
            </Container>
        </Navbar>
    );
};

export default Header;
