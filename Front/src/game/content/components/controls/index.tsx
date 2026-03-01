import cn from 'classnames';
import { useEffect, useState } from 'react';
import { Alert } from 'react-bootstrap';
import { PiBeerBottleThin, PiCoinsLight, PiTimer } from 'react-icons/pi';
import { useSelector } from 'react-redux';

import classes from './controls.module.less';
import { Constants } from '/app/constants';
import { getGameSettings, getGameStatistics, getTeamScores } from '/game/redux/gameSlice';

const toTimeSpan = (totalSeconds: number) => {
    const hours = Math.floor(totalSeconds / (60 * 60));
    const minutes = Math.floor((totalSeconds / 60) % 60);
    const seconds = Math.floor(totalSeconds % 60);
    return hours + ':' + (minutes < 10 ? 0 : '') + minutes + ':' + (seconds < 10 ? 0 : '') + seconds;
};

function Controls() {
    const { gameId, mapSize, mapId, tilesPackName, gameMode } = useSelector(getGameSettings);
    const stat = useSelector(getGameStatistics);
    const teamScores = useSelector(getTeamScores);

    const [showTiming, setShowTiming] = useState<boolean>(false);
    const [timing, setTiming] = useState<number>(0);
    const [curTeamId, setCurTeamId] = useState<number | undefined>(stat?.currentTeamId);

    useEffect(() => {
        const timer = setTimeout(() => {
            {
                stat && !stat.isGameOver && setTiming((prev) => (prev += 1));
            }
        }, 1000);

        return () => clearTimeout(timer);
    }, [timing]);

    useEffect(() => {
        if (stat?.currentTeamId !== undefined && teamScores) {
            setCurTeamId(() => {
                if (gameMode == Constants.gameModeTypes.TwoPlayersInTeam) {
                    if (stat?.currentTeamId % 2) {
                        setTiming(Math.floor(teamScores[1].wasteTime + teamScores[3].wasteTime));
                    } else {
                        setTiming(Math.floor(teamScores[0].wasteTime + teamScores[2].wasteTime));
                    }
                } else {
                    setTiming(Math.floor(teamScores[stat?.currentTeamId].wasteTime));
                }
                return stat.currentTeamId;
            });
        }
    }, [stat?.currentTeamId]);

    return (
        <>
            <div className={classes.statistic}>
                <div>
                    ИД игры: <span>{gameId}</span>
                </div>
                <div>
                    Код карты: <span>{mapId}</span>
                </div>
                <div>
                    Игровой набор: <span>{tilesPackName}</span>
                </div>
                <div>
                    Режим игры:{' '}
                    <span>{gameMode == Constants.gameModeTypes.TwoPlayersInTeam ? '2x2' : 'каждый сам за себя'}</span>
                </div>
                <div>
                    Размер карты: <span>{mapSize}</span>
                </div>
                <div>
                    Номер хода: <span>{stat?.turnNumber}</span>
                    {gameMode != Constants.gameModeTypes.TwoPlayersInTeam && (
                        <>
                            {showTiming ? (
                                <PiCoinsLight
                                    size={24}
                                    style={{ float: 'right', cursor: 'pointer' }}
                                    onClick={() => setShowTiming(false)}
                                />
                            ) : (
                                <PiTimer
                                    size={24}
                                    style={{ float: 'right', cursor: 'pointer' }}
                                    onClick={() => setShowTiming(true)}
                                />
                            )}
                        </>
                    )}
                </div>
                {gameMode != Constants.gameModeTypes.TwoPlayersInTeam && teamScores && (
                    <div className={cn(classes.teams, 'container')}>
                        {teamScores?.map((it) => (
                            <div
                                key={`ctrl_${it.teamId}`}
                                className={cn(classes.user, 'row')}
                                style={{
                                    backgroundColor: it?.backColor ?? '',
                                    borderColor: it?.backColor ?? '',
                                }}
                            >
                                <div className="col-8" style={{ lineHeight: '28px' }}>
                                    <div
                                        className={cn('row')}
                                        style={{
                                            padding: '1px',
                                            color: it?.backColor ?? '',
                                        }}
                                    >
                                        <div
                                            style={{
                                                background: it.teamId == curTeamId ? 'gold' : 'white',
                                                borderRadius: '50rem',
                                            }}
                                        >
                                            {it?.name}
                                        </div>
                                    </div>
                                </div>
                                <div className={cn(classes.scores, 'col-4')}>
                                    {showTiming ? (
                                        <>
                                            <PiTimer style={{ marginRight: '2px', color: 'white' }} />
                                            <span style={{ color: 'white' }}>
                                                {toTimeSpan(curTeamId === it.teamId ? timing : it.wasteTime)}
                                            </span>
                                        </>
                                    ) : (
                                        <>
                                            <PiBeerBottleThin style={{ margin: '0 2px', color: 'white' }} />
                                            <span style={{ color: 'white' }}>{it.bottles}</span>
                                            <div className={cn(classes.coinsCount, 'coins')}>{it.coins}</div>
                                        </>
                                    )}
                                </div>
                            </div>
                        ))}
                    </div>
                )}
                {gameMode == Constants.gameModeTypes.TwoPlayersInTeam && teamScores && (
                    <div className={cn(classes.teams, 'container')}>
                        {[0, 1].map((num) => (
                            <>
                                <div key={`teamstimer_${num}`} className="col-12 mb-1" style={{ fontWeight: 'bold' }}>
                                    <PiTimer
                                        size={24}
                                        style={{ marginRight: '2px', color: teamScores[num].backColor }}
                                    />
                                    <span style={{ color: teamScores[num].backColor, verticalAlign: 'middle' }}>
                                        {toTimeSpan(
                                            (curTeamId ?? 0) % 2 === num
                                                ? timing
                                                : teamScores[num].wasteTime + teamScores[num + 2].wasteTime,
                                        )}
                                    </span>
                                </div>

                                <div
                                    key={`teamscore_${num}`}
                                    className={cn(classes.user, 'row')}
                                    style={{
                                        backgroundColor: teamScores[num].backColor ?? '',
                                        borderColor: teamScores[num].backColor ?? '',
                                    }}
                                >
                                    <div className="col-8">
                                        {[0, 2].map((addnum) => {
                                            let scores = teamScores[num + addnum];
                                            return (
                                                <div
                                                    key={`ctrl_${scores.teamId}`}
                                                    className={cn('row')}
                                                    style={{
                                                        padding: '1px',
                                                        color: scores.backColor ?? '',
                                                    }}
                                                >
                                                    <div
                                                        style={{
                                                            background: scores.teamId == curTeamId ? 'gold' : 'white',
                                                            borderRadius: '50rem',
                                                        }}
                                                    >
                                                        {scores.name}
                                                    </div>
                                                </div>
                                            );
                                        })}
                                    </div>
                                    <div className={cn(classes.scores, 'col-4')}>
                                        <PiBeerBottleThin style={{ marginRight: '2px', color: 'white' }} />
                                        <span style={{ color: 'white' }}>{teamScores[num].bottles}</span>
                                        <div className={cn(classes.coinsCount, 'coins')}>{teamScores[num].coins}</div>
                                    </div>
                                </div>
                            </>
                        ))}
                    </div>
                )}
            </div>

            {stat?.gameMessage != undefined && (
                <Alert variant={stat?.isGameOver ? 'success' : 'primary'} className="my-2">
                    {stat?.gameMessage}
                </Alert>
            )}
        </>
    );
}

export default Controls;
