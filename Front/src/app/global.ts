import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import dayjs from 'dayjs';
import { NavigateFunction } from 'react-router-dom';

import config from './config';
import { Constants } from './constants';
import { PlayerInfo, PlayersInfo } from './content/layout/components/types';
import { PiratePhotoIdentity } from '/common/types/common';
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

export const getAnotherRandomValue = (
    min: number,
    max: number,
    except: number[],
    photos?: number[],
): PiratePhotoIdentity => {
    if (max - min + 1 < except.length) return { type: min, subtype: min, origin: min };

    let num = except.length > 0 ? except[0] : min;
    while (except.includes(num)) {
        num = Math.floor(Math.random() * (max - min + 1) + min);
    }

    if (photos && photos.length > 0 && photos[num - 1] > 1) {
        let subnum = Math.floor(Math.random() * photos[num - 1] + min);
        return { type: num, subtype: subnum, origin: num * 10 + subnum };
    }

    return { type: num, subtype: num, origin: num };
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

const convertToPlayers = (data: PlayersInfo): GamePlayer[] => {
    const { mode, gamers } = data;

    if (mode == 1) return [{ userId: gamers[0].userId, type: gamers[0].type, position: Constants.positions[0] }];
    else if (mode == 2)
        return [
            { userId: gamers[0].userId, type: gamers[0].type, position: Constants.positions[0] },
            { userId: gamers[2].userId, type: gamers[2].type, position: Constants.positions[2] },
        ];
    else
        return gamers.map((it, index) => ({
            userId: it.userId,
            type: it.type,
            position: Constants.positions[index],
        }));
};

export const convertToSettings = (data: GameSettingsFormData): GameSettings => ({
    players: convertToPlayers(data.players),
    mapId: data.mapId,
    mapSize: data.mapSize,
    tilesPackName: data.tilesPackName,
    gameMode: data.players.mode == 8 ? Constants.gameModeTypes.TwoPlayersInTeam : Constants.gameModeTypes.FreeForAll,
});

export const convertToGamers = (data: GamePlayer[], gamers: PlayerInfo[], defaults: PlayerInfo[]): PlayerInfo[] => {
    if (data.length == 1) return data.map((it) => getGamerByPlayer(it, gamers)).concat(defaults.slice(1));
    if (data.length == 2) {
        return [getGamerByPlayer(data[0], gamers), defaults[1], getGamerByPlayer(data[1], gamers), defaults[3]];
    } else return data.map((it) => getGamerByPlayer(it, gamers));
};

const getGamerByPlayer = (man: GamePlayer, gamers: PlayerInfo[]) => {
    return man.userId > 0
        ? (gamers.find((gm) => gm.userId === man.userId) ?? gamers[0])
        : (gamers.find((gm) => gm.type === man.type.toLocaleLowerCase()) ?? gamers[0]);
};
