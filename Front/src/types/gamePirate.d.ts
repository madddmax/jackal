interface GamePirate extends GamePiratePosition, GamePiratePhotoInitiation {
    withCoin?: boolean;
    withBigCoin?: boolean;
    isDrunk?: boolean;
    isInTrap?: boolean;
    isInHole?: boolean;
    groupId: string;
    photo: string;
    type: string;
    isActive?: boolean;
    backgroundColor?: string;
}

interface GamePiratePhotoInitiation {
    teamId: number;
    type: string;
    photoId: number;
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
