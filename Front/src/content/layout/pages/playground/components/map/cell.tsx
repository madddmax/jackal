import { useDispatch, useSelector } from 'react-redux';

import { sagaActions } from '/sagas/constants';
import { FieldState, GameState, ReduxState } from '/redux/types';
import store from '/app/store';
import cn from 'classnames';
import './cell.less';
import Level from './level';
import LevelZero from './levelZero';
import { TooltipRefProps } from 'react-tooltip';
import { RefObject, useCallback } from 'react';

interface CellProps {
    row: number;
    col: number;
    tooltipRef: RefObject<TooltipRefProps>;
}

function Cell({ row, col, tooltipRef }: CellProps) {
    const dispatch = useDispatch();

    const field = useSelector<ReduxState, FieldState>((state) => state.game.fields[row][col]);
    const cellSize = useSelector<ReduxState, number>((state) => state.game.cellSize);
    const pirateSize = useSelector<ReduxState, number>((state) => state.game.pirateSize);
    const gamename = useSelector<ReduxState, string | undefined>((state) => state.game.gameName);

    const onClick = useCallback(() => {
        let gameState = store.getState().game as GameState;
        let team = gameState.teams.find((it) => it.id == gameState.currentHumanTeamId);
        if (field.levels.length == 1 && field.levels[0].pirates?.some((it) => it.id == team?.activePirate)) {
            tooltipRef.current?.open({
                anchorSelect: `#cell_${col}_${row}`,
                content: (
                    <div
                        className={field.availableMove!.isRespawn ? 'respawn' : 'skipmove'}
                        style={{
                            width: pirateSize,
                            height: pirateSize,
                            cursor: 'pointer',
                        }}
                        onClick={() => {
                            dispatch({
                                type: sagaActions.GAME_TURN,
                                payload: {
                                    gameName: gamename,
                                    turnNum: field.availableMove!.num,
                                    pirateId: field!.availableMove!.pirate,
                                },
                            });
                            tooltipRef.current?.close();
                        }}
                    />
                ),
            });
        } else {
            dispatch({
                type: sagaActions.GAME_TURN,
                payload: {
                    gameName: gamename,
                    turnNum: field.availableMove!.num,
                    pirateId: field!.availableMove!.pirate,
                },
            });
        }
    }, [dispatch, field, gamename]);

    return (
        <>
            <div
                key="main_cell"
                id={`cell_${col}_${row}`}
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
                onClick={field.availableMove ? onClick : undefined}
            ></div>
            {field.levels &&
                field.levels.length === 1 &&
                ((field.levels[0].pirates && field.levels[0].pirates.length > 0) ||
                    field.levels[0].coin ||
                    (field.levels[0].features && field.levels[0].features.length > 0)) && (
                    <LevelZero
                        key={`cell-level-0`}
                        cellSize={cellSize}
                        pirateSize={pirateSize}
                        data={field.levels[0]}
                        onClick={field.availableMove ? onClick : undefined}
                    />
                )}
            {field.levels &&
                field.levels.length > 1 &&
                field.levels
                    .filter((it) => it.features && it.features.length > 0)
                    .map((it, idx) => (
                        <Level
                            key={`cell-level-${idx}-features`}
                            cellSize={cellSize}
                            pirateSize={pirateSize}
                            field={field}
                            data={it}
                            hasFeaturesOnly
                            onClick={field.availableMove ? onClick : undefined}
                        />
                    ))}
            {field.levels &&
                field.levels.length > 1 &&
                field.levels
                    .filter((it) => (it.pirates && it.pirates.length > 0) || it.coin)
                    .map((it, idx) => (
                        <Level
                            key={`cell-level-${idx}-pirates`}
                            cellSize={cellSize}
                            pirateSize={pirateSize}
                            field={field}
                            data={it}
                            onClick={field.availableMove ? onClick : undefined}
                        />
                    ))}
        </>
    );
}

export default Cell;
