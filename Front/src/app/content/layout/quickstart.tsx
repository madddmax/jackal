import { HubConnectionState } from '@microsoft/signalr';
import { useEffect } from 'react';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import { getUserSettings } from '../../../game/redux/gameSlice';
import { Constants } from '/app/constants';
import { hubConnection } from '/app/global';
import { getAuth } from '/auth/redux/authSlice';
import gameHub from '/game/hub/gameHub';

const Quickstart = () => {
    const navigate = useNavigate();

    const authInfo = useSelector(getAuth);
    const userSettings = useSelector(getUserSettings);

    const speedStart = () => {
        gameHub.startGame({
            players: [
                { userId: authInfo.user?.id ?? 0, type: 'human', position: Constants.positions[0] },
                { userId: 0, type: 'robot2', position: Constants.positions[2] },
            ],
            mapId: userSettings.mapId,
            mapSize: 11,
            tilesPackName: userSettings.tilesPackName,
        });
    };

    useEffect(() => {
        if (authInfo.isAuthorised && hubConnection?.state == HubConnectionState.Connected) {
            navigate('/');
            speedStart();
        }
    }, [authInfo.isAuthorised, hubConnection?.state]);

    return <>Ждите... Идёт загрузка...</>;
};

export default Quickstart;
