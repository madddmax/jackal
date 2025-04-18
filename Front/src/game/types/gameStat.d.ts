interface GameStat {
    turnNo: number;
    currentTeamId: number;
    isGameOver: boolean;
    gameMessage: string;
}

interface GameScore {
    teamId: number;
    coins: number;
}
