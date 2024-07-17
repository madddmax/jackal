import { useSelector } from 'react-redux';
import cn from 'classnames';
import classes from  './controls.module.less';
import { GameStat, ReduxState } from '/redux/types';

function Controls() {
  const stat = useSelector<ReduxState, GameStat | undefined>((state) => state.game.stat);
  const gamename = useSelector<ReduxState, string | undefined>((state) => state.game.gameName);
  const gamecode = useSelector<ReduxState, number | undefined>((state) => state.game.mapId);
  
    return (
      <>
        <div className={classes.statistic}>
          <div>Код игры: <span>{gamename}</span></div>
          <div>Код карты: <span>{gamecode}</span></div>
          <div>Номер хода: <span>{stat && stat.TurnNo}</span></div>
          <div className={cn(classes.teams, 'container')}>
            {stat && stat.Teams && stat.Teams.map((it) => (
              <div className='row' style={{ backgroundColor: it.backcolor }}>
                <div className='col-md-8'>{it.name}</div>
                <div className='col-md-4'>{it.gold}</div>
              </div>
            ))}
          </div>
        </div>
      </>  
    );
  }
  
  export default Controls;