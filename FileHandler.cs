using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class FileHandler
    {
        public static void SaveToFile(string filePath, string selectedMatrixType, int numberOfCities, int numberOfTransports, double numberOfPercent, ObservableCollection<ObservableCollection<double>> matrixFields, ObservableCollection<int> transportFields)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Сохранение типа матрицы
                writer.WriteLine($"SelectedMatrixType,{selectedMatrixType}");

                // Сохранение количества городов и транспорта
                writer.WriteLine($"NumberOfCities,{numberOfCities}");
                writer.WriteLine($"NumberOfTransports,{numberOfTransports}");
                writer.WriteLine($"NumberOfPercent,{numberOfPercent}");

                // Сохранение матрицы
                writer.WriteLine("Matrix:");
                foreach (var row in matrixFields)
                {
                    writer.WriteLine(string.Join(",", row));
                }

                // Сохранение транспортных полей
                writer.WriteLine("TransportFields:");
                writer.WriteLine(string.Join(",", transportFields));
            }
        }

        public static void LoadFromFile(string filePath, out string selectedMatrixType, out int numberOfCities, out int numberOfTransports, out double numberOfPercent, out ObservableCollection<ObservableCollection<double>> matrixFields, out ObservableCollection<int> transportFields)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                selectedMatrixType = reader.ReadLine().Split(',')[1];
                numberOfCities = int.Parse(reader.ReadLine().Split(',')[1]);
                numberOfTransports = int.Parse(reader.ReadLine().Split(',')[1]);
                numberOfPercent = double.Parse(reader.ReadLine().Split(',')[1]);

                // Чтение матрицы
                matrixFields = new ObservableCollection<ObservableCollection<double>>();
                reader.ReadLine(); // Пропустить строку "Matrix:"
                for (int i = 0; i < numberOfCities; i++)
                {
                    var row = new ObservableCollection<double>(reader.ReadLine().Split(',').Select(double.Parse));
                    matrixFields.Add(row);
                }

                // Чтение транспортных полей
                transportFields = new ObservableCollection<int>();
                reader.ReadLine(); // Пропустить строку "TransportFields:"
                var transportData = reader.ReadLine().Split(',').Select(int.Parse);
                foreach (var item in transportData)
                {
                    transportFields.Add(item);
                }
            }
        }
    }
}
