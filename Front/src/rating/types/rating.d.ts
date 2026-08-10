import { LeaderBoardItemResponse } from './ratingSaga';

export interface RatingState {
    leaders: LeaderBoardsInfo;
    usersOnline: number[];
}

export interface LeaderBoardsInfo {
    localLeaders: LeaderBoardItemResponse[];
    netLeaders: LeaderBoardItemResponse[];
    botLeaders: LeaderBoardItemResponse[];
    timestamp: number;
}
