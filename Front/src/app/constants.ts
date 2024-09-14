const groupIds = {
    girls: 'girls',
    caribian: 'caribian',
    somali: 'somali',
    skulls: 'skulls',
    redalert: 'redalert',
    orcs: 'orcs',
    anime: 'anime',
};

export const Constants = {
    pirateTypes: {
        Base: 0,
        Gann: 1,
    },
    groupIds,
    groups: [
        {
            id: groupIds.girls,
            photoMaxId: 6,
            gannMaxId: 4,
        },
        {
            id: groupIds.caribian,
            photoMaxId: 11,
        },
        {
            id: groupIds.somali,
            photoMaxId: 8,
        },
        {
            id: groupIds.redalert,
            photoMaxId: 5,
            gannMaxId: 4,
            extension: '.jpg',
        },
        {
            id: groupIds.orcs,
            photoMaxId: 6,
            gannMaxId: 2,
            extension: '.jpg',
        },
        {
            id: groupIds.skulls,
            photoMaxId: 5,
        },
        {
            id: groupIds.anime,
            photoMaxId: 3,
            gannMaxId: 2,
            extension: '.jpg',
        },
    ],
};
