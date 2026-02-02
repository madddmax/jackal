import Image from 'react-bootstrap/Image';

import classes from './players.module.less';
import { Constants } from '/app/constants';
import { GetPlayerTypePicture } from '/common/constants';

interface PlayerProps {
    id: string;
    position: number;
    type: string;
    userName?: string;
    group: number;
    posInfo?: string;
    changePlayer: () => void;
    changeGroup: () => void;
}

const Player = ({ id, position, type, userName, group, posInfo, changePlayer, changeGroup }: PlayerProps) => {
    const getTopPosition = (pos: number) => {
        if (pos === 0) return 200;
        else if (pos === 2) return 0;
        return 100;
    };

    const getLeftPosition = (pos: number) => {
        if (pos === 1) return 0;
        else if (pos === 3) return 200;
        return 100;
    };

    return (
        <div
            id={id}
            className={classes.player}
            style={{
                top: getTopPosition(position),
                left: getLeftPosition(position),
            }}
        >
            <img className={classes.type} src={GetPlayerTypePicture(type)} onClick={changePlayer} />
            <Image
                className={classes.group}
                roundedCircle
                src={`/pictures/${Constants.groups[group].id}/logo.png`}
                alt={Constants.groups[group].id}
                onClick={changeGroup}
            />
            {userName && <div className={classes.userName}>{userName}</div>}
            {posInfo && <div>{posInfo}</div>}
        </div>
    );
};
export default Player;
