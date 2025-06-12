import cn from 'classnames';
import { Alert } from 'react-bootstrap';
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
                <div className={cn(classes.teams, 'container')}>
                    {teamScores?.map((it) => (
                        <div key={`ctrl_${it.teamId}`} className="row" style={{ backgroundColor: it?.backColor ?? '' }}>
                            <div className="col-md-8">{it?.name}</div>
                            <div className="col-md-4">{it.coins}</div>
                        </div>
                    ))}
                </div>
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
