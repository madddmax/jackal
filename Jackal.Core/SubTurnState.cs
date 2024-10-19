namespace Jackal.Core;

/// <summary>
/// Состояние дополнительного хода
/// </summary>
public class SubTurnState
{
    /// <summary>
    /// Полет на самолете
    /// </summary>
    public bool AirplaneFlying { get; set; }
        
    /// <summary>
    /// Количество просмотров карты с маяка
    /// </summary>
    public int LighthouseViewCount { get; set; }
    
    /// <summary>
    /// Падение в дыру
    /// </summary>
    public bool FallingInTheHole { get; set; }

    /// <summary>
    /// Фаза разлома:
    /// 2 - выбираем первую клетку для обмена,
    /// 1 - выбираем вторую клетку
    /// 0 - конец
    /// </summary>
    public int QuakePhase { get; set; }
    
    public void Clear()
    {
        AirplaneFlying = false;
        LighthouseViewCount = 0;
        FallingInTheHole = false;
        QuakePhase = 0;
    }
}