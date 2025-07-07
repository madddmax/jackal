export interface GirlsLevel {
    level: number;
    levelsCountInCell: number;
    girls: string[] | undefined;
}

export interface GirlsPositions {
    Map: { [id: number]: GirlsLevel };
    AddPosition: (it: GamePiratePosition, levelsCount: number) => void;
    RemovePosition: (it: GamePiratePosition) => void;
    GetPosition: (it: GamePiratePosition) => GirlsLevel | undefined;
    ScrollGirls: (pos: GirlsLevel) => void;
}
