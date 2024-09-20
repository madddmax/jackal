using System;
using System.Collections.Generic;
using System.Linq;

namespace Jackal.Core
{
    public static class ArrowsCodesHelper
    {
        /// <summary>
        /// Одна стрелка перпендикулярно вверх
        /// </summary>
        public static int OneArrowUp => GetCodeFromString("10000000");

        /// <summary>
        /// Одна стрелка по диагонали правый верхний угол
        /// </summary>
        public static int OneArrowDiagonal => GetCodeFromString("01000000");
        
        /// <summary>
        /// Две стрелки по диагонали правый верхний - левый нижний угол
        /// </summary>
        public static int TwoArrowsDiagonal => GetCodeFromString("01000100");
        
        /// <summary>
        /// Две стрелки горизонтально на левую и правую стороны
        /// </summary>
        public static int TwoArrowsLeftRight => GetCodeFromString("00100010");

        /// <summary>
        /// Три стрелки одна по диагонали левый верхний угол, две перпендикулярно право и низ
        /// </summary>
        public static int ThreeArrows => GetCodeFromString("00101001");
        
        /// <summary>
        /// Четыре стрелки перпендикулярно на все стороны
        /// </summary>
        public static int FourArrowsPerpendicular => GetCodeFromString("10101010");
        
        /// <summary>
        /// Четыре стрелки по диагонали на все углы
        /// </summary>
        public static int FourArrowsDiagonal => GetCodeFromString("01010101");

        /// <summary>
        /// Последовательность типов стрелок не менять,
        /// используется для выбора номера картинки
        /// </summary>
        private static readonly int[] ArrowsTypes =
        [
            OneArrowUp,
            TwoArrowsDiagonal,
            OneArrowDiagonal,
            FourArrowsDiagonal,
            FourArrowsPerpendicular,
            ThreeArrows,
            TwoArrowsLeftRight
        ];

        public class ArrowSearchResult
        {
            public int ArrowType;
            public int RotateCount;
        }

        public static ArrowSearchResult Search(int code)
        {
            for (int arrowType = 0; arrowType < ArrowsTypes.Length; arrowType++)
            {
                int arrowsCode = ArrowsTypes[arrowType];
                for (int rotateCount = 0; rotateCount <= 3; rotateCount++)
                {
                    if (arrowsCode == code) return new ArrowSearchResult {ArrowType = arrowType, RotateCount = rotateCount};
                    arrowsCode = DoRotate(arrowsCode);
                }
            }
            throw new Exception("Unknown arrow type");
        }

        public static int GetCodeFromString(string str)
        {
            if (str.Length != 8) throw new ArgumentException("str");
            str = new string(str.Reverse().ToArray());
            return Convert.ToInt32(str, 2);
        }

        /// <summary>
        /// Поворот по часовой стрелке
        /// </summary>
        public static int DoRotate(int code)
        {
            code &= 255;
            //биты 6 и 7
            int bits67 = code >> 6;
            int bits05 = code & 63;
            int newBits = (bits05 << 2) | bits67;
            return newBits;
        }

        public static IEnumerable<Position> GetExitDeltas(int code)
        {
            code &= 255;
            if ((code & 1) != 0)
                yield return new Position(0, 1);
            if ((code & 2) != 0)
                yield return new Position(1, 1);
            if ((code & 4) != 0)
                yield return new Position(1, 0);
            if ((code & 8) != 0)
                yield return new Position(1, -1);
            if ((code & 16) != 0)
                yield return new Position(0, -1);
            if ((code & 32) != 0)
                yield return new Position(-1, -1);
            if ((code & 64) != 0)
                yield return new Position(-1, 0);
            if ((code & 128) != 0)
                yield return new Position(-1, 1);
        }
    }
}