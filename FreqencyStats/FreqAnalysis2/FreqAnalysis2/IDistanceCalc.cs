using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreqAnalysis2
{
    interface IDistanceCalc
    {
        double GetDistance(string a, string b);
        double GetDistance(double[] a, double[] b);
        double[] GetVector(string p);
    }
}
