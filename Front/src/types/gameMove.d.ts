interface GameMove {
    moveNum: number;
    from: GameMovePosition;
    to: GameMovePosition;
    prev?: {
        x: number;
        y: number;
    };
    withCoin: boolean;
    withBigCoin: boolean;
    withRumBottle: boolean;
    withRespawn: boolean;
    withLighthouse: boolean;
    withQuakeFirst: boolean;
    withQuakeLast: boolean;
}

interface GameMovePosition {
    pirateIds: string[];
    level: number;
    x: number;
    y: number;
}
