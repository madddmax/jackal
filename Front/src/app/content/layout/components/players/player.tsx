import { useRef } from 'react';
import Image from 'react-bootstrap/Image';
import { PlacesType, Tooltip, TooltipRefProps } from 'react-tooltip';

import classes from './players.module.less';
import { Constants } from '/app/constants';
import { GetPlayerTypePicture } from '/common/constants';

const tooltipPositions: PlacesType[] = ['bottom', 'left', 'top', 'right'];
const tooltipAnchors: string[] = ['#players_mode_cntrl', '#player_pos_3', '#player_pos_0', '#player_pos_1'];

interface PlayerProps {
    id: string;
    position: number;
    type: string;
    userName?: string;
    group: number;
    posInfo?: string;
    changePlayer: () => void;
    changeGroup: (pos: number) => void;
    isIgnoredGroup: (pos: number) => boolean;
}

const Player = ({
    id,
    position,
    type,
    userName,
    group,
    posInfo,
    changePlayer,
    changeGroup,
    isIgnoredGroup,
}: PlayerProps) => {
    const actionsTooltip = useRef<TooltipRefProps>(null);

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

    const showGroupModal = () => {
        actionsTooltip.current?.open({
            content: (
                <div className={classes.content}>
                    {Constants.groups.map((it, index) =>
                        isIgnoredGroup(index) ? (
                            <></>
                        ) : (
                            <Image
                                className={classes.icon}
                                roundedCircle
                                src={`/pictures/${it.id}/logo.png`}
                                onClick={() => {
                                    actionsTooltip.current?.close();
                                    changeGroup(index);
                                }}
                            />
                        ),
                    )}
                </div>
            ),
        });
    };

    return (
        <>
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
                    onClick={showGroupModal}
                />
                {userName && <div className={classes.userName}>{userName}</div>}
                {posInfo && <div>{posInfo}</div>}
            </div>
            <Tooltip
                ref={actionsTooltip}
                anchorSelect={tooltipAnchors[position]}
                place={tooltipPositions[position]}
                className={classes.groupsTooltip}
                classNameArrow={classes.groupsTooltipArrow}
                style={{ backgroundColor: 'white', zIndex: 1000 }}
                clickable
                openEvents={{}}
                closeEvents={{ blur: true }}
                globalCloseEvents={{ clickOutsideAnchor: true, escape: true }}
            />
        </>
    );
};
export default Player;
