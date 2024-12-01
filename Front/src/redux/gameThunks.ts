import { Dispatch, UnknownAction } from '@reduxjs/toolkit';
import {
    applyChanges,
    applyPirateChanges,
    applyStat,
    highlightHumanMoves,
    initGame,
    initMap,
    setCurrentHumanTeam,
} from '/redux/gameSlice';
import { GameStartResponse, GameTurnResponse } from '/redux/types';

export const applyStartData = (data: GameStartResponse) => async (dispatch: Dispatch<UnknownAction>) => {
    dispatch(initMap(data.map));
    dispatch(initGame(data));
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

export const applyMoveChanges = (data: GameTurnResponse) => async (dispatch: Dispatch<UnknownAction>) => {
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
