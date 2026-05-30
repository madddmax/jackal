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
    beautiful = 'beautiful'
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
            name: 'Бен Кот',
            description: 'Годами живет на острове в полном одиночестве, питаясь рыбой и диким мышами, и мечтает лишь об одном — чтобы его поскорее забрали домой и налили миску теплого молока.',
        },
        {
            subTypeCount: 1,
            name: 'Бен Лис',
            description: 'Харизматичный персонаж-одиночка. В одиночку обчистил половину тайников, но из-за своей лисьей натуры и жадности не может унести всё сам.',
        },
        {
            subTypeCount: 1,
            name: 'Бен Пёс',
            description: 'Годы одиночества и палящего солнца превратили грозного охранного пса в чудаковатого отшельника. Он до смерти соскучился по хорошей компании, чесанию за ушком и готов служить верой и правдой любой команде пиратов, которая угостит его сухарем и возьмет с собой на корабль.',
        },
        {
            subTypeCount: 1,
            name: 'Бен Волк',
            description: 'Мрачный, нелюдимый и гордый. Он постоянно рычит, общается короткими фразами и держится на расстоянии. Однако в глубине души он истосковался по настоящему пиратскому делу и готов примкнуть к самой сильной и дерзкой команде.',
        },
    ],
    fridayPhotos: [
        {
            subTypeCount: 1,
            name: '',
            description: '',
        },
    ],
    imageGroups: {
        [ImageGroupsIds.girls]: {
            name: 'Фурии морей',
            description: 'Самая первая созданная команда для настольной игры «Шакал». Эти шесть грозных пираток славятся своей безжалостностью, идеальной командной работой и способностью выживать в самых суровых условиях необитаемых островов. Пока мужчины спорят о дележке рома, «Фурии» методично выносят золото Шакала сундуками.',
            photos: [
                {
                    subTypeCount: 1,
                    name: 'Джесс «Буря»',
                    description: 'Лидер команды. Высокая девушка в роскошном потрепанном камзоле, с треуголкой набекрень и неизменной подзорной трубой. Дерзкая, расчетливая, никогда не паникует.'
                },
                {
                    subTypeCount: 1,
                    name: 'Мэри «Порох»',
                    description: 'Боец и канонир. Сильная и опасная пиратка. Хотя на лице у неё добрая улыбка. Но за поясом торчит пара ножей, а в руках — мушкет.'
                },
                {
                    subTypeCount: 1,
                    name: 'Кора «Призрак»',
                    description: 'Разведчик и картограф. Хрупкая, изящная, одетая в бесшумные одежды. Скрытная и молчаливая - идеальный шпион.'
                },
                {
                    subTypeCount: 1,
                    name: 'Рыжая Бесс',
                    description: 'Бортмеханик и эксперт по артиллерии. Вечно взъерошенная девушка в обгоревшем кожаном фартуке. Гиперактивная, обожает порох и сложные механизмы.'
                },
                {
                    subTypeCount: 1,
                    name: 'Гвен «Стальная»',
                    description: 'Прирожденный лидер и тактик. Высокая женщина с суровым взглядом, с повязкой на одном глазу и фамильной абордажной саблей. Хладнокровная, авторитетная, не прощает ошибок.'
                },
                {
                    subTypeCount: 1,
                    name: 'Ловкая Джули',
                    description: 'Главный инкассатор команды. Молодая, ловкая гимнастка с легким заплечным мешком для добычи. Хвастливая, азартная, невероятно быстрая на руку.'
                },
            ],
            extension: '',
        },
        [ImageGroupsIds.caribian]: {
            name: '',
            description: '',
            photos: [
                {
                    subTypeCount: 2,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 5,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 3,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
            ],
        },
        [ImageGroupsIds.somali]: {
            name: '',
            description: '',
            photos: [
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
            ],
        },
        [ImageGroupsIds.redalert]: {
            name: '',
            description: '',
            photos: [
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
            ],
            extension: '.jpg',
        },
        [ImageGroupsIds.orcs]: {
            name: '',
            description: '',
            photos: [
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
            ],
            extension: '.jpg',
        },
        [ImageGroupsIds.skulls]: {
            name: '',
            description: '',
            photos: [
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
            ],
        },
        [ImageGroupsIds.anime]: {
            name: '',
            description: '',
            photos: [
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
            ],
            extension: '.jpg',
        },
        [ImageGroupsIds.clover]: {
            name: '',
            description: '',
            photos: [
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
            ],
            extension: '.jpg',
        },
        [ImageGroupsIds.army]: {
            name: '',
            description: '',
            photos: [
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
            ],
            extension: '.jpg',
        },
        [ImageGroupsIds.space]: {
            name: '',
            description: '',
            photos: [
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
            ],
            extension: '.jpg',
        },
        [ImageGroupsIds.badguys]: {
            name: 'Плохие парни',
            description: 'Это уникальная вымышленная банда, собранная из культовых кинозлодеев и плохих парней разных эпох. Они не знают пощады, презирают законы моря и пришли на остров только за тем, чтобы забрать абсолютно всё золото и уничтожить конкурентов.',
            photos: 
            [
                {
                    subTypeCount: 2,
                    name: 'Дарт Вейдер',
                    description:
                        'Командующий армией Галактической Империи, главный символ её мощи и страха. Одет в чёрный бронированный костюм и шлем с механическим дыханием. Властный, хладнокровный, требующий абсолютного подчинения.',
                },
                {
                    subTypeCount: 2,
                    name: 'Антон Чигур',
                    description:
                        'Совершенно хладнокровен, не испытывает никакого раскаяния, и встреча с ним для большинства персонажей оказывается смертельной. Чигур считает себя орудием судьбы. Участь нескольких встреченных им персонажей он решает при помощи подбрасывания монеты.',
                },
                {
                    subTypeCount: 1,
                    name: 'T-1000',
                    description: 'Робот-терминатор, прибывший из будущего, чтобы убить Джона Коннора.',
                },
                {
                    subTypeCount: 1,
                    name: 'Джокер',
                    description:
                        'Безумный криминальный гений в образе клоуна. Стал символом хаоса и анархии, а его личность и происхождение остаются загадкой, хотя в разных историях упоминаются имена Артур Флек или Джек Napier.',
                },
                {
                    subTypeCount: 1,
                    name: 'Винсент Вега',
                    description:
                        `Харизматичный гангстер, склонный к философским беседам и нелепым случайностям. Носит костюм, вооружен никелированным пистолетом M1911A1.`,
                },
                {
                    subTypeCount: 1,
                    name: 'Джулс Уиннфилд',
                    description:
                        'Киллер, работающий на Марселласа Уоллеса вместе с напарником Винсентом Вегой. После чудесного спасения решает уйти из криминала, чтобы начать новую жизнь. Любит цитировать вымышленную версию Иезекииля 25:17 Путь праведника труден...',
                },
                {
                    subTypeCount: 1,
                    name: 'Чернокнижник',
                    description:
                        'Могущественный, харизматичный и жестокий приспешник Сатаны из XVII века, перенесшийся в современность. Он элегантен, циничен и обладает обширными чёрными магическими знаниями, стремится собрать «Великий Гримуар».',
                },
                {
                    subTypeCount: 2,
                    name: 'Харли Квинн',
                    description:
                        'Суперзлодейка, позже антигероиня вселенной DC Comics. Псевдоним персонажа образован из её настоящего имени, Харлин Квинзель, как ассоциация со словом «арлекин».',
                },
            ],
            extension: '.jpg',
        },
        [ImageGroupsIds.resident]: {
            name: '',
            description: '',
            photos: [
                {
                    subTypeCount: 5,
                    name: 'Джилл Валентайн',
                    description: 'Эксперт по взломам в команде Альфа полицейского спецотряда «S.T.A.R.S.»',
                },
                {
                    subTypeCount: 3,
                    name: 'Клэр Редфилд',
                    description:
                        'Младшая сестра Криса Редфилда. Носит куртку с логотипом «Сделано на небесах» (англ. Made in Heaven) и ножны для табельного ножа «S.T.A.R.S.», подаренные Крисом.',
                },
                {
                    subTypeCount: 4,
                    name: 'Ада Вонг',
                    description:
                        'О прошлом Ады ничего неизвестно, как и о том, где и когда она родилась. Отличается уникальными физическими данными, сильным характером и острым умом.',
                },
                {
                    subTypeCount: 1,
                    name: 'Крис Редфилд',
                    description: 'Старший брат Клэр Редфилда.',
                },
                {
                    subTypeCount: 2,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 3,
                    name: 'Ребекка',
                    description:
                        'Советник Альянса Противодействия Биотерроризму (АПБ) и бывший медик в отряде «Браво».',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 2,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 3,
                    name: '',
                    description: '',
                },
            ],
            extension: '.jpg',
        },
        [ImageGroupsIds.reAnime]: {
            name: '',
            description: '',
            photos: [
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
            ],
            extension: '.jpg',
        },
        [ImageGroupsIds.reGirls]: {
            name: '',
            description: '',
            photos: [
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
                },
                {
                    subTypeCount: 1,
                    name: '',
                    description: '',
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
        [ImageGroupsIds.beautiful]: {
            name: 'Прекрасные',
            description: 'Это вымышленная, но самая душевная команда в игре, состоящая из реальных девушек. Эта банда действует не грубой силой, а женской интуицией, хитростью и абсолютным взаимопониманием.',
            photos: [
                {
                    subTypeCount: 1,
                    name: 'Кристина',
                    description: 'Секретное оружие команды. Будучи профессионалом, она знает, как устроен этот остров изнутри. Видит скрытые алгоритмы там, где остальные видят лишь слепую удачу. Кристина питает слабость к блеску золотых монет, поэтому находит их быстрее всех.',
                },
                {
                    subTypeCount: 1,
                    name: 'Повелительница',
                    description: 'Повелительница семи морей. Её взгляд спокоен и пронзителен: она не ждёт попутного ветра, она сама управляет своей судьбой. Когда она смотрит на горизонт, сам Нептун убирает свой трезубец, признавая ее власть. Для пиратского братства она — высший закон. Ей не нужны пушки, чтобы побеждать; ей достаточно одного взгляда, чтобы заставить океан расступиться, а врагов — молить о пощаде.',
                },
                {
                    subTypeCount: 1,
                    name: 'Елена',
                    description: 'Елена - победоносная, покоряет врагов и союзников. Ей не нужно вступать в яростные схватки, ведь её величие, харизма и королевская уверенность действуют на окружающих магнетически.',
                },
                {
                    subTypeCount: 1,
                    name: 'Анна',
                    description: 'Анна - аналитик команды. Женщина в строгом, но помятом деловом костюме. В руках — планшет с треснувшим экраном. За её спиной — голографические карты, которые она рисует и перерисовывает прямо в воздухе, не глядя.',
                },
            ],
            extension: '.jpg',
        },
    },
};
