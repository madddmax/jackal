import { Constants } from '/app/constants';

const PirateGallery = () => {

  const imageGroups = Constants.imageGroups;

  return (
    <div style={styles.mainContainer}>
      <h1 style={styles.mainTitle}>🏴‍☠️ Пиратская галерея</h1>

      {Object.entries(imageGroups).map(([groupId, group]) => (
        <div key={groupId} style={styles.groupContainer}>

          {/* Проверяем, есть ли имя у группы */}
          {group.name && (
            <h2 style={styles.groupTitle}>{group.name}</h2>
          )}

          {/* Проверяем, есть ли описание */}
          {group.description && (
            <p style={styles.groupDescription}>{group.description}</p>
          )}

          {/* Сетка карточек пиратов */}
          <div style={styles.photosGrid}>
            {group.photos.map((photo, index) => (
              // Фильтруем пустые записи
              photo.name ? (
                <div key={index} style={styles.pirateCard}>
                  <h3 style={styles.pirateName}>{photo.name}</h3>
                  <p style={styles.pirateDescription}>{photo.description}</p>
                  <div style={styles.pirateStats}>
                    <span style={styles.statBadge}>
                      👥 {photo.subTypeCount} {photo.subTypeCount === 1 ? 'версия' : photo.subTypeCount < 5 ? 'версии' : 'версий'}
                    </span>
                  </div>
                </div>
              ) : null
            ))}
          </div>
        </div>
      ))}
    </div>
  );
};

// Стили (можно вынести в отдельный CSS файл)
const styles = {
  mainContainer: {
    maxWidth: '1200px',
    margin: '0 auto',
    padding: '20px',
    fontFamily: "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif",
  },
  mainTitle: {
    fontSize: '2.5rem',
    color: '#2c3e50',
    marginBottom: '40px',
    borderBottom: '3px solid #8b4513',
    paddingBottom: '20px',
  },
  groupContainer: {
    marginBottom: '50px',
    backgroundColor: '#f9f3e8',
    padding: '30px',
    borderRadius: '15px',
    boxShadow: '0 4px 15px rgba(0,0,0,0.1)',
  },
  groupTitle: {
    fontSize: '2rem',
    color: '#8b4513',
    marginBottom: '15px',
    paddingBottom: '10px',
    borderBottom: '2px solid #d4a574',
  },
  groupDescription: {
    fontSize: '1.1rem',
    color: '#555',
    lineHeight: '1.6',
    marginBottom: '30px',
    fontStyle: 'italic',
  },
  photosGrid: {
    display: 'grid',
    gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))',
    gap: '25px',
    marginTop: '20px',
  },
  pirateCard: {
    backgroundColor: 'white',
    border: '1px solid #ddd',
    borderRadius: '10px',
    padding: '20px',
    boxShadow: '0 2px 8px rgba(0,0,0,0.1)',
    transition: 'transform 0.3s ease, box-shadow 0.3s ease',
    cursor: 'pointer',
  },
  pirateName: {
    fontSize: '1.3rem',
    color: '#2c3e50',
    marginBottom: '12px',
    borderBottom: '2px solid #ecf0f1',
    paddingBottom: '8px',
  },
  pirateDescription: {
    fontSize: '0.95rem',
    color: '#666',
    lineHeight: '1.5',
    marginBottom: '15px',
  },
  pirateStats: {
    display: 'flex',
    justifyContent: 'flex-end',
  },
  statBadge: {
    backgroundColor: '#8b4513',
    color: 'white',
    padding: '5px 12px',
    borderRadius: '20px',
    fontSize: '0.9rem',
    fontWeight: 'bold',
  },
  emptyMessage: {
    textAlign: 'center',
    color: '#999',
    fontStyle: 'italic',
    padding: '40px',
    fontSize: '1.2rem',
  },
  errorMessage: {
    textAlign: 'center',
    color: '#e74c3c',
    fontSize: '1.2rem',
    padding: '40px',
    backgroundColor: '#fadbd8',
    borderRadius: '10px',
  },
};

export default PirateGallery;