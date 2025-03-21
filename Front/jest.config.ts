import { JestConfigWithTsJest } from 'ts-jest';

const config: JestConfigWithTsJest = {
    roots: ['<rootDir>/src'],
    testMatch: ['<rootDir>/src/**/*.test.ts'],
    preset: 'ts-jest',
    modulePaths: ['<rootDir>/src'],
    moduleNameMapper: {
        '/app/(.*)': ['<rootDir>/src/app/$1'],
        '/common/(.*)': ['<rootDir>/src/common/$1'],
        '/game/(.*)': ['<rootDir>/src/game/$1'],
    },
};

export default config;
