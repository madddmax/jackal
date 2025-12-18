import cn from 'classnames';
import { Alert } from 'react-bootstrap';
import { PiBeerBottleThin, PiCoinVerticalThin } from 'react-icons/pi';
import { useSelector } from 'react-redux';

import classes from './controls.module.less';
import { Constants } from '/app/constants';
import { getGameSettings, getGameStatistics, getTeamScores } from '/game/redux/gameSlice';

function Controls() {
    const { gameId, mapSize, mapId, tilesPackName, gameMode } = useSelector(getGameSettings);
    const stat = useSelector(getGameStatistics);
    const teamScores = useSelector(getTeamScores);

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
                </div>
                {gameMode != Constants.gameModeTypes.TwoPlayersInTeam && teamScores && (
                    <div className={cn(classes.teams, 'container')}>
                        {teamScores?.map((it) => (
                            <div
                                key={`ctrl_${it.teamId}`}
                                className={cn(classes.user, 'row')}
                                style={{
                                    backgroundColor: it?.backColor ?? '',
                                    borderColor: it.teamId == stat?.currentTeamId ? 'gold' : (it?.backColor ?? ''),
                                }}
                            >
                                <div className="col-8">{it?.name}</div>
                                <div className="col-4">
                                    <PiBeerBottleThin style={{ marginRight: '2px' }} />
                                    {it.bottles}
                                    <PiCoinVerticalThin style={{ margin: '0 2px 0 4px' }} />
                                    {it.coins}
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
                                                        background:
                                                            scores.teamId == stat?.currentTeamId ? 'gold' : 'white',
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
