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
import { GameStartResponse, GameState, GameTurnResponse, TeamState } from '/redux/types';
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
            }),
        );
        const currentTeam = data.stats.teams.find((it) => it.id === data.stats.currentTeamId);
        if (currentTeam?.isHuman) {
            dispatch(setCurrentHumanTeam(data.stats.currentTeamId));
            dispatch(highlightHumanMoves({ moves: data.moves }));
        }
        dispatch(applyStat(data.stats));
    };

export const applyMoveChanges =
    (data: GameTurnResponse): ThunkAction<void, GameState, unknown, UnknownAction> =>
    async (dispatch, state) => {
        dispatch(applyChanges(data.changes));
        dispatch(
            applyPirateChanges({
                moves: data.moves,
                changes: data.pirateChanges,
            }),
        );

        const currentTeam: TeamState | undefined = state().teams.find((it) => it.id === data.stats.currentTeamId);
        if (currentTeam!.isHuman) {
            dispatch(setCurrentHumanTeam(data.stats.currentTeamId));
            dispatch(highlightHumanMoves({ moves: data.moves }));
        }

        dispatch(applyStat(data.stats));
        if (data.stats.isGameOver) {
            dispatch(highlightHumanMoves({ moves: [] }));
        }
    };
