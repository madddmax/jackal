namespace Jackal.Core.Domain;

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
    /// Код клетки:
    /// используется для задания подтипа клетки для стрелок и пустых клеток,
    /// задаёт количество для клеток с монетами и бутылками
    /// </summary>
    public int Code;
        
    /// <summary>
    /// Направление клетки (или количество поворотов)
    /// </summary>
    public DirectionType Direction;
        
    /// <summary>
    /// Количество ходов на задерживающей клетке
    /// </summary>
    public int SpinningCount;

    public TileParams()
    {
    }
        
    public TileParams(TileType type)
    {
        Type = type;
    }
        
    public TileParams(TileType type, int code)
    {
        Type = type;
        Code = code;
    }
    
    public TileParams Clone()
    {
        return (TileParams)MemberwiseClone();
    }
}