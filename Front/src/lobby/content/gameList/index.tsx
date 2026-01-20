import cn from 'classnames';
import { Button, Col, Container, ListGroup, Row, Tab, Tabs } from 'react-bootstrap';
import { PiEyesThin } from 'react-icons/pi';
import { TbArrowsJoin } from 'react-icons/tb';
import { VscDebugContinueSmall } from 'react-icons/vsc';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import { getGames, getLeaders, getNetGames, getUsersOnline } from '../../redux/lobbySlice';
import GameListItem from './components/gameListItem';
import classes from './gamelist.module.less';
import gameHub from '/game/hub/gameHub';
import Leaderboard from '/lobby/content/gameList/components/leaderboard';

const GameList = () => {
    const navigate = useNavigate();
    const list = useSelector(getGames);
    const netList = useSelector(getNetGames);
    const leaders = useSelector(getLeaders);
    const usersOnline = useSelector(getUsersOnline);

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
                <Col lg className="g-lg-2">
                    <div className={cn(classes.gameList)}>
                        <ListGroup>
                            {list &&
                                list.map((it) => (
                                    <GameListItem
                                        key={`game-${it.id}`}
                                        id={it.id}
                                        isPublic={it.isPublic}
                                        creatorName={it.creatorName}
                                        timeStamp={it.timeStamp}
                                    >
                                        <Button
                                            className="float-end"
                                            variant="outline-primary"
                                            type="submit"
                                            onClick={() => loadGame(it.id)}
                                        >
                                            {it.isCreator || it.isPlayer ? (
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
                </Col>
                <Col lg className="g-lg-2">
                    <div className={cn(classes.netGameList)}>
                        <ListGroup>
                            {netList &&
                                netList.map((it) => (
                                    <GameListItem
                                        key={`netgame-${it.id}`}
                                        creatorName={it.creatorName}
                                        timeStamp={it.timeStamp}
                                    >
                                        <Button
                                            className="float-end"
                                            variant="outline-primary"
                                            type="submit"
                                            onClick={() => continueNet(it.id)}
                                        >
                                            {it.isCreator && (
                                                <>
                                                    <VscDebugContinueSmall
                                                        size={20}
                                                        style={{ verticalAlign: 'bottom', marginRight: 3 }}
                                                    />
                                                    Продолжить
                                                </>
                                            )}
                                            {!it.isCreator && it.isPlayer && (
                                                <>
                                                    <PiEyesThin
                                                        size={20}
                                                        style={{ verticalAlign: 'bottom', marginRight: 3 }}
                                                    />
                                                    Смотреть
                                                </>
                                            )}
                                            {!it.isCreator && !it.isPlayer && (
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
                </Col>
            </Row>
            <Row className="justify-content-center">
                <Col className="g-lg-2">
                    <div className={classes.leaderboard}>
                        <Tabs defaultActiveKey="netleaders" id="leaderboard-tab" className="mb-3">
                            <Tab eventKey="leaders" title="Одиночный" style={{ overflowX: 'auto' }}>
                                <Leaderboard items={leaders.localLeaders} usersOnline={usersOnline} />
                            </Tab>
                            <Tab eventKey="netleaders" title="Командный">
                                <Leaderboard items={leaders.netLeaders} usersOnline={usersOnline} />
                            </Tab>
                            <Tab eventKey="botleaders" title="Роботы">
                                <Leaderboard items={leaders.botLeaders} usersOnline={usersOnline} />
                            </Tab>
                        </Tabs>
                    </div>
                </Col>
            </Row>
        </Container>
    );
};

export default GameList;
