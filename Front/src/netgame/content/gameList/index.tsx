import { Button, Container, ListGroup, Row } from 'react-bootstrap';
import { BsArrowCounterclockwise } from 'react-icons/bs';
import { PiEyesThin } from 'react-icons/pi';
import { VscDebugContinueSmall } from 'react-icons/vsc';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import classes from './gamelist.module.less';
import { fromNow } from '/app/global';
import { getAuth } from '/auth/redux/authSlice';
import gameHub from '/game/hub/gameHub';
import { getGames } from '/netgame/redux/lobbySlice';

const GameList = () => {
    const navigate = useNavigate();
    const list = useSelector(getGames);
    const auth = useSelector(getAuth);

    const loadGame = (gameId: number) => {
        navigate('/');
        gameHub.loadGame(gameId);
    };

    return (
        <Container>
            <Row className="justify-content-center">
                <div className={classes.gameList}>
                    <ListGroup>
                        {list &&
                            list.map((it) => {
                                const timeData = fromNow(it.timeStamp);
                                return (
                                    <ListGroup.Item
                                        key={`netgame-${it.id}`}
                                        className={classes.listIconsItem}
                                        style={{
                                            display: 'flex',
                                            alignItems: 'center',
                                        }}
                                    >
                                        <span>{it.id}</span>
                                        <span>
                                            <BsArrowCounterclockwise
                                                size={48}
                                                color={timeData.color}
                                                style={{ verticalAlign: 'middle' }}
                                            />
                                            <span
                                                style={{
                                                    fontSize: '8px',
                                                    marginLeft: -36,
                                                    color: timeData.color,
                                                }}
                                            >
                                                {timeData.value}
                                                {timeData.unit}
                                            </span>
                                        </span>

                                        <span style={{ flexGrow: 2 }}>{it.creator.name}</span>
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
                                    </ListGroup.Item>
                                );
                            })}
                    </ListGroup>
                </div>
            </Row>
        </Container>
    );
};

export default GameList;
