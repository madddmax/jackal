using System;
using System.Diagnostics;

namespace Jackal.BotArena;

public static class StopwatchMeter
{
    public static TimeSpan GetElapsed(Action act) {
        var sw = new Stopwatch();
        sw.Start();
        act();
        sw.Stop();
        return sw.Elapsed;
    }
}