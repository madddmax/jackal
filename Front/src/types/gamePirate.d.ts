interface GamePirate extends GamePiratePosition {
    teamId: number;
    withCoin?: boolean;
    withBigCoin?: boolean;
    isDrunk?: boolean;
    isInTrap?: boolean;
    isInHole?: boolean;
    groupId: string;
    photo: string;
    photoId: number;
    type: string;
    isActive?: boolean;
    backgroundColor?: string;
}

interface GamePiratePosition {
    id: string;
    position: {
        level: number;
        x: number;
        y: number;
    };
}
