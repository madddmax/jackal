import cn from 'classnames';
import { Constants } from '/app/constants';
import Image from 'react-bootstrap/Image';

interface Photo {
  name: string;
  description: string;
  subTypeCount?: number;
}

const PeopleGallery = () => {
  const gannGroups: Photo[] = Constants.gannPhotos;
  const fridayGroups: Photo[] = Constants.fridayPhotos;
  const missionerGroups: Photo[] = Constants.missionerPhotos;

  return (
      <div style={styles.mainContainer}>
      <div style={styles.groupContainer}>
          {/* Сетка карточек жителей острова */}
          <div style={styles.photosGrid}>

          {gannGroups.map((group, index) => (
              <div style={styles.pirateCard}>
              <div style={styles.cardTop}>
                  <div style={styles.imageContainer}>
                  <Image
                      src={`/pictures/commonganns/gann_${index + 1}.png`}
                      roundedCircle
                      className={cn('photo', {
                      'photo-active': true,
                      })}
                      style={styles.pirateImage}
                  />
                  </div>
                  <h3 style={styles.pirateName}>{group.name}</h3>
              </div>
              <p style={styles.pirateDescription}>{group.description}</p>
              </div>
          ))}

          {fridayGroups.map((group, index) => (
              <div style={styles.pirateCard}>
              <div style={styles.cardTop}>
                  <div style={styles.imageContainer}>
                  <Image
                      src={`/pictures/commonfridays/friday_${index + 1}.jpg`}
                      roundedCircle
                      className={cn('photo', {
                      'photo-active': true,
                      })}
                      style={styles.pirateImage}
                  />
                  </div>
                  <h3 style={styles.pirateName}>{group.name}</h3>
              </div>
              <p style={styles.pirateDescription}>{group.description}</p>
              </div>
          ))}

          {missionerGroups.map((group, index) => (
              <div style={styles.pirateCard}>
              <div style={styles.cardTop}>
                  <div style={styles.imageContainer}>
                  <Image
                      src={`/pictures/common_missioners/missioner_${index + 1}.jpg`}
                      roundedCircle
                      className={cn('photo', {
                      'photo-active': true,
                      })}
                      style={styles.pirateImage}
                  />
                  </div>
                  <h3 style={styles.pirateName}>{group.name}</h3>
              </div>
              <p style={styles.pirateDescription}>{group.description}</p>
              </div>
          ))}
          
          </div>
      </div>
      </div>
  );
};

const styles = {
  mainContainer: {
    maxWidth: '1200px',
    margin: '0 auto',
    padding: '10px',
    fontFamily: "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif",
  },
  groupContainer: {
    marginBottom: '30px',
    backgroundColor: '#f9f3e8',
    padding: '20px',
    borderRadius: '15px',
    boxShadow: '0 4px 15px rgba(0,0,0,0.1)',
  },
  photosGrid: {
    display: 'grid',
    gridTemplateColumns: 'repeat(auto-fill, minmax(280px, 1fr))',
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
    display: 'flex',
    flexDirection: 'column' as const,
  },
  cardTop: {
    display: 'flex',
    alignItems: 'center',
    marginBottom: '15px',
  },
  imageContainer: {
    flexShrink: 0,
    marginRight: '15px',
  },
  pirateImage: {
    width: '80px',
    height: '80px',
    objectFit: 'cover' as const,
    borderRadius: '50%',
  },
  pirateName: {
    fontSize: '1.3rem',
    color: '#2c3e50',
    margin: 0,
    flex: 1,
    wordBreak: 'break-word' as const,
  },
  pirateDescription: {
    fontSize: '0.95rem',
    color: '#666',
    lineHeight: '1.5',
    margin: 0,
    width: '100%',
  },
};

export default PeopleGallery;