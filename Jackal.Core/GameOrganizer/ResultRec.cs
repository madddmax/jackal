namespace Jackal.GameOrganizer
{

    public class ResultRec
    {
        public double Position1;
        public double Position2;
        public double Position3;
        public double Position4;

        public double AveragePosition
        {
            get
            {
                var total = Total;
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (total <1) return 0;
                return (Position1 * 1 + Position2 * 2 + Position3 * 3 + Position4 * 4) / total;
            }
        }

        public double Total
        {
            get
            {
                return Position1 + Position2 + Position3 + Position4;
            }
        }

        public double TotalGold;
    }
}
