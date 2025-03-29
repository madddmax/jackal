interface GameCell {
    backgroundImageSrc: string;
    rotate: number;
    levels: GameLevel[];
    x: number;
    y: number;
}

interface GameLevel {
    level: number;
    coins: number;
    piratesWithCoinsCount?: number;
    freeCoinGirlId?: string;
    features?: LevelFeature[];
}

interface LevelFeature {
    backgroundColor: string;
    photo: string;
}
