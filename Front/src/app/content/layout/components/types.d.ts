import { ImageGroupsIds } from '/app/constants';

export interface PlayersInfo {
    mode: number;
    groups: ImageGroupsIds[];
    gamers: PlayerInfo[];
}

export interface PlayerInfo {
    id: number;
    type: string;
    userId: number;
    userName?: string;
}
