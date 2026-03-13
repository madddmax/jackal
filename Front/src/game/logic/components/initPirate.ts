import { TeamGroup } from '../gameLogic.types';
import { Constants, ImageGroupsIds } from '/app/constants';
import { getAnotherRandomValue } from '/app/global';
import { PirateIdentity } from '/common/types/common';

export interface InitPiratesPhotoProps {
    girlType: string;
    allGirls?: GamePirateInitiation[];
    teamId: number;
    imageGroupId: ImageGroupsIds;
    teamGroup: TeamGroup;
    gannPhotos?: PirateIdentity[];
}

export interface InitPirateDto {
    photo: string;
    photoId: number;
    name?: string;
    description?: string;
}

export const InitPirate = ({
    girlType,
    allGirls,
    teamId,
    imageGroupId,
    teamGroup,
    gannPhotos,
}: InitPiratesPhotoProps): InitPirateDto => {
    if (girlType == Constants.pirateTypes.BenGunn) {
        const pnumber = getAnotherRandomValue(
            gannPhotos ?? Constants.gannPhotos,
            allGirls
                ?.filter((pr) => pr.type == Constants.pirateTypes.BenGunn && pr.photoId > 0)
                .map((pr) => pr.photoId) ?? [],
        );
        const girl = (gannPhotos ?? Constants.gannPhotos)[pnumber.type - 1] as PirateIdentity;
        return {
            photo: `commonganns/gann_${pnumber.origin}.png`,
            photoId: pnumber.type,
            name: girl.name,
            description: girl.description,
        };
    } else if (girlType == Constants.pirateTypes.Friday) {
        const pnumber = getAnotherRandomValue(Constants.fridayPhotos, []);
        const girl = Constants.fridayPhotos[pnumber.type - 1] as PirateIdentity;
        return {
            photo: `commonfridays/friday_${pnumber.origin}.png`,
            photoId: pnumber.type,
            name: girl.name,
            description: girl.description,
        };
    } else {
        const pnumber = getAnotherRandomValue(
            teamGroup.photos,
            allGirls
                ?.filter((pr) => pr.teamId == teamId && pr.type == Constants.pirateTypes.Usual && pr.photoId > 0)
                .map((pr) => pr.photoId) ?? [],
        );
        const girl = teamGroup.photos[pnumber.type - 1] as PirateIdentity;
        return {
            photo: `${imageGroupId}/pirate_${pnumber.origin}${teamGroup.extension || '.png'}`,
            photoId: pnumber.type,
            name: girl.name,
            description: girl.description,
        };
    }
};
