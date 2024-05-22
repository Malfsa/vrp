using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class ConverCoordinateMatrix
    {
        public static double[,] ConvertCoordinatesToDistanceMatrix(double[,] coordinatesMatrix)
        {
            int numberOfCities = coordinatesMatrix.GetLength(0);
            double[,] distanceMatrix = new double[numberOfCities, numberOfCities];

            for (int i = 0; i < numberOfCities; i++)
            {
                for (int j = 0; j < numberOfCities; j++)
                {
                    if (i == j)
                    {
                        distanceMatrix[i, j] = 100000; // Расстояние от города до самого себя равно 0
                    }
                    else
                    {
                        double x1 = coordinatesMatrix[i, 0];
                        double y1 = coordinatesMatrix[i, 1];
                        double x2 = coordinatesMatrix[j, 0];
                        double y2 = coordinatesMatrix[j, 1];

                        // Вычисляем расстояние между двумя точками с помощью формулы Евклида
                        double distance = CalculateEuclideanDistance(x1, y1, x2, y2);
                        // if(i==j) distanceMatrix[i, j] =  100000;
                        // Предполагаем, что расстояние является целым числом
                        distanceMatrix[i, j] = (double)distance;
                    }
                }
            }

            return distanceMatrix;
        }

        private static double CalculateEuclideanDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }
}
