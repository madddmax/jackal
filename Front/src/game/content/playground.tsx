import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';
import { useSelector } from 'react-redux';

import Controls from './components/controls';
import Map from './components/map';
import Pirates from './components/pirates';
import classes from './playground.module.less';
import { getGameSettings } from '/game/redux/gameSlice';

function Playground() {
    const { cellSize, mapSize } = useSelector(getGameSettings);

    return (
        <Row className="justify-content-center gap-1">
            {mapSize && cellSize && (
                <>
                    <Col xs={{ span: 12, order: 'last' }} lg={2} xl={1} className={classes.pirates}>
                        <Pirates />
                    </Col>
                    <Col xs={12} lg={{ span: 9, order: 'last' }} xl={7}>
                        <Map mapSize={mapSize} cellSize={cellSize} />
                    </Col>
                    <Col xs={{ span: 12, order: 'first' }} xl={{ span: 3, order: 'last' }}>
                        <Controls />
                    </Col>
                </>
            )}
        </Row>
    );
}

export default Playground;
