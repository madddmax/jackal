interface GameMove {
    moveNum: number;
    from: GameMovePosition;
    to: GameMovePosition;
    prev?: {
        x: number;
        y: number;
    };
    withCoin: boolean;
    withRespawn: boolean;
}

interface GameMovePosition {
    pirateIds: string[];
    level: number;
    x: number;
    y: number;
}
