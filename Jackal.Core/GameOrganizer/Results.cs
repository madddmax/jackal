using System;
using System.Collections.Generic;
using System.Linq;

namespace Jackal.Core.GameOrganizer;

public class Results
{
    Dictionary<string, ResultRec> table = new Dictionary<string, ResultRec>();
    public volatile int MapId;
    public volatile int PermutationId;
    public volatile int GamesCount;

    public string GetState()
    {
        String rez = "";
        lock (table)
        {
            if (!table.Keys.Any())
                return "no data";

            int maxNameLen = table.Keys.Max(x => x.Length);
            foreach (var pair in table.OrderByDescending(x => x.Value.Total<1?0: x.Value.Position1/x.Value.Total))
            {
                double p1 = 0, p2 = 0, p3 = 0, p4 = 0;
                double total = pair.Value.Total;
                if (total >= 1)
                {
                    p1 = pair.Value.Position1/total;
                    p2 = pair.Value.Position2/total;
                    p3 = pair.Value.Position3/total;
                    p4 = pair.Value.Position4/total;
                }

                rez += string.Format("{0} средняя позиция {1:N3}, вероятности {2:N3} {3:N3} {4:N3} {5:N3}, золота {6:N1}\r\n",
                    pair.Key.PadRight(maxNameLen),
                    pair.Value.AveragePosition,
                    p1,
                    p2,
                    p3,
                    p4,
                    pair.Value.TotalGold/total);
            }
        }
        return rez;
    }

    public void AddGameResult(string clientId, double[] positionsCount,double goldCount)
    {
        if (positionsCount == null || positionsCount.Length != 4)
            throw new ArgumentException();
        lock (table)
        {
            ResultRec rec;
            if (table.TryGetValue(clientId, out rec) == false)
            {
                rec = new ResultRec();
                table[clientId] = rec;
            }
            rec.Position1 += positionsCount[0];
            rec.Position2 += positionsCount[1];
            rec.Position3 += positionsCount[2];
            rec.Position4 += positionsCount[3];
            rec.TotalGold += goldCount;
        }
    }
}