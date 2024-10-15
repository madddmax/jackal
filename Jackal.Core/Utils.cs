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
        int N = array.Length;
        if (N == 1)
            return array;
        int permutationsCount = Factorial(N);
        index %= permutationsCount;
        var t = array[index/(permutationsCount/N)];
        return new T[] {t}.Concat(GetPermutation<T>(index%(permutationsCount/N), array.Where(x => !x.Equals(t)).ToArray()));
    }

    public static T Min<T>(this IEnumerable<T> source, Comparison<T> comparison)
    {
        if (source == null) throw new ArgumentNullException("source");
        if (comparison == null) throw new ArgumentNullException("comparison");

        bool hasValue = false;
        T value = default(T);
        foreach (T x in source)
        {
            if (hasValue)
            {
                if (comparison(x, value) < 0)
                    value = x;
            }
            else
            {
                value = x;
                hasValue = true;
            }
        }
        if (hasValue) return value;
        throw new ArgumentException("source");
    }

    public static IEnumerable<T> WhereEqualMin<T>(this IEnumerable<T> source, Comparison<T> comparison)
    {
        var enumerable = source as IList<T> ?? source.ToList();
        var min = enumerable.Min(comparison);
        return enumerable.Where(x => comparison(x, min) == 0);
    }
        
    public static int Distance(Position pos1, Position pos2)
    {
        int deltaX = Math.Abs(pos1.X - pos2.X);
        int deltaY = Math.Abs(pos1.Y - pos2.Y);
        int totalDelta = Math.Max(deltaX, deltaY);
        return totalDelta;
    }
}