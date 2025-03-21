import { NavigateFunction } from 'react-router-dom';
import config from './config';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { GameTurnResponse, PiratePosition } from '../common/redux.types';

export const uuidGen = () => {
    return '10000000-1000-4000-8000-100000000000'.replace(/[018]/g, (c) =>
        (+c ^ (crypto.getRandomValues(new Uint8Array(1))[0] & (15 >> (+c / 4)))).toString(16),
    );
};

export const getRandomValues = (min: number, max: number, count: number): number[] => {
    if (max - min + 1 < count) return [];

    let arr = [] as number[];
    while (arr.length < count) {
        let x = Math.floor(Math.random() * (max - min + 1) + min);
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

export const debugLog = (message?: any, ...optionalParams: any[]) => {
    if (config.HasDebug) console.log(message, optionalParams);
};

interface History {
    navigate?: NavigateFunction;
}

export const history: History = {};

export const hubConnection = new HubConnectionBuilder()
    .withUrl(config.HubApi)
    .withAutomaticReconnect()
    .configureLogging(LogLevel.Information)
    .build();

export const animateQueue: GameTurnResponse[] = [];

export interface GirlsLevel {
    level: number;
    levelsCountInCell: number;
    girls: string[] | undefined;
}

export interface GirlsPositions {
    Map: { [id: number]: GirlsLevel };
    AddPosition: (it: PiratePosition, levelsCount: number) => void;
    RemovePosition: (it: PiratePosition) => void;
    GetPosition: (it: PiratePosition) => GirlsLevel | undefined;
    ScrollGirls: (pos: GirlsLevel) => void;
}

// словарь, отслеживающий размещение нескольких пираток на одной клетке
// для корректного их смещения относительно друг друга
export const girlsMap: GirlsPositions = {
    Map: {},
    AddPosition: function (it: PiratePosition, levelsCount: number) {
        let cachedId = it.position.y * 1000 + it.position.x * 10 + it.position.level;
        let level = this.Map[cachedId];
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
    RemovePosition: function (it: PiratePosition) {
        let cachedId = it.position.y * 1000 + it.position.x * 10 + it.position.level;
        let girlsLevel = this.Map[cachedId];
        if (girlsLevel?.girls != undefined) {
            girlsLevel.girls = girlsLevel.girls.filter((girl) => girl != it.id);
            if (girlsLevel.girls.length == 0) delete this.Map[cachedId];
        }
    },
    GetPosition: function (it: PiratePosition): GirlsLevel | undefined {
        let cachedId = it.position.y * 1000 + it.position.x * 10 + it.position.level;
        return this.Map[cachedId];
    },
    ScrollGirls: function (pos: GirlsLevel) {
        if (pos && pos.girls && pos.girls.length > 1) {
            pos.girls.push(pos.girls.shift()!);
        }
    },
};
