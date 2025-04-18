import { HubConnection } from '@microsoft/signalr';
import { useEffect } from 'react';

import { debugLog } from '/app/global';

const useClientMethod = (
    enableSockets: boolean,
    hubConnection: HubConnection | undefined,
    methodName: string,
    method: (...args: unknown[]) => void,
) => {
    useEffect(() => {
        debugLog('useClientMethod', methodName, hubConnection);

        if (!hubConnection) {
            return;
        }

        if (enableSockets) {
            hubConnection.on(methodName, method);

            return () => {
                hubConnection.off(methodName, method);
            };
        } else {
            hubConnection.off(methodName, method);
        }
    }, [hubConnection, enableSockets, method, methodName]);
};

export default useClientMethod;
