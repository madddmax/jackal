import cn from 'classnames';
import { RefObject } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { TooltipRefProps } from 'react-tooltip';

import './cell.less';
import { CalcTooltipType, TooltipTypes } from './cell.logic';
import Level from './level';
import LevelZero from './levelZero';
import store from '/app/store';
import { showMessage } from '/common/redux/commonSlice';
import gameHub from '/game/hub/gameHub';
import { getGameField, getGameSettings } from '/game/redux/gameSlice';
import { AvailableMove, FieldState, GameState } from '/game/types';

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
    const field = useSelector<{ game: GameState }, FieldState>((state) => getGameField(state, row, col));
    const { gameId, cellSize, pirateSize } = useSelector(getGameSettings);
    const hasMove = field.availableMoves.length > 0;

    const dispatch = useDispatch();

    const onClick = () => {
        if (gameId == undefined) {
            dispatch(
                showMessage({
                    isError: true,
                    errorCode: 'HasNoGameId',
                    messageText: 'Не найден ключ игры',
                }),
            );

            return;
        }

        const makeMove = (move: AvailableMove) => {
            gameHub.makeGameMove({
                gameId: gameId,
                turnNum: move.num,
                pirateId: move.pirateId,
            });
        };
        const tooltipOnClick = (move: AvailableMove) => () => {
            makeMove(move);
            tooltipRef.current?.close();
        };

        const gameState = store.getState().game as GameState;
        const tooltipType = CalcTooltipType({ row, col, field, state: gameState });
        switch (tooltipType) {
            case TooltipTypes.Respawn:
            case TooltipTypes.GroundHole:
            case TooltipTypes.SkipMove:
            case TooltipTypes.Seajump:
                tooltipRef.current?.open({
                    anchorSelect: `#cell_${col}_${row}`,
                    content: (
                        <div
                            className={tooltipType}
                            style={{
                                width: pirateSize,
                                height: pirateSize,
                                cursor: 'pointer',
                            }}
                            onClick={tooltipOnClick(field.availableMoves[0])}
                        />
                    ),
                });
                break;
            case TooltipTypes.SomeFields: {
                const moves = [] as CellAvailableMove[];
                field.availableMoves.forEach((it) => {
                    const cell = gameState.fields[it.prev!.y][it.prev!.x];
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
                                    onClick={tooltipOnClick(it)}
                                />
                            ))}
                        </>
                    ),
                });
                break;
            }
            case TooltipTypes.NoTooltip:
            default:
                makeMove(field.availableMoves[0]);
                break;
        }
    };

    return (
        <>
            <div
                key="main_cell"
                id={`cell_${col}_${row}`}
                className={cn(
                    'cell',
                    { 'cell-dark': field.dark === true },
                    { 'cell-active': field.highlight === true },
                )}
                style={{
                    width: cellSize,
                    height: cellSize,
                    backgroundImage: field.image ? `url(${field.image})` : '',
                    transform: field.rotate && field.rotate > 0 ? `rotate(${field.rotate * 90}deg)` : 'none',
                    opacity: hasMove ? '0.5' : '1',
                    cursor: hasMove ? 'pointer' : 'default',
                }}
                onClick={hasMove ? onClick : undefined}
            ></div>
            {field.levels &&
                field.levels.length === 1 &&
                (field.levels[0].hasFreeMoney() ||
                    (field.levels[0].features && field.levels[0].features.length > 0)) && (
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
                    .filter((it) => it.hasFreeMoney())
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
