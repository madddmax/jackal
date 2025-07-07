import { ThunkAction, UnknownAction } from '@reduxjs/toolkit';

import { girlsMap } from '../logic/gameLogic';
import { GameState } from '../types';
import { GameStartResponse, GameTurnResponse } from '../types/gameSaga';
import { applyChanges, applyPirateChanges, applyStat, highlightHumanMoves, initGame, initMap } from './gameSlice';

export const applyStartData =
    (data: GameStartResponse): ThunkAction<void, GameState, unknown, UnknownAction> =>
    async (dispatch) => {
        dispatch(initMap(data.map));
        dispatch(initGame(data));
        dispatch(applyStat(data));
        data.pirates.forEach((girl) => {
            girlsMap.AddPosition(girl, 1);
        });
        dispatch(
            applyPirateChanges({
                moves: data.moves,
                changes: data.pirates,
            }),
        );
        dispatch(highlightHumanMoves({ moves: data.moves }));
    };

export const applyMoveChanges =
    (data: GameTurnResponse): ThunkAction<void, GameState, unknown, UnknownAction> =>
    async (dispatch) => {
        dispatch(applyStat(data));
        dispatch(applyChanges(data.changes));
        dispatch(
            applyPirateChanges({
                moves: data.moves,
                changes: data.pirateChanges,
            }),
        );
        dispatch(highlightHumanMoves({ moves: data.moves }));

        if (data.stats.isGameOver) {
            dispatch(highlightHumanMoves({ moves: [] }));
        }
    };
