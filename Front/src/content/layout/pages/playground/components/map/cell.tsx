import { useDispatch, useSelector } from 'react-redux';

import { sagaActions } from '/redux/saga';
import { FieldState, ReduxState } from '/redux/types';
import cn from 'classnames';
import './cell.less';
import Level from '/content/layout/pages/playground/components/map/level';

interface CellProps {
    row: number;
    col: number;
}

function Cell({ row, col }: CellProps) {
    const dispatch = useDispatch();

    const field = useSelector<ReduxState, FieldState>((state) => state.game.fields[row][col]);
    const cellSize = useSelector<ReduxState, number>((state) => state.game.cellSize);
    const pirateSize = useSelector<ReduxState, number>((state) => state.game.pirateSize);
    const gamename = useSelector<ReduxState, string | undefined>((state) => state.game.gameName);

    return (
        <>
            <div
                key="main_cell"
                className={cn('cell', {
                    'sell-active': field.highlight && field.highlight === true,
                })}
                style={{
                    width: cellSize,
                    height: cellSize,
                    backgroundImage: field.image ? `url(${field.image})` : '',
                    backgroundColor: field.backColor || 'transparent',
                    transform: field.rotate && field.rotate > 0 ? `rotate(${field.rotate * 90}deg)` : 'none',
                    opacity: field.availableMove?.num !== undefined ? '0.5' : '1',
                    cursor: field.availableMove?.num !== undefined ? 'pointer' : 'default',
                }}
                onClick={
                    field.availableMove
                        ? () => {
                              dispatch({
                                  type: sagaActions.GAME_TURN,
                                  payload: {
                                      gameName: gamename,
                                      turnNum: field.availableMove!.num,
                                      pirateId: field!.availableMove!.pirate,
                                  },
                              });
                          }
                        : undefined
                }
            ></div>
            {field.levels &&
                field.levels
                    .filter((it) => (it.pirates && it.pirates.length > 0) || it.coin)
                    .map((it) => <Level cellSize={cellSize} pirateSize={pirateSize} field={field} data={it} />)}
        </>
    );
}

export default Cell;
