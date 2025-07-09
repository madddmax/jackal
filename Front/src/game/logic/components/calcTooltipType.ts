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

    if (field.levels.length == 1 && activePirate?.position.y === row && activePirate?.position.x === col) {
        const move = field.availableMoves[0];
        if (move.isRespawn) return TooltipTypes.Respawn;
        if (field.image?.includes('hole.png')) return TooltipTypes.GroundHole;
        return TooltipTypes.SkipMove;
    }

    if (field.availableMoves.length > 1 && !field.availableMoves.some((it) => !it.prev)) {
        return TooltipTypes.SomeFields;
    }

    if (
        (field.image?.includes('water.png') &&
            !pirateField?.image?.includes('ship_1.png') &&
            !pirateField?.image?.includes('ship_2.png') &&
            !pirateField?.image?.includes('ship_3.png') &&
            !pirateField?.image?.includes('ship_4.png') &&
            !pirateField?.image?.includes('water.png')) || // and jump from beach
        field.image?.includes('cannon.png')
    ) {
        return TooltipTypes.Seajump;
    }
    return TooltipTypes.NoTooltip;
};
