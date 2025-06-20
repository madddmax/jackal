import { Button, Container, ListGroup, Row, Table } from 'react-bootstrap';
import { PiEyesThin } from 'react-icons/pi';
import { TbArrowsJoin } from 'react-icons/tb';
import { VscDebugContinueSmall } from 'react-icons/vsc';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import GameListItem from './components/gameListItem';
import classes from './gamelist.module.less';
import gameHub from '/game/hub/gameHub';
import { getGames, getLeaderBoard, getNetGames } from '/netgame/redux/lobbySlice';

const GameList = () => {
    const navigate = useNavigate();
    const list = useSelector(getGames);
    const netList = useSelector(getNetGames);
    const leaders = useSelector(getLeaderBoard);

    const continueNet = (gameId: number) => {
        navigate('/newpublic');
        gameHub.netJoin(gameId);
    };

    const loadGame = (gameId: number) => {
        gameHub.loadGame(gameId);
    };

    let ratingNumber = 1;

    return (
        <Container>
            <Row className="justify-content-center">
                <div className={classes.gameList}>
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
            </Row>
            <Row className="justify-content-center">
                <div className={classes.netGameList}>
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
            </Row>
            <Row className="justify-content-center">
                <div className={classes.leaderboard}>
                    <Table striped>
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>Логин</th>
                                <th>Рейтинг</th>
                                <th>Победы</th>
                                <th>Игры</th>
                                <th>Монеты</th>
                            </tr>
                        </thead>
                        <tbody>
                            {leaders &&
                                leaders.map((it) => (
                                    <tr key={`leader_${ratingNumber}`}>
                                        <td>{ratingNumber++}</td>
                                        <td>{it.playerName}</td>
                                        <td>{it.rating}</td>
                                        <td>{it.totalWin}</td>
                                        <td>{it.gamesCount}</td>
                                        <td>{it.totalCoins}</td>
                                    </tr>
                                ))}
                        </tbody>
                    </Table>
                </div>
            </Row>
        </Container>
    );
};

export default GameList;
