import { HubConnectionState } from '@microsoft/signalr';
import { useEffect } from 'react';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import { Constants } from '/app/constants';
import { hubConnection } from '/app/global';
import { getAuth } from '/auth/redux/authSlice';
import { PlayerTypes } from '/common/constants';
import gameHub from '/game/hub/gameHub';

const Madstart = () => {
    const navigate = useNavigate();

    const authInfo = useSelector(getAuth);
    const mapSizes: number[] = [7, 9, 11];
    const piratesPerPlayer: number[] = [1, 2, 3];

    const speedStart = () => {
        gameHub.startGame({
            players: [
                { userId: authInfo.user?.id ?? 0, type: PlayerTypes.Human, position: Constants.positions[0] },
                { userId: 0, type: PlayerTypes.Robot2, position: Constants.positions[2] },
            ],
            mapSize: mapSizes[Math.floor(Math.random() * mapSizes.length)],
            tilesPackName: 'imbalance',
            piratesPerPlayer: piratesPerPlayer[Math.floor(Math.random() * piratesPerPlayer.length)],
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

export default Madstart;
