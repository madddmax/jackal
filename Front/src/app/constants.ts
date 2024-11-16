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
        Usual: 'Usual',
        BenGunn: 'BenGunn',
        Friday: 'Friday',
    },
    commonGannMaxId: 4,
    commonFridayMaxId: 1,
    groupIds,
    groups: [
        {
            id: groupIds.girls,
            photoMaxId: 6,
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
            extension: '.jpg',
        },
        {
            id: groupIds.orcs,
            photoMaxId: 6,
            extension: '.jpg',
        },
        {
            id: groupIds.skulls,
            photoMaxId: 5,
        },
        {
            id: groupIds.anime,
            photoMaxId: 5,
            extension: '.jpg',
        },
    ],
};
