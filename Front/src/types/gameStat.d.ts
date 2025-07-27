interface GameStat {
    turnNumber: number;
    currentTeamId: number;
    currentUserId: number;
    isCurrentUsersMove?: boolean; // TODO: это поле вычисляемое на фронте
    isGameOver: boolean;
    gameMessage: string;
}

interface GameScore {
    teamId: number;
    coins: number;
    rumBottles: number;
}
