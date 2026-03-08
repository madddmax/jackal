export enum ImagesPacksIds {
    classic = 'classic',
    space = 'space',
    dendy_8_bit = 'dendy_8_bit'
}

export enum ImageGroupsIds {
    girls = 'girls',
    caribian = 'caribian',
    somali = 'somali',
    skulls = 'skulls',
    redalert = 'redalert',
    orcs = 'orcs',
    anime = 'anime',
    clover = 'clover',
    army = 'army',
    space = 'space',
    badguys = 'badguys',
    resident = 'resident',
    reAnime = 'resident anime',
    reGirls = 'resident girls',
}

export const Constants = {
    imagesPacks: {
        [ImagesPacksIds.classic]: '/fields/',
        [ImagesPacksIds.space]: '/fields_space/',
        [ImagesPacksIds.dendy_8_bit]: '/fields_dendy_8_bit/',
    },
    gameModeTypes: {
        FreeForAll: 'FreeForAll',
        TwoPlayersInTeam: 'TwoPlayersInTeam',
    },
    teamColors: ['DarkRed', 'DarkBlue', 'DarkViolet', 'DarkOrange'],
    pirateTypes: {
        Usual: 'Usual',
        BenGunn: 'BenGunn',
        Friday: 'Friday',
    },
    positions: ['Down', 'Left', 'Up', 'Right'],
    gannPhotos: [1, 1, 1, 1],
    fridayPhotos: [1],
    imageGroups: {
        [ImageGroupsIds.girls]: {
            photos: [1, 1, 1, 1, 1, 1],
            extension: '',
        },
        [ImageGroupsIds.caribian]: {
            photos: [2, 5, 3, 1],
        },
        [ImageGroupsIds.somali]: {
            photos: [1, 1, 1, 1, 1, 1, 1, 1],
        },
        [ImageGroupsIds.redalert]: {
            photos: [1, 1, 1, 1, 1],
            extension: '.jpg',
        },
        [ImageGroupsIds.orcs]: {
            photos: [1, 1, 1, 1, 1, 1],
            extension: '.jpg',
        },
        [ImageGroupsIds.skulls]: {
            photos: [1, 1, 1, 1, 1],
        },
        [ImageGroupsIds.anime]: {
            photos: [1, 1, 1, 1, 1],
            extension: '.jpg',
        },
        [ImageGroupsIds.clover]: {
            photos: [1, 1, 1, 1],
            extension: '.jpg',
        },
        [ImageGroupsIds.army]: {
            photos: [1, 1, 1, 1],
            extension: '.jpg',
        },
        [ImageGroupsIds.space]: {
            photos: [1, 1, 1, 1],
            extension: '.jpg',
        },
        [ImageGroupsIds.badguys]: {
            photos: [1, 2, 1, 2, 1, 1, 1],
            extension: '.jpg',
        },
        [ImageGroupsIds.resident]: {
            photos: [5, 2, 3, 1, 2, 1, 1, 2, 3],
            extension: '.jpg',
        },
        [ImageGroupsIds.reAnime]: {
            photos: [1, 1, 1, 1, 1, 1, 1],
            extension: '.jpg',
        },
        [ImageGroupsIds.reGirls]: {
            photos: [1, 1, 3, 1, 1],
            extension: '.jpg',
        },
    },
};
