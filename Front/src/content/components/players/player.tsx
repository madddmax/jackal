import { Constants } from '/app/constants';
import classes from './players.module.less';
import Image from 'react-bootstrap/Image';

interface PlayerProps {
    position: number;
    type: string;
    group: number;
    posInfo?: string;
    changePlayer: () => void;
    changeGroup: () => void;
}

const Player = ({ position, type, group, posInfo, changePlayer, changeGroup }: PlayerProps) => {
    const getUrlByPlayer = (type: string) => {
        if (type === 'human') return '/pictures/human.png';
        else if (type === 'robot') return '/pictures/robot.png';
        return '/pictures/robot2.png';
    };

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
            className={classes.player}
            style={{
                top: getTopPosition(position),
                left: getLeftPosition(position),
            }}
        >
            <img className={classes.type} src={getUrlByPlayer(type)} onClick={changePlayer} />
            <Image
                className={classes.group}
                roundedCircle
                src={`/pictures/${Constants.groups[group].id}/logo.png`}
                alt={Constants.groups[group].id}
                onClick={changeGroup}
            />
            {posInfo && <div>{posInfo}</div>}
        </div>
    );
};
export default Player;
