export interface LeaderBoardItemResponse {
    userId: number;
    playerName: string;
    rank: string;
    winCountToday: number;
    winCountThisWeek: number;
    winCountThisMonth: number;
    totalWin: number;
    loseCountToday: number;
    loseCountThisWeek: number;
    loseCountThisMonth: number;
    totalLose: number;
    winPercent: number;
    totalCoins: number;
}

export interface NetGameUsersOnlineResponse {
    users: number[];
}
