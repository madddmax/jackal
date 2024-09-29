namespace Jackal.Core;

public class TileParams : IClonable<TileParams>
{
    /// <summary>
    /// Позиция
    /// </summary>
    public Position Position;
        
    /// <summary>
    /// Тип клетки
    /// </summary>
    public TileType Type;
        
    /// <summary>
    /// Код клетки, используется для стрелок и пустых клеток.
    /// По коду вычисляем номер картинки для клетки.
    /// </summary>
    public int ArrowsCode;
        
    /// <summary>
    /// Направление клетки (или количество поворотов):
    /// 0 - вверх, 1 - вправо, 2 - вниз, 3 - влево
    /// </summary>
    public int Direction;
        
    /// <summary>
    /// Количество ходов на клетке-вертушке
    /// </summary>
    public int SpinningCount;

    public TileParams()
    {
    }
        
    public TileParams(TileType type)
    {
        Type = type;
    }
        
    public TileParams(TileType type, int arrowsCode)
    {
        Type = type;
        ArrowsCode = arrowsCode;
    }
        
    public TileParams Clone()
    {
        return (TileParams)MemberwiseClone();
    }
}