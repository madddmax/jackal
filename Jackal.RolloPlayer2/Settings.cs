namespace Jackal.RolloPlayer2;

public class Settings
{
	public static Settings Default 
	{
		get
		{
			return new Settings();
		}
	}

	/// <summary>
	/// Занести золото на корабль
	/// </summary>
	public int AboardToShipWithGold = 1900;
	/// <summary>
	/// Двигаемся к закрытому
	/// </summary>
	public int MoveToUnknown = 700;
	/// <summary>
	/// Двигаться с золотом в направлении корабля
	/// </summary>
	public int MoveToShipWithGold = 1800;
	/// <summary>
	/// Двигаемся к золоту
	/// </summary>
	public int MoveToGold = 1800;
	/// <summary>
	/// Атака врага
	/// </summary>
	public int Atack = 2000;
	/// <summary>
	/// Атака нескольких врагов (за каждого)
	/// </summary>
	public int AtackManyEnemies = 900;
	/// <summary>
	/// Подставиться под удар
	/// </summary>
	public int MoveUnderAtack = 0;
	/// <summary>
	/// Уход от атаки
	/// </summary>
	public double MoveFromAtackToAll = 1.2;

	/// <summary>
	/// Уход от атаки
	/// </summary>
	public int MoveFromAtack = 1350;

	/// <summary>
	/// Двигаемся к занятому золоту
	/// </summary>
	public int MoveToOccupiedGold = 300;
	/// <summary>
	/// Удаление от корабля
	/// </summary>
	public int MoveFromShip = -500;


}