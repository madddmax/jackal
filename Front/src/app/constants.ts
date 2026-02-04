const groupIds = {
    girls: 'girls',
    caribian: 'caribian',
    somali: 'somali',
    skulls: 'skulls',
    redalert: 'redalert',
    orcs: 'orcs',
    anime: 'anime',
    clover: 'clover',
    army: 'army',
    space: 'space',
    badguys: 'badguys',
    resident: 'resident',
    reAnime: 'resident anime',
    reGirls: 'resident girls',
};

export const Constants = {
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
    groupIds,
    groups: [
        {
            id: groupIds.girls,
            photos: [1, 1, 1, 1, 1, 1],
        },
        {
            id: groupIds.caribian,
            photos: [2, 5, 3, 1],
        },
        {
            id: groupIds.somali,
            photos: [1, 1, 1, 1, 1, 1, 1, 1],
        },
        {
            id: groupIds.redalert,
            photos: [1, 1, 1, 1, 1],
            extension: '.jpg',
        },
        {
            id: groupIds.orcs,
            photos: [1, 1, 1, 1, 1, 1],
            extension: '.jpg',
        },
        {
            id: groupIds.skulls,
            photos: [1, 1, 1, 1, 1],
        },
        {
            id: groupIds.anime,
            photos: [1, 1, 1, 1, 1],
            extension: '.jpg',
        },
        {
            id: groupIds.clover,
            photos: [1, 1, 1, 1],
            extension: '.jpg',
        },
        {
            id: groupIds.army,
            photos: [1, 1, 1, 1],
            extension: '.jpg',
        },
        {
            id: groupIds.space,
            photos: [1, 1, 1, 1],
            extension: '.jpg',
        },
        {
            id: groupIds.badguys,
            photos: [1, 2, 1, 2, 1, 1],
            extension: '.jpg',
        },
        {
            id: groupIds.resident,
            photos: [5, 2, 3, 1, 2, 1, 1, 2, 3],
            extension: '.jpg',
        },
        {
            id: groupIds.reAnime,
            photos: [1, 1, 1, 1, 1, 1, 1],
            extension: '.jpg',
        },
        {
            id: groupIds.reGirls,
            photos: [1, 1, 1],
            extension: '.jpg',
        },
    ],
};
