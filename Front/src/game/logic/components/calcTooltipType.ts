import { TooltipTypes } from '../../constants';
import { FieldState, GameState } from '../../types';

export interface CalcTooltipTypeProps {
    row: number;
    col: number;
    field: FieldState;
    state: GameState;
}

export const CalcTooltipType = ({ row, col, field, state }: CalcTooltipTypeProps): string => {
    const team = state.teams.find((it) => it.id == state.currentHumanTeamId);
    const activePirate = state.pirates?.find((it) => it.id == team?.activePirate);
    const pirateField = activePirate && state.fields[activePirate.position.y][activePirate.position.x];

    const move = field.availableMoves[0];

    if (field.levels.length == 1 && activePirate?.position.y === row && activePirate?.position.x === col) {
        if (move.isRespawn) return TooltipTypes.Respawn;
        if (field.tileType == 'hole') return TooltipTypes.GroundHole;
        return TooltipTypes.SkipMove;
    }

    if (field.availableMoves.length > 1 && !field.availableMoves.some((it) => !it.prev)) {
        return TooltipTypes.SomeFields;
    }

    if (
        state.lastMoves.length > 1 &&
        ((field.tileType == 'water' &&
            !field?.image?.includes('ship_') &&
            !pirateField?.image?.includes('ship_') &&
            !pirateField?.image?.includes('water.png')) || // and jump from beach to water
            (field.tileType == 'cannon' && !move.isQuakeBegin && !move.isQuakeEnd))
    ) {
        return TooltipTypes.Seajump;
    }
    return TooltipTypes.NoTooltip;
};
