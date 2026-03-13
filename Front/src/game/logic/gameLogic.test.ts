import { TooltipTypes } from '../constants';
import reducer, {
    applyPirateChanges,
    highlightHumanMoves,
    initMap,
    initPiratePositions,
    initTeams,
    setCurrentHumanTeam,
} from '../redux/gameSlice';
import { getMapData } from '../redux/mapDataForTests';
import { GameState } from '../types';
import { GameTeamResponse } from '../types/gameSaga';
import { CalcTooltipType } from './components/calcTooltipType';
import { InitPirate } from './components/initPirate';
import { Constants, ImageGroupsIds, ImagesPacksIds } from '/app/constants';
import { PlayerTypes } from '/common/constants';

const testTeamId = 12;

const twoTeamsData: GameTeamResponse[] = [
    {
        id: 1,
        name: 'girls',
        coins: 0,
        userId: 0,
        isHuman: false,
        ship: {
            x: 5,
            y: 0,
        },
    },
    {
        id: testTeamId,
        name: 'boys',
        coins: 0,
        userId: 2,
        isHuman: true,
        isCurrentUser: true,
        ship: {
            x: 5,
            y: 10,
        },
    },
];

const testPirates: GamePirate[] = [
    {
        id: '100',
        teamId: testTeamId,
        position: {
            level: 0,
            x: 2,
            y: 0,
        },
        photo: '',
        photoId: 0,
        type: Constants.pirateTypes.Usual,
    },
];

const getState = (pirates: GamePirate[]): GameState => ({
    fields: [[]],
    lastMoves: [],
    gameSettings: {
        mapSize: 5,
        cellSize: 50,
        pirateSize: 15,
        tilesPackNames: [],
    },
    userSettings: {
        groups: [ImageGroupsIds.girls, ImageGroupsIds.redalert, ImageGroupsIds.orcs, ImageGroupsIds.skulls],
        mapSize: 11,
        hasChessBar: false,
        players: [PlayerTypes.Human, PlayerTypes.Robot2, PlayerTypes.Robot, PlayerTypes.Robot2],
        playersMode: 4,
        gameSpeed: 0,
        imagesPackName: ImagesPacksIds.classic,
    },
    stat: {
        turnNumber: 1,
        currentTeamId: testTeamId,
        currentUserId: 2,
        isCurrentUsersMove: true,
        isGameOver: false,
        gameMessage: '',
    },
    teams: [],
    pirates: pirates,
    currentHumanTeamId: 0,
    highlight_x: 0,
    highlight_y: 0,
    hasPirateAutoChange: true,
    includeMovesWithRum: false,
});

