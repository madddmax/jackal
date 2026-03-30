import cn from 'classnames';
import { useRef } from 'react';
import Image from 'react-bootstrap/Image';
import { PlacesType, Tooltip, TooltipRefProps } from 'react-tooltip';

import { GetPlayerTypePicture } from '../../../constants';
import { PlayerInfo } from '../../../types/players';
import classes from './players.module.less';
import { ImageGroupsIds } from '/app/constants';

const tooltipPositions: PlacesType[] = ['bottom', 'left', 'top', 'right'];
const tooltipAnchors: string[] = ['#players_mode_cntrl', '#player_pos_3', '#player_pos_0', '#player_pos_1'];

interface PlayerProps {
    id: string;
    position: number;
    type: string;
    userName?: string;
    imageGroupId: ImageGroupsIds;
    posInfo?: string;
    changePlayer: (pos: number) => void;
    changeGroup: (grpId: ImageGroupsIds) => void;
    allowedGamers: PlayerInfo[];
    isPublic: boolean;
}

const Player = ({
    id,
    position,
    type,
    userName,
    imageGroupId,
    posInfo,
    changePlayer,
    changeGroup,
    allowedGamers,
    isPublic,
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
                    {Object.values(ImageGroupsIds).map((grpId) => (
                        <Image
                            className={classes.icon}
                            roundedCircle
                            src={`/pictures/${grpId}/logo.png`}
                            onClick={() => {
                                actionsTooltip.current?.close();
                                changeGroup(grpId);
                            }}
                        />
                    ))}
                </div>
            ),
        });
    };

    const showPlayerModal = () => {
        actionsTooltip.current?.open({
            content: (
                <div
                    className={cn({
                        [classes.content]: isPublic,
                        [classes.contentSmall]: !isPublic,
                    })}
                >
                    {allowedGamers.map((it, index) =>
                        isPublic ? (
                            <div>
                                <Image
                                    className={classes.icon}
                                    roundedCircle
                                    src={GetPlayerTypePicture(it.type)}
                                    onClick={() => {
                                        actionsTooltip.current?.close();
                                        changePlayer(index);
                                    }}
                                />
                                {it.userName && <span className={classes.userModalName}>{it.userName}</span>}
                            </div>
                        ) : (
                            <Image
                                className={classes.icon}
                                roundedCircle
                                src={GetPlayerTypePicture(it.type)}
                                onClick={() => {
                                    actionsTooltip.current?.close();
                                    changePlayer(index);
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
                <img className={classes.type} src={GetPlayerTypePicture(type)} onClick={showPlayerModal} />
                <Image
                    className={classes.group}
                    roundedCircle
                    src={`/pictures/${imageGroupId}/logo.png`}
                    alt={imageGroupId}
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
                style={{ backgroundColor: 'white', zIndex: 1000, padding: 0 }}
                clickable
                openEvents={{}}
                closeEvents={{ blur: true }}
                globalCloseEvents={{ clickOutsideAnchor: true, escape: true }}
            />
        </>
    );
};
export default Player;
