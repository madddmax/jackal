import cn from 'classnames';
import { Button, Col, Container, ListGroup, Row } from 'react-bootstrap';
import { PiEyesThin } from 'react-icons/pi';
import { TbArrowsJoin } from 'react-icons/tb';
import { VscDebugContinueSmall } from 'react-icons/vsc';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import { getGames, getLeaderBoard, getNetGames, getNetLeaderBoard } from '../../redux/lobbySlice';
import GameListItem from './components/gameListItem';
import classes from './gamelist.module.less';
import gameHub from '/game/hub/gameHub';
import Leaderboard from '/lobby/content/gameList/components/leaderboard';

const GameList = () => {
    const navigate = useNavigate();
    const list = useSelector(getGames);
    const netList = useSelector(getNetGames);
    const leaders = useSelector(getLeaderBoard);
    const netLeaders = useSelector(getNetLeaderBoard);

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
                <Col lg={6} className="g-lg-2">
                    <div className={classes.leaderboard}>
                        <Leaderboard items={leaders} />
                    </div>
                </Col>
                <Col xs={{ order: 'first' }} lg={{ span: 6, order: 'last' }} className="g-lg-2">
                    <div className={classes.netLeaderboard}>
                        <Leaderboard items={netLeaders} />
                    </div>
                </Col>
            </Row>
        </Container>
    );
};

export default GameList;