describe('CalcTooltipType tests', () => {
    let defaultState: GameState;

    beforeAll(() => {
        defaultState = getState(testPirates);
        defaultState = reducer(defaultState, initMap(getMapData));
        defaultState = reducer(defaultState, initTeams(twoTeamsData));
        defaultState = reducer(defaultState, initPiratePositions());
        defaultState = reducer(defaultState, setCurrentHumanTeam());
        defaultState = reducer(
            defaultState,
            highlightHumanMoves({
                moves: [
                    {
                        moveNum: 1,
                        from: { pirateIds: ['100'], level: 0, x: 2, y: 0 },
                        to: { pirateIds: ['100'], level: 0, x: 2, y: 0 },
                        withCoin: false,
                        withBigCoin: false,
                        withRespawn: false,
                        withRumBottle: false,
                        withQuake: false,
                        withLighthouse: false,
                    },
                    {
                        moveNum: 2,
                        from: { pirateIds: ['100'], level: 0, x: 2, y: 0 },
                        to: { pirateIds: ['100'], level: 0, x: 2, y: 3 },
                        withCoin: false,
                        withBigCoin: false,
                        withRespawn: false,
                        withRumBottle: false,
                        withQuake: false,
                        withLighthouse: false,
                    },
                    {
                        moveNum: 3,
                        from: { pirateIds: ['100'], level: 0, x: 2, y: 0 },
                        to: { pirateIds: ['100'], level: 0, x: 3, y: 3 },
                        withCoin: false,
                        withBigCoin: false,
                        withRespawn: false,
                        withRumBottle: false,
                        withQuake: true,
                        withLighthouse: false,
                    },
                ],
            }),
        );
    });

    test('Пропускаем ход', () => {
        const row = 0;
        const col = 2;
        const result = CalcTooltipType({
            row,
            col,
            field: defaultState.fields[row][col],
            state: defaultState,
        });
        expect(result).toEqual(TooltipTypes.SkipMove);
    });

    test('Прыгаем в воду', () => {
        const newState = reducer(
            defaultState,
            applyPirateChanges({
                changes: [
                    {
                        id: '100',
                        type: Constants.pirateTypes.Usual,
                        teamId: testTeamId,
                        position: { level: 0, x: 2, y: 2 },
                    },
                ],
                moves: [],
            }),
        );

        let row = 4;
        let col = 2;
        const result = CalcTooltipType({
            row,
            col,
            field: newState.fields[row][col],
            state: newState,
        });
        expect(result).toEqual(TooltipTypes.Seajump);
    });

    test('Прыгаем на пушку', () => {
        let row = 3;
        let col = 2;
        const result = CalcTooltipType({
            row,
            col,
            field: defaultState.fields[row][col],
            state: defaultState,
        });
        expect(result).toEqual(TooltipTypes.Seajump);
    });

    test('Разыгрываем пушку при разломе', () => {
        let row = 3;
        let col = 3;
        const result = CalcTooltipType({
            row,
            col,
            field: defaultState.fields[row][col],
            state: defaultState,
        });
        expect(result).toEqual(TooltipTypes.NoTooltip);
    });
});

describe('InitPirates tests', () => {
    test('добавляем бенгана и пирата', () => {
        const groupInfo = {
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
            ],
            extension: '.jpg',
        };
        const gannPhotos = [
            {
                subTypeCount: 1,
            },
            {
                subTypeCount: 1,
            },
            {
                subTypeCount: 1,
            },
        ];
        const girls: GamePirateInitiation[] = [
            {
                teamId: 1,
                photoId: 0,
                type: Constants.pirateTypes.Usual,
            },
            {
                teamId: 1,
                photoId: 0,
                type: Constants.pirateTypes.Usual,
            },
            {
                teamId: 2,
                photoId: 1,
                type: Constants.pirateTypes.BenGunn,
            },
            {
                teamId: 2,
                photoId: 2,
                type: Constants.pirateTypes.BenGunn,
            },
        ];
        girls.forEach((it) => {
            const initPhoto = InitPirate({
                girlType: it.type,
                allGirls: girls,
                teamId: 1,
                imageGroupId: ImageGroupsIds.anime,
                teamGroup: groupInfo,
                gannPhotos,
            });
            it.photoId = initPhoto.photoId;
        });

        girls.push({
            teamId: 1,
            photoId: InitPirate({
                girlType: Constants.pirateTypes.BenGunn,
                allGirls: girls,
                teamId: 1,
                imageGroupId: ImageGroupsIds.anime,
                teamGroup: groupInfo,
                gannPhotos,
            }).photoId,
            type: Constants.pirateTypes.BenGunn,
        });

        girls.push({
            teamId: 1,
            photoId: InitPirate({
                girlType: Constants.pirateTypes.Usual,
                allGirls: girls,
                teamId: 1,
                imageGroupId: ImageGroupsIds.anime,
                teamGroup: groupInfo,
                gannPhotos,
            }).photoId,
            type: Constants.pirateTypes.Usual,
        });

        let old = 0;
        girls
            .filter((it) => it.type === Constants.pirateTypes.Usual)
            .map((it) => it.photoId)
            .sort()
            .forEach((it) => {
                expect(it).not.toEqual(old);
                old = it!;
            });
    });
});
