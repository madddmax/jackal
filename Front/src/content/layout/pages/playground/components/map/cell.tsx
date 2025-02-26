import { useDispatch, useSelector } from 'react-redux';

import { sagaActions } from '/sagas/constants';
import { AvailableMove, FieldState, GameState, ReduxState } from '/redux/types';
import store from '/app/store';
import cn from 'classnames';
import './cell.less';
import Level from './level';
import LevelZero from './levelZero';
import { TooltipRefProps } from 'react-tooltip';
import { RefObject, useCallback } from 'react';
import { hubConnection } from '/app/global';

interface CellAvailableMove extends AvailableMove {
    img?: string;
    rotate?: number;
}

interface CellProps {
    row: number;
    col: number;
    tooltipRef: RefObject<TooltipRefProps>;
}

function Cell({ row, col, tooltipRef }: CellProps) {
    const dispatch = useDispatch();

    const useSockets = useSelector<ReduxState, boolean>((state) => state.common.useSockets);
    const field = useSelector<ReduxState, FieldState>((state) => state.game.fields[row][col]);
    const cellSize = useSelector<ReduxState, number>((state) => state.game.cellSize);
    const pirateSize = useSelector<ReduxState, number>((state) => state.game.pirateSize);
    const gamename = useSelector<ReduxState, string | undefined>((state) => state.game.gameName);
    const hasMove = field.availableMoves.length > 0;

    const onClick = useCallback(() => {
        let gameState = store.getState().game as GameState;
        let team = gameState.teams.find((it) => it.id == gameState.currentHumanTeamId);
        let activePirate = gameState.pirates?.find((it) => it.id == team?.activePirate);
        if (field.levels.length == 1 && activePirate?.position.y === row && activePirate?.position.x === col) {
            let move = field.availableMoves[0];
            tooltipRef.current?.open({
                anchorSelect: `#cell_${col}_${row}`,
                content: (
                    <div
                        className={move.isRespawn ? 'respawn' : 'skipmove'}
                        style={{
                            width: pirateSize,
                            height: pirateSize,
                            cursor: 'pointer',
                        }}
                        onClick={() => {
                            if (useSockets) {
                                hubConnection.send('Move', {
                                    gameName: gamename,
                                    turnNum: move.num,
                                    pirateId: move.pirateId,
                                });
                            } else {
                                dispatch({
                                    type: sagaActions.GAME_TURN,
                                    payload: {
                                        gameName: gamename,
                                        turnNum: move.num,
                                        pirateId: move.pirateId,
                                    },
                                });
                            }
                            tooltipRef.current?.close();
                        }}
                    />
                ),
            });
        } else if (field.availableMoves.length > 1 && !field.availableMoves.some((it) => !it.prev)) {
            let moves = [] as CellAvailableMove[];
            field.availableMoves.forEach((it) => {
                let cell = gameState.fields[it.prev!.y][it.prev!.x];
                moves.push({
                    ...it,
                    img: cell.image!,
                    rotate: cell.rotate,
                });
            });

            tooltipRef.current?.open({
                anchorSelect: `#cell_${col}_${row}`,
                content: (
                    <>
                        {moves.map((it) => (
                            <img
                                style={{
                                    transform: it.rotate && it.rotate > 0 ? `rotate(${it.rotate * 90}deg)` : 'none',
                                    margin: '5px 3px',
                                    width: pirateSize * 1.6,
                                    height: pirateSize * 1.6,
                                    cursor: 'pointer',
                                }}
                                src={it.img}
                                onClick={() => {
                                    if (useSockets) {
                                        hubConnection.send('Move', {
                                            gameName: gamename,
                                            turnNum: it.num,
                                            pirateId: it.pirateId,
                                        });
                                    } else {
                                        dispatch({
                                            type: sagaActions.GAME_TURN,
                                            payload: {
                                                gameName: gamename,
                                                turnNum: it.num,
                                                pirateId: it.pirateId,
                                            },
                                        });
                                    }
                                    tooltipRef.current?.close();
                                }}
                            />
                        ))}
                    </>
                ),
            });
        } else {
            let move = field.availableMoves[0];
            if (useSockets) {
                hubConnection.send('Move', {
                    gameName: gamename,
                    turnNum: move.num,
                    pirateId: move.pirateId,
                });
            } else {
                dispatch({
                    type: sagaActions.GAME_TURN,
                    payload: {
                        gameName: gamename,
                        turnNum: move.num,
                        pirateId: move.pirateId,
                    },
                });
            }
        }
    }, [dispatch, field, gamename]);

    return (
        <>
            <div
                key="main_cell"
                id={`cell_${col}_${row}`}
                className={cn(
                    'cell',
                    { 'cell-dark': field.dark === true },
                    { 'sell-active': field.highlight === true },
                )}
                style={{
                    width: cellSize,
                    height: cellSize,
                    backgroundImage: field.image ? `url(${field.image})` : '',
                    backgroundColor: field.backColor || 'transparent',
                    transform: field.rotate && field.rotate > 0 ? `rotate(${field.rotate * 90}deg)` : 'none',
                    opacity: hasMove ? '0.5' : '1',
                    cursor: hasMove ? 'pointer' : 'default',
                }}
                onClick={hasMove ? onClick : undefined}
            ></div>
            {field.levels &&
                field.levels.length === 1 &&
                (field.levels[0].coin || (field.levels[0].features && field.levels[0].features.length > 0)) && (
                    <LevelZero
                        key={`cell-level-0`}
                        cellSize={cellSize}
                        pirateSize={pirateSize}
                        data={field.levels[0]}
                        onClick={hasMove ? onClick : undefined}
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
                            onClick={hasMove ? onClick : undefined}
                        />
                    ))}
            {field.levels &&
                field.levels.length > 1 &&
                field.levels
                    .filter((it) => it.coin)
                    .map((it, idx) => (
                        <Level
                            key={`cell-level-${idx}-pirates`}
                            cellSize={cellSize}
                            pirateSize={pirateSize}
                            field={field}
                            data={it}
                            onClick={hasMove ? onClick : undefined}
                        />
                    ))}
        </>
    );
}

export default Cell;
