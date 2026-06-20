import cn from 'classnames';
import { Constants } from '/app/constants';
import Image from 'react-bootstrap/Image';

const PirateGallery = () => {

  const imageGroups = Constants.imageGroups;

  return (
    <div style={styles.mainContainer}>
      {Object.entries(imageGroups).map(([groupId, group]) => (
        <div key={groupId} style={styles.groupContainer}>

          {/* Название команды */}
          {group.name && (
            <h2 style={styles.groupTitle}>{group.name}</h2>
          )}

          {/* Описание команды */}
          {group.description && (
            <p style={styles.groupDescription}>{group.description}</p>
          )}

          {/* Сетка карточек пиратов */}
          <div style={styles.photosGrid}>
            {group.photos.map((photo, index) => (
              <div key={index} style={styles.pirateCard}>
                <h3 style={styles.pirateName}>{photo.name}</h3>
                <p style={styles.pirateDescription}>{photo.description}</p>
                <div style={styles.pirateStats}>
                  <Image
                    src={`/pictures/${groupId}/pirate_${index + 1}${photo.subTypeCount > 1 ? '1' : ''}${group.extension || '.png'}`}
                    roundedCircle
                    className={cn('photo', {
                      'photo-active': true,
                    })}
                  />
                </div>
              </div>
            ))}
          </div>
        </div>
      ))}
    </div>
  );
};

// Стили (вынести в отдельный CSS файл)
const styles = {
  mainContainer: {
    maxWidth: '1200px',
    margin: '0 auto',
    padding: '20px',
    fontFamily: "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif",
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
};

export default PirateGallery;