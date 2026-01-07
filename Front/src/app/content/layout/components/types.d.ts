export interface PlayersInfo {
    mode: number;
    users: number[];
    groups: string[];
    gamers: PlayerInfo[];
}

export interface PlayerInfo {
    id: number;
    type: string;
    userId: number;
    userName?: string;
}
