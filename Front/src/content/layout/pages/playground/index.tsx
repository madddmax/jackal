import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';

import Pirates from './components/pirates/';
import Map from './components/map/';
import Controls from './components/controls/';
import classes from './playground.module.less';
import { useSelector } from 'react-redux';
import { ReduxState } from '/redux/types';

function Playground() {
    const gamename = useSelector<ReduxState, string | undefined>(
        (state) => state.game.gameName,
    );

    return (
        <Row className="justify-content-center gap-1">
            {gamename && (
                <>
                    <Col
                        xs={{ span: 12, order: 'last' }}
                        lg={2}
                        xl={1}
                        className={classes.pirates}
                    >
                        <Pirates />
                    </Col>
                    <Col xs={12} lg={{ span: 9, order: 'last' }} xl={7}>
                        <Map />
                    </Col>
                    <Col
                        xs={{ span: 12, order: 'first' }}
                        xl={{ span: 3, order: 'last' }}
                    >
                        <Controls />
                    </Col>
                </>
            )}
        </Row>
    );
}

export default Playground;
