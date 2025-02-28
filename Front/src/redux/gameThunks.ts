import { ThunkAction, UnknownAction } from '@reduxjs/toolkit';
import {
    applyChanges,
    applyPirateChanges,
    applyStat,
    highlightHumanMoves,
    initGame,
    initMap,
    setCurrentHumanTeam,
} from '/redux/gameSlice';
import { GameStartResponse, GameState, GameTurnResponse } from '/redux/types';
import { girlsMap } from '/app/global';

export const applyStartData =
    (data: GameStartResponse): ThunkAction<void, GameState, unknown, UnknownAction> =>
    async (dispatch) => {
        dispatch(initMap(data.map));
        dispatch(initGame(data));
        data.pirates.forEach((girl) => {
            girlsMap.AddPosition(girl, 1);
        });
        dispatch(
            applyPirateChanges({
                moves: data.moves,
                changes: data.pirates,
                isHumanPlayer: data.stats.isHumanPlayer,
            }),
        );
        if (data.stats.isHumanPlayer) {
            dispatch(setCurrentHumanTeam(data.stats.currentTeamId));
            dispatch(highlightHumanMoves({ moves: data.moves }));
        }
        dispatch(applyStat(data.stats));
    };

export const applyMoveChanges =
    (data: GameTurnResponse): ThunkAction<void, GameState, unknown, UnknownAction> =>
    async (dispatch) => {
        dispatch(applyChanges(data.changes));
        dispatch(
            applyPirateChanges({
                moves: data.moves,
                changes: data.pirateChanges,
                isHumanPlayer: data.stats.isHumanPlayer,
            }),
        );

        if (data.stats.isHumanPlayer) {
            dispatch(setCurrentHumanTeam(data.stats.currentTeamId));
            dispatch(highlightHumanMoves({ moves: data.moves }));
        }

        dispatch(applyStat(data.stats));
        if (data.stats.isGameOver) {
            dispatch(highlightHumanMoves({ moves: [] }));
        }
    };
