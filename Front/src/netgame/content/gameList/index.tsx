import { Button, Container, ListGroup, Row } from 'react-bootstrap';
import { PiEyesThin } from 'react-icons/pi';
import { TbArrowsJoin } from 'react-icons/tb';
import { VscDebugContinueSmall } from 'react-icons/vsc';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import GameListItem from './components/gameListItem';
import classes from './gamelist.module.less';
import { getAuth } from '/auth/redux/authSlice';
import gameHub from '/game/hub/gameHub';
import { getGames, getNetGames } from '/netgame/redux/lobbySlice';

const GameList = () => {
    const navigate = useNavigate();
    const list = useSelector(getGames);
    const netList = useSelector(getNetGames);
    const auth = useSelector(getAuth);

    const continueNet = (gameId: number) => {
        navigate('/newpublic');
        gameHub.netJoin(gameId);
    };

    const loadGame = (gameId: number) => {
        gameHub.loadGame(gameId);
    };

    return (
        <Container>
            <Row className="justify-content-center">
                <div className={classes.gameList}>
                    <ListGroup>
                        {list &&
                            list.map((it) => (
                                <GameListItem
                                    key={`game-${it.id}`}
                                    info={{ id: it.id, creatorName: it.creator.name, timeStamp: it.timeStamp }}
                                >
                                    <Button
                                        className="float-end"
                                        variant="outline-primary"
                                        type="submit"
                                        onClick={() => loadGame(it.id)}
                                    >
                                        {it.creator.id === auth.user?.id ? (
                                            <>
                                                <VscDebugContinueSmall
                                                    size={20}
                                                    style={{ verticalAlign: 'bottom', marginRight: 3 }}
                                                />
                                                Продолжить
                                            </>
                                        ) : (
                                            <>
                                                <PiEyesThin
                                                    size={20}
                                                    style={{ verticalAlign: 'bottom', marginRight: 3 }}
                                                />
                                                Смотреть
                                            </>
                                        )}
                                    </Button>
                                </GameListItem>
                            ))}
                    </ListGroup>
                </div>
            </Row>
            <Row className="justify-content-center">
                <div className={classes.netGameList}>
                    <ListGroup>
                        {netList &&
                            netList.map((it) => (
                                <GameListItem
                                    key={`netgame-${it.id}`}
                                    info={{ creatorName: it.creator.name, timeStamp: it.timeStamp }}
                                >
                                    <Button
                                        className="float-end"
                                        variant="outline-primary"
                                        type="submit"
                                        onClick={() => continueNet(it.id)}
                                    >
                                        {it.creator.id === auth.user?.id ? (
                                            <>
                                                <VscDebugContinueSmall
                                                    size={20}
                                                    style={{ verticalAlign: 'bottom', marginRight: 3 }}
                                                />
                                                Продолжить
                                            </>
                                        ) : (
                                            <>
                                                <TbArrowsJoin
                                                    size={20}
                                                    style={{ verticalAlign: 'bottom', marginRight: 3 }}
                                                />
                                                Присоединиться
                                            </>
                                        )}
                                    </Button>
                                </GameListItem>
                            ))}
                    </ListGroup>
                </div>
            </Row>
        </Container>
    );
};

export default GameList;
