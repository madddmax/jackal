using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Jackal.Tests2;

public class UnitTests
{
    [Fact]
    public void TestPermutations()
    {
        Assert.True(Utils.Factorial(0) == 1);
        Assert.True(Utils.Factorial(1) == 1);
        Assert.True(Utils.Factorial(4) == 4 * 3 * 2 * 1);

        Console.WriteLine("Permutations:");
        HashSet<string> hashSet=new HashSet<string>();
        for (int i = 0; i < Utils.Factorial(4); i++)
        {
            var rec = Utils.GetPermutation(i, new[] {"1", "2", "3", "4"});
            var val = string.Join(",", rec.ToArray());
            Assert.True(hashSet.Contains(val) == false);
            Console.WriteLine("{0}: {1}", i, val);
        }
        var set1 = Utils.GetPermutation(0, new[] {"1", "2", "3", "4"});
        var set2 = Utils.GetPermutation(Utils.Factorial(4), new[] {"1", "2", "3", "4"});
        Assert.True(set1.SequenceEqual(set2));
    }

    [Fact]
    public void TestArrowsHelper()
    {
        var rez = ArrowsCodesHelper.DoRotate(255);
        Assert.True(rez==255);

        for (int code = 0; code <= 255; code++)
        {
            int test = code;
            for (int i = 1; i <= 4; i++)
                test = ArrowsCodesHelper.DoRotate(test);
            Assert.True(code == test);
        }
    }
}