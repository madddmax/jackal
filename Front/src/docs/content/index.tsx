import { useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Col, Container, Row, Tab, Tabs } from 'react-bootstrap';

import classes from './gamedocuments.module.less';
import DocRules from '/docs/content/docRules/docRules';
import MapRenderer from '/docs/content/mapRenderer/mapRenderer';
import PirateGallery from '/docs/content/pirateGallery/pirateGallery';
import PeopleGallery from '/docs/content/peopleGallery/peopleGallery';

// Определяем тип для ключей вкладок
type TabKey = 'rules' | 'pirateGallery' | 'peopleGallery' | 'mapRenderer';

// Определяем тип для конфигурации вкладки
interface TabConfig {
    title: string;
    component: React.ComponentType;
}

// Типизируем конфигурацию
const TABS_CONFIG: Record<TabKey, TabConfig> = {
    rules: {
        title: 'Правила',
        component: DocRules
    },
    pirateGallery: {
        title: 'Команды пиратов',
        component: PirateGallery
    },
    peopleGallery: {
        title: 'Жители острова',
        component: PeopleGallery
    },
    mapRenderer: {
        title: 'Отрисовщик карты',
        component: MapRenderer
    }
};

// Функция для валидации ключа вкладки
const isValidTabKey = (key: string | undefined): key is TabKey => {
    return key !== undefined && key in TABS_CONFIG;
};

const GameDocuments: React.FC = () => {
    const { tabId } = useParams<{ tabId: string }>();
    const navigate = useNavigate();

    // Получаем текущую активную вкладку из URL или устанавливаем по умолчанию
    const activeKey: TabKey = isValidTabKey(tabId) ? tabId : 'rules';

    // Обновляем URL при первом рендере, если вкладка невалидна или отсутствует
    useEffect(() => {
        if (!isValidTabKey(tabId)) {
            navigate('/docs/rules', { replace: true });
        }
    }, [tabId, navigate]);

    const handleTabSelect = (key: string | null) => {
        if (key && isValidTabKey(key)) {
            navigate(`/docs/${key}`);
        }
    };

    return (
        <Container>
            <Row className="justify-content-center">
                <Col lg className="g-lg-2">
                    <div className={classes.gamedocuments}>
                        <Tabs 
                            activeKey={activeKey}
                            onSelect={handleTabSelect}
                            id="learning-tab" 
                            className="mb-3"
                        >
                            {(Object.entries(TABS_CONFIG) as [TabKey, TabConfig][]).map(([key, config]) => (
                                <Tab 
                                    key={key}
                                    eventKey={key} 
                                    title={config.title}
                                    style={{ overflowX: 'auto' }}
                                >
                                    <config.component />
                                </Tab>
                            ))}
                        </Tabs>
                    </div>
                </Col>
            </Row>
        </Container>
    );
};

export default GameDocuments;