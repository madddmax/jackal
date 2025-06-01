import { Button } from 'react-bootstrap';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import { getUserSettings, saveMySettings } from '../../../game/redux/gameSlice';
import GameSettingsForm from './components/gameSettingsForm';
import { Constants } from '/app/constants';
import gameHub from '/game/hub/gameHub';
import { GameSettingsExt } from '/game/types/hubContracts';

const Newgame = () => {
    const dispatch = useDispatch();
    const navigate = useNavigate();

    const userSettings = useSelector(getUserSettings);

    const newStart = () => {
        navigate('/');
        saveToLocalStorage();

        if (formData) {
            gameHub.startGame(formData);
        }
    };

    const createNetGame = () => {
        navigate('/netcreate');

        if (formData) {
            gameHub.netCreate(formData);
        }
    };

    const saveToLocalStorage = (hasStoredMapCode: boolean = true) => {
        if (formData) {
            dispatch(
                saveMySettings({
                    ...userSettings,
                    groups: formData.groups,
                    mapSize: formData.mapSize,
                    players: formData.members,
                    playersMode:
                        formData.gameMode === Constants.gameModeTypes.TwoPlayersInTeam ? 8 : formData.players.length,
                    mapId: hasStoredMapCode ? formData.mapId : undefined,
                    tilesPackName: formData.tilesPackName,
                }),
            );
        }
    };

    let formData: GameSettingsExt | undefined;
    const getFormData = (data: GameSettingsExt) => (formData = data);

    return (
        <Container>
            <Row className="justify-content-center">
                <GameSettingsForm onChange={getFormData} saveToLocalStorage={saveToLocalStorage}>
                    <>
                        <Button variant="primary" type="submit" onClick={newStart}>
                            Начать
                        </Button>
                        <Button className="float-end" variant="outline-primary" type="submit" onClick={createNetGame}>
                            Создать сетевую игру
                        </Button>
                    </>
                </GameSettingsForm>
            </Row>
        </Container>
    );
};

export default Newgame;
