using System;
using System.Threading;
using System.Threading.Tasks;

namespace WarSteel.Utils;

public static class Timer
{


    // runs a callback once after x ms have passed
    public static async void Timeout(int timeInMs, Action cb)
    {
        await Task.Delay(timeInMs);
        cb();
    }
}