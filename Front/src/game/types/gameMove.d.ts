export interface GameMove {
    moveNum: number;
    from: AcceptableMove;
    to: AcceptableMove;
    prev?: {
        x: number;
        y: number;
    };
    withCoin: boolean;
    withRespawn: boolean;
}

interface AcceptableMove {
    pirateIds: string[];
    level: number;
    x: number;
    y: number;
}
