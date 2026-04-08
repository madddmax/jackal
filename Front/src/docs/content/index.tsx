import { Col, Container, Row, Tab, Tabs } from 'react-bootstrap';

import classes from './gamedocuments.module.less';
import DocRules from '/docs/content/docRules/docRules';
import MapRenderer from '/docs/content/mapRenderer/mapRenderer';

const GameDocuments = () => {
    return (
        <Container>
            <Row className="justify-content-center">
                <Col lg className="g-lg-2">
                    <div className={classes.gamedocuments}>
                        <Tabs defaultActiveKey="rules" id="learning-tab" className="mb-3">
                            <Tab eventKey="rules" title="Правила" style={{ overflowX: 'auto' }}>
                                <DocRules />
                            </Tab>
                            <Tab eventKey="mapRenderer" title="Отрисовщик карты">
                                <MapRenderer />
                            </Tab>
                        </Tabs>
                    </div>
                </Col>
            </Row>
        </Container>
    );
};

export default GameDocuments;
