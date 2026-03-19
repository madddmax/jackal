import { ImageGroupsIds } from '/app/constants';

export interface ScreenSizes {
    width: number;
    height: number;
}

export interface TeamScores {
    teamId: number;
    name: string;
    backColor: string;
    imageGroupId: ImageGroupsIds;
    coins: number;
    bottles: number;
    wasteTime: number;
}
