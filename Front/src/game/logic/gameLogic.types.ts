import { PirateIdentity } from '/common/types/common';

export interface GirlsLevel {
    level: number;
    levelsCountInCell: number;
    girls: GirlsLogicPosition[] | undefined;
}

export interface GirlsPositions {
    Map: { [id: number]: GirlsLevel };
    AddPosition: (it: GamePiratePosition, levelsCount: number) => void;
    RemovePosition: (it: GamePiratePosition) => void;
    GetPosition: (it: GamePiratePosition) => GirlsLevel | undefined;
    CalcTopOffset: (girl: GamePiratePosition, mapSize: number, cellSize: number, pirateSize: number) => number;
    CalcLeftOffset: (girl: GamePiratePosition, cellSize: number, pirateSize: number) => number;
    ScrollGirls: (pos: GirlsLevel) => void;
}

export interface GirlsLogicPosition {
    id: string;
    teamId: number;
    order: number;
}

export interface TeamGroup {
    photos: PirateIdentity[];
    extension?: string;
}
