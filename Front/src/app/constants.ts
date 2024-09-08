const groupIds = {
    girls: 'girls',
    caribian: 'caribian',
    somali: 'somali',
    skulls: 'skulls',
    redalert: 'redalert',
    orcs: 'orcs',
};

export const Constants = {
    pirateTypes: {
        Base: 0,
        Gann: 1,
    },
    groupIds,
    humanGroupIds: [groupIds.redalert, groupIds.caribian],
    robotGroupIds: [groupIds.skulls, groupIds.redalert, groupIds.orcs, groupIds.caribian, groupIds.somali],
    groups: [
        {
            id: groupIds.girls,
            photoMaxId: 7,
            gannMaxId: 5,
        },
        {
            id: groupIds.caribian,
            photoMaxId: 12,
        },
        {
            id: groupIds.somali,
            photoMaxId: 9,
        },
        {
            id: groupIds.redalert,
            photoMaxId: 6,
            gannMaxId: 5,
            extension: '.jpg',
        },
        {
            id: groupIds.orcs,
            photoMaxId: 7,
            gannMaxId: 3,
            extension: '.jpg',
        },
        {
            id: groupIds.skulls,
            photoMaxId: 6,
        },
    ],
};
