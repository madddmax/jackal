export interface DocsState {
    pirate: GamePirate;
    availableMoves: GameMovePosition[];
    stepOpacity: number;
}

export interface DocsPiratePosition {
    mapSize: number;
    tiles: string[];
    img?: string;
    position: {
        level: number;
        x: number;
        y: number;
    };
}
