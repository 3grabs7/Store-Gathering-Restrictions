using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp
{
    public enum Pressure
    {
        Mild, Average, Crowded
    }
    public class Testing
    {
        private IEnumerable<Section> Sections { get; set; }
        private static Dictionary<Pressure, int> EntersPerSecond = new Dictionary<Pressure, int>()
        {
            [Pressure.Mild] = 1,
            [Pressure.Average] = 5,
            [Pressure.Crowded] = 10,
        };

        public Testing(Store store)
        {
            Sections = store.Sections;
        }

        public async Task Start(int duration, Pressure pressure)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            while (stopWatch.Elapsed.TotalSeconds < duration)
            {
                // add people to random sections
                // keep another thread with exits
                await Task.Delay(1000);
            }
        }
    }
}
