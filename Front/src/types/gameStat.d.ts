interface GameStat {
    turnNumber: number;
    currentTeamId: number;
    isGameOver: boolean;
    gameMessage: string;
}

interface GameScore {
    teamId: number;
    coins: number;
}
