export enum ImagesPacksIds {
    classic = 'classic',
    space = 'space',
    dendy_8_bit = 'dendy_8_bit',
    black_and_white = 'black_and_white',
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
        [ImagesPacksIds.black_and_white]: '/fields_black_and_white/',
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
    gannPhotos: [
        {
            subTypeCount: 1,
        },
        {
            subTypeCount: 1,
        },
        {
            subTypeCount: 1,
        },
        {
            subTypeCount: 1,
        },
    ],
    fridayPhotos: [
        {
            subTypeCount: 1,
        },
    ],
    imageGroups: {
        [ImageGroupsIds.girls]: {
            photos: [
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
            ],
            extension: '',
        },
        [ImageGroupsIds.caribian]: {
            photos: [
                {
                    subTypeCount: 2,
                },
                {
                    subTypeCount: 5,
                },
                {
                    subTypeCount: 3,
                },
                {
                    subTypeCount: 1,
                },
            ],
        },
        [ImageGroupsIds.somali]: {
            photos: [
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
            ],
        },
        [ImageGroupsIds.redalert]: {
            photos: [
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
            ],
            extension: '.jpg',
        },
        [ImageGroupsIds.orcs]: {
            photos: [
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
            ],
            extension: '.jpg',
        },
        [ImageGroupsIds.skulls]: {
            photos: [
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
            ],
        },
        [ImageGroupsIds.anime]: {
            photos: [
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
            ],
            extension: '.jpg',
        },
        [ImageGroupsIds.clover]: {
            photos: [
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
            ],
            extension: '.jpg',
        },
        [ImageGroupsIds.army]: {
            photos: [
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
            ],
            extension: '.jpg',
        },
        [ImageGroupsIds.space]: {
            photos: [
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
            ],
            extension: '.jpg',
        },
        [ImageGroupsIds.badguys]: {
            photos: [
                {
                    subTypeCount: 1,
                    name: 'Дарт Вейдер',
                    description:
                        'Командующий армией Галактической Империи, главный символ её мощи и страха. Один из ситхов. Одет в чёрный бронированный костюм и шлем с механическим дыханием. Костюм — это система жизнеобеспечения, поддерживающая его после тяжёлых ран.',
                },
                {
                    subTypeCount: 2,
                    name: 'Антон Чигур',
                    description:
                        'Совершенно хладнокровен, не испытывает никакого раскаяния, и встреча с ним для большинства персонажей оказывается смертельной. Чигур считает себя орудием судьбы. Участь нескольких встреченных им персонажей он решает при помощи подбрасывания монеты.',
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 2,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                    name: 'Повелительница',
                    description:
                        'Её взгляд спокоен и пронзителен: она не ждёт попутного ветра, она сама управляет своей судьбой. Когда она смотрит на горизонт, сам Нептун убирает свой трезубец, признавая ее власть. Для пиратского братства она — высший закон. Ей не нужны пушки, чтобы побеждать; ей достаточно одного взгляда, чтобы заставить океан расступиться, а врагов — молить о пощаде.',
                },
                {
                    subTypeCount: 1,
                    name: 'Чернокнижник',
                    description:
                        'Могущественный, харизматичный и жестокий приспешник Сатаны из XVII века, перенесшийся в современность. Он элегантен, циничен и обладает обширными чёрными магическими знаниями, стремится собрать «Великий Гримуар».',
                },
                {
                    subTypeCount: 1,
                    name: 'Харли Квинн',
                    description:
                        'Суперзлодейка, позже антигероиня вселенной DC Comics. Псевдоним персонажа образован из её настоящего имени, Харлин Квинзель, как ассоциация со словом «арлекин».',
                },
            ],
            extension: '.jpg',
        },
        [ImageGroupsIds.resident]: {
            photos: [
                {
                    subTypeCount: 5,
                },
                {
                    subTypeCount: 2,
                },
                {
                    subTypeCount: 3,
                    name: 'Ада Вонг',
                    description:
                        'О прошлом Ады ничего неизвестно, как и о том, где и когда она родилась. Отличается уникальными физическими данными, сильным характером и острым умом.',
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 2,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 2,
                },
                {
                    subTypeCount: 3,
                },
            ],
            extension: '.jpg',
        },
        [ImageGroupsIds.reAnime]: {
            photos: [
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
            ],
            extension: '.jpg',
        },
        [ImageGroupsIds.reGirls]: {
            photos: [
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 1,
                },
                {
                    subTypeCount: 3,
                    name: 'Ребекка',
                    description:
                        'Советник Альянса Противодействия Биотерроризму (АПБ) и бывший медик в отряде «Браво».',
                },
                {
                    subTypeCount: 1,
                    name: 'Ада Вонг',
                    description:
                        'О прошлом Ады ничего неизвестно, как и о том, где и когда она родилась. Отличается уникальными физическими данными, сильным характером и острым умом.',
                },
                {
                    subTypeCount: 1,
                },
            ],
            extension: '.jpg',
        },
    },
};
