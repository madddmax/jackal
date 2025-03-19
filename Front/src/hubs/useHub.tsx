import { HubConnection, HubConnectionState } from '@microsoft/signalr';
import { useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import { showMessage } from '/redux/commonSlice';
import { debugLog } from '/app/global';

const useHub = (enableSockets: boolean, hubConnection?: HubConnection) => {
    const [hubConnectionState, setHubConnectionState] = useState<HubConnectionState>(
        hubConnection?.state ?? HubConnectionState.Disconnected,
    );
    const dispatch = useDispatch();

    useEffect(() => {
        debugLog('useHub', hubConnection, hubConnection?.state);

        if (!hubConnection) {
            setHubConnectionState(HubConnectionState.Disconnected);
            return;
        }
        if (!enableSockets) {
            hubConnection.stop();
            return;
        }

        if (hubConnection.state === HubConnectionState.Disconnected) {
            debugLog('start connection', hubConnection.state);
            hubConnection
                .start()
                .then(() => setHubConnectionState(hubConnection?.state))
                .catch((err) => {
                    debugLog('hubConnection error', err);
                    dispatch(
                        showMessage({
                            isError: true,
                            errorCode: err.response?.statusText,
                            messageText: 'Соединение не установлено',
                        }),
                    );
                });
        }

        return () => {
            hubConnection.stop();
        };
    }, [hubConnection, enableSockets]);

    return { hubConnectionState };
};

export default useHub;
