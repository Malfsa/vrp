using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class CalculationModel
    {
        public int NumberOfCities { get; set; }
        public int NumberOfTransports { get; set; }
        public double ReplacementPercentage { get; set; }
        public string MatrixType { get; set; }
        public double[,] DistanceMatrix { get; set; }
        public double[,] CoordinateMatrix { get; set; }
    }
}
