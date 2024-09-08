const teamIds = {
    girls: 'girls',
    caribian: 'caribian',
    somali: 'somali',
    skulls: 'skulls',
    redalert: 'redalert',
    orcs: 'orcs',
};

export const Constants = {
    teamIds,
    humanTeamIds: [teamIds.girls, teamIds.caribian],
    robotTeamIds: [teamIds.skulls, teamIds.redalert, teamIds.orcs, teamIds.caribian, teamIds.somali],
    groups: [
        {
            id: teamIds.girls,
            photoMaxId: 7,
        },
        {
            id: teamIds.caribian,
            photoMaxId: 12,
        },
        {
            id: teamIds.somali,
            photoMaxId: 9,
        },
        {
            id: teamIds.redalert,
            photoMaxId: 6,
            extension: '.jpg',
        },
        {
            id: teamIds.orcs,
            photoMaxId: 7,
            extension: '.jpg',
        },
        {
            id: teamIds.skulls,
            photoMaxId: 6,
        },
    ],
};
