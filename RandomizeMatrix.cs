using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class RandomizeMatrix
    {
       public int[,] RandomizeMatrix1(int[,] matrix)
        {
            Random random = new Random();
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = random.Next(100); // Генерация случайного числа от 0 до 99
                }
            }
            return matrix;
        }
    }
}
