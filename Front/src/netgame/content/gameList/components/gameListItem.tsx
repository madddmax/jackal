import { ListGroup } from 'react-bootstrap';
import { BsArrowCounterclockwise } from 'react-icons/bs';

import classes from '../gamelist.module.less';
import { fromNow } from '/app/global';
import { DisplayedGameInfo } from '/common/redux.types';

interface GameListItemProps {
    key: string;
    info: DisplayedGameInfo;
    children: React.ReactElement;
}

const GameListItem = ({ key, info: { id, creatorName, timeStamp }, children }: GameListItemProps) => {
    const timeData = fromNow(timeStamp);

    return (
        <ListGroup.Item
            key={key}
            className={classes.listIconsItem}
            style={{
                display: 'flex',
                alignItems: 'center',
            }}
        >
            {id && <span>{id}</span>}
            <span>
                <BsArrowCounterclockwise size={48} color={timeData.color} style={{ verticalAlign: 'middle' }} />
                <span
                    style={{
                        fontSize: '8px',
                        marginLeft: -36,
                        color: timeData.color,
                    }}
                >
                    {timeData.value}
                    {timeData.unit}
                </span>
            </span>

            <span style={{ flexGrow: 2 }}>{creatorName}</span>
            {children}
        </ListGroup.Item>
    );
};

export default GameListItem;
