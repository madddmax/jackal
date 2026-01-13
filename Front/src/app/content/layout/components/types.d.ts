export interface PlayersInfo {
    mode: number;
    groups: string[];
    gamers: PlayerInfo[];
}

export interface PlayerInfo {
    id: number;
    type: string;
    userId: number;
    userName?: string;
}
