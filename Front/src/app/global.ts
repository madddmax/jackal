import config from './config';

export const uuidGen = () => {
    return '10000000-1000-4000-8000-100000000000'.replace(/[018]/g, (c) =>
        (+c ^ (crypto.getRandomValues(new Uint8Array(1))[0] & (15 >> (+c / 4)))).toString(16),
    );
};

export const getRandomValues = (min: number, max: number, count: number): number[] => {
    if (max - min < count) return [];

    let arr = [] as number[];
    while (arr.length < count) {
        let x = Math.floor(Math.random() * (max - min) + min);
        if (!arr.includes(x)) {
            arr.push(x);
        }
    }
    return arr;
};

export const getAnotherRandomValue = (min: number, max: number, except: number[]): number => {
    if (max - min < except.length) return 0;

    let num = except.length > 0 ? except[0] : min;
    while (except.includes(num)) {
        num = Math.floor(Math.random() * (max - min) + min);
    }
    return num;
};

export const debugLog = (message?: any, ...optionalParams: any[]) => {
    if (config.HasDebug) console.log(message, optionalParams);
};
