import { HubConnection } from '@microsoft/signalr';
import { UnknownAction } from '@reduxjs/toolkit';
import { Dispatch, useEffect } from 'react';

import { debugLog } from '/app/global';

const useClientMethod = (
    enableSockets: boolean,
    hubConnection: HubConnection | undefined,
    dispatch: Dispatch<UnknownAction>,
    methods: {
        name: string;
        sagaAction: string;
    }[],
) => {
    useEffect(() => {
        for (const key in methods) {
            debugLog('useClientMethod', key, hubConnection);
        }

        if (!hubConnection) {
            return;
        }

        const handlers = methods.map((it) => ({
            name: it.name,
            action: (data: unknown) => {
                debugLog(data);
                dispatch({ type: it.sagaAction, payload: data });
            },
        }));

        if (enableSockets) {
            handlers.forEach((it) => {
                hubConnection.on(it.name, it.action);
            });

            return () => {
                handlers.forEach((it) => {
                    hubConnection.off(it.name, it.action);
                });
            };
        } else {
            handlers.forEach((it) => {
                hubConnection.off(it.name, it.action);
            });
        }
    }, [hubConnection, enableSockets, dispatch, methods]);
};

export default useClientMethod;
