import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Map from './components/map';

function Layout() {

    return (
      <Container>
        <Row>
          <Col><Map /></Col>
        </Row>
      </Container>
    );
  }
  
  export default Layout;