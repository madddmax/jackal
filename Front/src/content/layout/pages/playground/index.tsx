import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';

import Pirates from './components/pirates/';
import Map from './components/map/';
import Controls from './components/controls/';
import classes from './playground.module.less';

function Playground() {
    return (
        <Container>
            <Row className="justify-content-center gap-1">
                <Col
                    xs={{ span: 12, order: 'last' }}
                    md={2}
                    lg={1}
                    className={classes.pirates}
                >
                    <Pirates />
                </Col>
                <Col xs={12} md={{ span: 9, order: 'last' }} lg={7}>
                    <Map />
                </Col>
                <Col
                    xs={{ span: 12, order: 'first' }}
                    lg={{ span: 3, order: 'last' }}
                >
                    <Controls />
                </Col>
            </Row>
        </Container>
    );
}

export default Playground;
