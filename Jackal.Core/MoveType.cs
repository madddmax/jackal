namespace Jackal.Core
{
    /// <summary>
    /// Тип хода
    /// </summary>
    public enum MoveType
    {
        /// <summary>
        /// Обычный
        /// </summary>
        Usual = 0,
        
        /// <summary>
        /// Перенос монеты
        /// </summary>
        WithCoin = 1,
        
        /// <summary>
        /// Воскрешение пирата на бабе
        /// </summary>
        WithRespawn = 2,
        
        /// <summary>
        /// Открытие неизвестной клетки с маяка
        /// </summary>
        WithLighthouse = 3
    };
}