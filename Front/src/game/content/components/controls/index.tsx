import cn from 'classnames';
import { useEffect, useRef, useState } from 'react';
import { Alert } from 'react-bootstrap';
import { PiBeerBottleThin, PiCoinsLight, PiTimer } from 'react-icons/pi';
import { useSelector } from 'react-redux';

import classes from './controls.module.less';
import { Constants } from '/app/constants';
import { getGameSettings, getGameStatistics, getTeamScores } from '/game/redux/gameSlice';
import { TeamScores } from '/game/redux/gameSlice.types';

const initTimes = (ts: TeamScores[] | undefined, mode: string | undefined): Record<number, number> => {
    const times: Record<number, number> = {};
    if (mode == Constants.gameModeTypes.TwoPlayersInTeam) {
        [0, 1].forEach((it) => (times[it] = 0));
    } else {
        ts?.forEach((it) => (times[it.teamId] = 0));
    }
    return times;
};

function Controls() {
    const { gameId, mapSize, mapId, tilesPackName, gameMode } = useSelector(getGameSettings);
    const stat = useSelector(getGameStatistics);
    const teamScores = useSelector(getTeamScores);

    const [showTiming, setShowTiming] = useState<boolean>(false);
    const [timing, setTiming] = useState<number>(0);
    const [curTeamId, setCurTeamId] = useState<number | undefined>(stat?.currentTeamId);
    const times = useRef<Record<number, number>>(initTimes(teamScores, gameMode));

    useEffect(() => {
        const timer = setTimeout(() => {
            {
                stat && !stat.isGameOver && setTiming((prev) => (prev += 1));
            }
        }, 1000);

        return () => clearTimeout(timer);
    }, [timing]);

    useEffect(() => {
        if (stat?.currentTeamId !== undefined) {
            setCurTeamId((prev) => {
                if (prev !== undefined) {
                    if (gameMode == Constants.gameModeTypes.TwoPlayersInTeam) {
                        times.current[prev % 2] = timing;
                        setTiming(times.current[stat.currentTeamId % 2] || 0);
                    } else {
                        times.current[prev] = timing;
                        setTiming(times.current[stat.currentTeamId] || 0);
                    }
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
                                                {curTeamId === it.teamId ? timing : times.current[it.teamId]}
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
                                    {showTiming ? (
                                        <>
                                            <PiTimer style={{ marginRight: '2px', color: 'white' }} />
                                            <span style={{ color: 'white' }}>
                                                {(curTeamId ?? 0) % 2 === num ? timing : times.current[num]}
                                            </span>
                                        </>
                                    ) : (
                                        <>
                                            <PiBeerBottleThin style={{ marginRight: '2px', color: 'white' }} />
                                            <span style={{ color: 'white' }}>{teamScores[num].bottles}</span>
                                            <div className={cn(classes.coinsCount, 'coins')}>
                                                {teamScores[num].coins}
                                            </div>
                                        </>
                                    )}
                                </div>
                            </div>
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
