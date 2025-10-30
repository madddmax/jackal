export interface LeaderBoardItemResponse {
    playerName: string;
    rank: string;
    winCountToday: number;
    winCountThisWeek: number;
    winCountThisMonth: number;
    totalWin: number;
    gamesCountToday: number;
    gamesCountThisWeek: number;
    gamesCountThisMonth: number;
    gamesCountTotal: number;
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
