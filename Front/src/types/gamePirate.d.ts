interface GamePirate extends GamePiratePosition, GamePirateInitiation {
    withCoin?: boolean;
    withBigCoin?: boolean;
    isDrunk?: boolean;
    isInTrap?: boolean;
    isInHole?: boolean;
    photo: string;
    type: string;
    isActive?: boolean;
    backgroundColor?: string;
}

interface GamePirateInitiation {
    teamId: number;
    type: string;
    photoId: number;
    name?: string;
    description?: string;
}

interface GamePiratePosition {
    teamId: number;
    id: string;
    position: {
        level: number;
        x: number;
        y: number;
    };
}
