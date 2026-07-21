import { PayloadAction, createSlice } from '@reduxjs/toolkit';

import docsLogic from '../logic/docsLogic';
import { DocsPiratePosition, DocsState } from '../types/types';
import { Constants, ImageGroupsIds } from '/app/constants';

export const docsSlice = createSlice({
    name: 'docs',
    initialState: {
        pirate: {
            id: '100',
            teamId: 0,
            position: {
                level: 0,
                x: 0,
                y: 0,
            },
            photo: `${ImageGroupsIds.girls}/pirate_2.png`,
            backgroundColor: 'darkred',
            photoId: 0,
            type: Constants.pirateTypes.Usual,
        },
        availableMoves: [],
        stepOpacity: 0.5,
    } satisfies DocsState as DocsState,
    reducers: {
        setPiratePosition: (state, action: PayloadAction<DocsPiratePosition>) => {
            state.pirate.position = action.payload.position;
            state.availableMoves = docsLogic.CalcAvailableMoves(action.payload).map((pos) => ({
                pirateIds: [],
                level: 0,
                x: pos[0],
                y: pos[1],
            }));
        },
        setStepOpacity: (state, action: PayloadAction<number>) => {
            state.stepOpacity = action.payload;
        },
    },
    selectors: {
        getPirate: (state): GamePirate => state.pirate,
        getStepOpacity: (state): number => state.stepOpacity,
        hasAvailableMove: (state, x: number, y: number): boolean =>
            state.availableMoves.some((it) => it.x == x && it.y == y),
    },
});

export const { setPiratePosition, setStepOpacity } = docsSlice.actions;

export const { getPirate, hasAvailableMove, getStepOpacity } = docsSlice.selectors;

export default docsSlice.reducer;
