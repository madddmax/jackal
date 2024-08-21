import { JestConfigWithTsJest } from 'ts-jest';

const config: JestConfigWithTsJest = {
    roots: ['<rootDir>/src'],
    testMatch: ['<rootDir>/src/**/*.test.ts'],
    preset: 'ts-jest',
    modulePaths: ['<rootDir>/src'],
    moduleNameMapper: {
        '/app/(.*)': ['<rootDir>/src/app/$1'],
    },
};

export default config;
