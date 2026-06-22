import cn from 'classnames';
import { Constants } from '/app/constants';
import Image from 'react-bootstrap/Image';
import { useState } from 'react';

interface ImageGroup {
  name?: string;
  description?: string;
  extension?: string;
  photos: Array<{
    name: string;
    description: string;
    subTypeCount?: number;
  }>;
}

interface ExpandedGroups {
  [key: string]: boolean;
}

const PirateGallery = () => {
  const imageGroups: Record<string, ImageGroup> = Constants.imageGroups;
  const [expandedGroups, setExpandedGroups] = useState<ExpandedGroups>({});

  const toggleGroup = (groupId: string) => {
    setExpandedGroups(prev => ({
      ...prev,
      [groupId]: !prev[groupId]
    }));
  };

  return (
    <div style={styles.mainContainer}>
      {Object.entries(imageGroups).map(([groupId, group]: [string, ImageGroup]) => {
        const isExpanded = expandedGroups[groupId] || false;

        return (
          <div key={groupId} style={styles.groupContainer}>
            {/* Заголовок команды с логотипом слева - кликабельно */}
            <div 
              style={styles.teamHeader} 
              onClick={() => toggleGroup(groupId)}
              className="team-header-clickable"
            >
              <Image
                className={cn('icon')}
                roundedCircle
                src={`/pictures/${groupId}/logo.png`}
                style={styles.teamLogo}
              />
              {group.name && (
                <h2 style={styles.groupTitle}>{group.name}</h2>
              )}
              <span style={styles.expandIndicator}>
                {isExpanded ? '▲' : '▼'}
              </span>
            </div>

            {/* Контент группы (описание + карточки) - сворачивается */}
            <div style={{
              maxHeight: isExpanded ? '5000px' : '0',
              overflow: 'hidden',
              transition: 'max-height 0.4s ease',
            }}>
              {/* Описание команды */}
              {group.description && (
                <p style={styles.groupDescription}>{group.description}</p>
              )}

              {/* Сетка карточек пиратов */}
              <div style={styles.photosGrid}>
                {group.photos.map((photo, index) => (
                  <div key={index} style={styles.pirateCard}>
                    <div style={styles.cardTop}>
                      <div style={styles.imageContainer}>
                        <Image
                          src={`/pictures/${groupId}/pirate_${index + 1}${photo.subTypeCount && photo.subTypeCount > 1 ? '1' : ''
                            }${group.extension || '.png'}`}
                          roundedCircle
                          className={cn('photo', {
                            'photo-active': true,
                          })}
                          style={styles.pirateImage}
                        />
                      </div>
                      <h3 style={styles.pirateName}>{photo.name}</h3>
                    </div>
                    <p style={styles.pirateDescription}>{photo.description}</p>
                  </div>
                ))}
              </div>
            </div>
          </div>
        );
      })}

      {/* Стили для курсора при наведении на заголовок */}
      <style>
        {`
          .team-header-clickable {
            cursor: pointer;
            transition: background-color 0.2s ease;
          }
          .team-header-clickable:hover {
            opacity: 0.8;
          }
        `}
      </style>
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
  teamHeader: {
    display: 'flex',
    alignItems: 'center',
    marginBottom: '0',
    userSelect: 'none' as const,
  },
  teamLogo: {
    width: '40px',
    height: '40px',
    objectFit: 'cover' as const,
    borderRadius: '50%',
    marginRight: '15px',
  },
  groupTitle: {
    fontSize: '2rem',
    color: '#8b4513',
    margin: 0,
    flex: 1,
  },
  expandIndicator: {
    fontSize: '1.2rem',
    color: '#8b4513',
    marginLeft: '10px',
  },
  groupDescription: {
    fontSize: '1.1rem',
    color: '#555',
    lineHeight: '1.6',
    marginBottom: '30px',
    fontStyle: 'italic',
    marginTop: '20px',
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

export default PirateGallery;
