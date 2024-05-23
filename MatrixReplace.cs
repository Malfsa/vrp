using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public static class MatrixReplace
    {
        public static double[,] ReplaceTopValuesWith(double[,] matrix, int replacementValue, double per)//меняем на большие числа 
        {
           
            int rowCount = matrix.GetLength(0);
            int colCount = matrix.GetLength(1);
            int totalValuesPerRow = colCount - 1; // не считая диагональные элементы
            for (int i = 0; i < rowCount; i++)
            {
                // Создаем массив значений текущей строки (исключая диагональные элементы)
                double[] rowValues = new double[totalValuesPerRow];
                int index = 0;
                for (int j = 0; j < colCount; j++)
                {
                    if (j != i) // Исключаем диагональные элементы
                    {
                        rowValues[index++] = matrix[i, j];
                    }

                }
                // Сортируем значения по убыванию
                Array.Sort(rowValues);
                Array.Reverse(rowValues);

                // Находим индекс элемента, соответствующего 20% от общего числа значений
                int count = (int)Math.Round(totalValuesPerRow * per/100.0);
                //  MessageBox.Show("Индекс соотв 20 "+count.ToString());
                double thresholdValue;
                if (count != 0) thresholdValue = rowValues[count - 1];
                else thresholdValue = 0;
                // Заменяем самые большие значения на replacementValue
                for (int j = 0; j < colCount; j++)
                {
                    if (j != i && matrix[i, j] >= thresholdValue)
                    {
                        matrix[i, j] = replacementValue;
                    }
                    else if (i == j)
                    {
                        matrix[i, j] = 100000;
                    }
                }
            }
            return matrix;
        }
    }
}
