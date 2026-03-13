import { TeamGroup } from '../gameLogic.types';
import { Constants, ImageGroupsIds } from '/app/constants';
import { getAnotherRandomValue } from '/app/global';
import { PirateIdentity } from '/common/types/common';

export interface InitPiratesPhotoProps {
    girlType: string;
    allGirls?: GamePiratePhotoInitiation[];
    teamId: number;
    imageGroupId: ImageGroupsIds;
    teamGroup: TeamGroup;
    gannPhotos?: PirateIdentity[];
}

export interface PiratePhotoDto {
    photo: string;
    photoId: number;
}

export const InitPiratesPhoto = ({
    girlType,
    allGirls,
    teamId,
    imageGroupId,
    teamGroup,
    gannPhotos,
}: InitPiratesPhotoProps): PiratePhotoDto => {
    let pname;
    let pnumber;
    let extension = '.png';

    if (girlType == Constants.pirateTypes.BenGunn) {
        pname = 'commonganns/gann';
        pnumber = getAnotherRandomValue(
            gannPhotos ?? Constants.gannPhotos,
            allGirls
                ?.filter((pr) => pr.type == Constants.pirateTypes.BenGunn && pr.photoId > 0)
                .map((pr) => pr.photoId) ?? [],
        );
    } else if (girlType == Constants.pirateTypes.Friday) {
        pname = 'commonfridays/friday';
        pnumber = getAnotherRandomValue(Constants.fridayPhotos, []);
    } else {
        pname = `${imageGroupId}/pirate`;
        pnumber = getAnotherRandomValue(
            teamGroup.photos,
            allGirls
                ?.filter((pr) => pr.teamId == teamId && pr.type == Constants.pirateTypes.Usual && pr.photoId > 0)
                .map((pr) => pr.photoId) ?? [],
        );
        extension = teamGroup.extension || '.png';
    }

    return { photo: `${pname}_${pnumber.origin}${extension}`, photoId: pnumber.type };
};
