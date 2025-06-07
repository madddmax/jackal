import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import dayjs from 'dayjs';
import { NavigateFunction } from 'react-router-dom';

import config from './config';
import { Constants } from './constants';
import { PlayersInfo } from './content/layout/components/types';
import { GamePlayer, GameSettings, GameSettingsFormData } from '/game/types/hubContracts';

export const uuidGen = () => {
    return '10000000-1000-4000-8000-100000000000'.replace(/[018]/g, (c) =>
        (+c ^ (crypto.getRandomValues(new Uint8Array(1))[0] & (15 >> (+c / 4)))).toString(16),
    );
};

export const getRandomValues = (min: number, max: number, count: number): number[] => {
    if (max - min + 1 < count) return [];

    const arr = [] as number[];
    while (arr.length < count) {
        const x = Math.floor(Math.random() * (max - min + 1) + min);
        if (!arr.includes(x)) {
            arr.push(x);
        }
    }
    return arr;
};

export const getAnotherRandomValue = (min: number, max: number, except: number[]): number => {
    if (max - min + 1 <= except.length) return min;

    let num = except.length > 0 ? except[0] : min;
    while (except.includes(num)) {
        num = Math.floor(Math.random() * (max - min + 1) + min);
    }
    return num;
};

export interface fromNowStruct {
    value: number;
    unit: string;
    color: string;
}

export const fromNow = (ts: number) => {
    let inSec = Math.abs(dayjs(ts * 1000).diff(new Date(), 's'));
    if (inSec < 60) {
        return {
            value: inSec,
            unit: 'с',
            color: 'green',
        };
    }
    inSec = Math.ceil(inSec / 60);
    return {
        value: inSec,
        unit: 'м',
        color: inSec < 3 ? 'orange' : 'red',
    };
};

export const debugLog = (message?: unknown, ...optionalParams: unknown[]) => {
    if (config.HasDebug) console.log(message, optionalParams);
};

interface History {
    navigate?: NavigateFunction;
}

export const history: History = {};

export const hubConnection = new HubConnectionBuilder()
    .withUrl(config.HubApi, { accessTokenFactory: () => localStorage.auth })
    .withAutomaticReconnect()
    .configureLogging(LogLevel.Information)
    .build();

export interface GirlsLevel {
    level: number;
    levelsCountInCell: number;
    girls: string[] | undefined;
}

export interface GirlsPositions {
    Map: { [id: number]: GirlsLevel };
    AddPosition: (it: GamePiratePosition, levelsCount: number) => void;
    RemovePosition: (it: GamePiratePosition) => void;
    GetPosition: (it: GamePiratePosition) => GirlsLevel | undefined;
    ScrollGirls: (pos: GirlsLevel) => void;
}

// словарь, отслеживающий размещение нескольких пираток на одной клетке
// для корректного их смещения относительно друг друга
export const girlsMap: GirlsPositions = {
    Map: {},
    AddPosition: function (it: GamePiratePosition, levelsCount: number) {
        const cachedId = it.position.y * 1000 + it.position.x * 10 + it.position.level;
        const level = this.Map[cachedId];
        if (!level) {
            this.Map[cachedId] = {
                level: it.position.level,
                levelsCountInCell: levelsCount,
                girls: [it.id],
            };
        } else {
            if (level.girls) {
                level.girls.push(it.id);
            } else {
                level.girls = [it.id];
            }
        }
    },
    RemovePosition: function (it: GamePiratePosition) {
        const cachedId = it.position.y * 1000 + it.position.x * 10 + it.position.level;
        const girlsLevel = this.Map[cachedId];
        if (girlsLevel?.girls != undefined) {
            girlsLevel.girls = girlsLevel.girls.filter((girl) => girl != it.id);
            if (girlsLevel.girls.length == 0) delete this.Map[cachedId];
        }
    },
    GetPosition: function (it: GamePiratePosition): GirlsLevel | undefined {
        const cachedId = it.position.y * 1000 + it.position.x * 10 + it.position.level;
        return this.Map[cachedId];
    },
    ScrollGirls: function (pos: GirlsLevel) {
        if (pos && pos.girls && pos.girls.length > 1) {
            pos.girls.push(pos.girls.shift()!);
        }
    },
};

const convertPlayers = (data: PlayersInfo): GamePlayer[] => {
    const { users, members, mode } = data;
    if (mode == 1)
        return [{ userId: members[0] === 'human' ? users[0] : 0, type: members[0], position: Constants.positions[0] }];
    else if (mode == 2)
        return [
            { userId: members[0] === 'human' ? users[0] : 0, type: members[0], position: Constants.positions[0] },
            { userId: members[2] === 'human' ? users[0] : 0, type: members[2], position: Constants.positions[2] },
        ];
    else
        return members.map((it, index) => ({
            userId: it === 'human' ? users[index] : 0,
            type: it,
            position: Constants.positions[index],
        }));
};

export const convertToSettings = (data: GameSettingsFormData): GameSettings => ({
    players: convertPlayers(data.players),
    mapId: data.mapId,
    mapSize: data.mapSize,
    tilesPackName: data.tilesPackName,
    gameMode: data.players.mode == 8 ? Constants.gameModeTypes.TwoPlayersInTeam : Constants.gameModeTypes.FreeForAll,
});

export const convertToMembers = (data: GamePlayer[], defaults: string[]): string[] => {
    if (data.length == 1) return data.map((it) => it.type.toLocaleLowerCase()).concat(defaults.slice(1));
    if (data.length == 2) {
        return [data[0].type.toLocaleLowerCase(), defaults[1], data[1].type.toLocaleLowerCase(), defaults[3]];
    } else return data.map((it) => it.type.toLocaleLowerCase());
};
