namespace Jackal
{
    public class TileParams : IClonable<TileParams>
    {
        public Position Position;
        public TileType Type;
        public int ArrowsCode;
		public int CanonDirection;
        public int SpinningCount;


        public TileParams Clone()
        {
            return (TileParams)this.MemberwiseClone();
        }
    }
}