export interface PlayersInfo {
    mode: number;
    members: string[];
    users: number[];
    groups: string[];
    gamers: PlayerInfo[];
}

export interface PlayerInfo {
    id: number;
    type: string;
    userId: number;
}
