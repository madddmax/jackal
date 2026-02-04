import { Constants } from './constants';
import { getAnotherRandomValue } from './global';
import { PiratePhotoIdentity } from '/common/types/common';

interface GroupTest {
    photo?: PiratePhotoIdentity;
}

describe('global functions tests', () => {
    test('Подбираем разных пираток', () => {
        const group = Constants.groups.find((it) => it.id === Constants.groupIds.reGirls)!;
        const girls: GroupTest[] = [{}, {}, {}];
        girls.forEach((it) => {
            it.photo = getAnotherRandomValue(
                group.photos,
                girls.filter((pr) => pr.photo && pr.photo.type > 0).map((pr) => pr.photo!.type) ?? [],
            );
        });
        expect(girls.map((it) => it.photo?.type).sort()).toEqual([1, 2, 3]);

        girls.push({
            photo: getAnotherRandomValue(
                group.photos,
                girls.filter((pr) => pr.photo && pr.photo.type > 0).map((pr) => pr.photo?.type ?? 0) ?? [],
            ),
        });

        expect(girls.map((it) => it.photo?.type).sort()).toEqual([1, 1, 2, 3]);
    });

    test('Подбираем разных пираток 2', () => {
        const group = Constants.groups.find((it) => it.id === Constants.groupIds.reAnime)!;
        const girls: GroupTest[] = [{}, {}, {}];
        girls.forEach((it) => {
            it.photo = getAnotherRandomValue(
                group.photos,
                girls.filter((pr) => pr.photo && pr.photo.type > 0).map((pr) => pr.photo?.type ?? 0) ?? [],
            );
        });
        expect(girls[0].photo?.type).not.toEqual(girls[1].photo?.type);
        expect(girls[0].photo?.type).not.toEqual(girls[2].photo?.type);
        expect(girls[1].photo?.type).not.toEqual(girls[2].photo?.type);
    });
});
