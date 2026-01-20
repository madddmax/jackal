export interface LeaderBoardItemResponse {
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

export interface NetGameInfoResponse {
    id: number;
    gameId?: number;
    creatorId: number;
    settings: GameSettings;
    viewers: number[];
    users: UserInfo[];
}

export interface NetGameUsersOnlineResponse {
    users: number[];
}

export interface NetGameListResponse {
    gamesEntries: NetGameEntryResponse[];
}

export interface NetGameEntryResponse {
    gameId: number;
    creator: {
        id: number;
        name: string;
    };
    players: [
        {
            id: number;
        },
    ];
    timeStamp: number;
}
