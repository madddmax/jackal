using System;
using System.Collections.Generic;
using System.Linq;
using Jackal.Core.Domain;

namespace Jackal.Core;

public static class Utils
{
    public static int Factorial(int n)
    {
        if (n < 0)
            throw new ArgumentException("n");
        switch (n)
        {
            case 0:
            case 1:
                return 1;
            case 2:
                return 2;
            case 3:
                return 3*2;
            case 4:
                return 4*3*2;
            default:
            {
                int rez = 4*3*2;
                for (int i = 5; i <= n; i++)
                {
                    checked
                    {
                        rez *= i;
                    }
                }
                return rez;
            }
        }
    }

    /// <summary>
    /// Попадание в углы участка 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static bool InCorners(Position value, int min, int max)
    {
        return (value.X == min || value.X == max) && (value.Y == min || value.Y == max);
    }

    public static IEnumerable<T> GetPermutation<T>(int index, T[] array) where T : class
    {
        int length = array.Length;
        if (length == 1)
            return array;

        int permutationsCount = Factorial(length);
        index %= permutationsCount;
        var t = array[index / (permutationsCount / length)];
        return new T[] { t }.Concat(GetPermutation<T>(
            index % (permutationsCount / length),
            array.Where(x => !x.Equals(t)).ToArray())
        );
    }
}