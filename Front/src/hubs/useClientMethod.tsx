import { HubConnection } from '@microsoft/signalr';
import { useEffect } from 'react';
import { debugLog } from '/app/global';

const useClientMethod = (
    useSockets: boolean,
    hubConnection: HubConnection | undefined,
    methodName: string,
    method: (...args: any[]) => void,
) => {
    useEffect(() => {
        debugLog('useClientMethod', methodName, hubConnection);

        if (!hubConnection) {
            return;
        }

        if (useSockets) {
            hubConnection.on(methodName, method);

            return () => {
                hubConnection.off(methodName, method);
            };
        } else {
            hubConnection.off(methodName, method);
        }
    }, [hubConnection, useSockets, method, methodName]);
};

export default useClientMethod;
