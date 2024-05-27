using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class FileHandler
    {
        public static void SaveToFile(string filePath, string selectedMatrixType, int numberOfCities, int numberOfTransports, double numberOfPercent, ObservableCollection<ObservableCollection<MatrixElement>> matrixFields, ObservableCollection<ObservableInt> transportFields)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Сохранение типа матрицы
                //   writer.WriteLine($"SelectedMatrixType,{selectedMatrixType}");

                writer.WriteLine($"Тип матрицы: {selectedMatrixType}");
                writer.WriteLine($"Количество городов: {numberOfCities}");
                writer.WriteLine($"Количество транспортных средств: {numberOfTransports}");
                writer.WriteLine($"Процент замены чисел в разреженной матрице: {numberOfPercent}");
                // Сохранение транспортных полей
                writer.WriteLine("Минимальное количество посещенных городов для каждого транспортного средства:");
                foreach (var field in transportFields)
                {
                    writer.WriteLine(field.Value);
                }
                // Сохранение матрицы
                writer.WriteLine("Матрица:");
                foreach (var row in matrixFields)
                {
                    writer.WriteLine(string.Join(" ", row.Select(m => m.Value)));
                }


            }
        }

       // private const string DataFilePath = "combinedData.txt";

        public static void SaveData(string fileName, string selectedMatrixType, int numberOfCities, int numberOfTransports, int numberOfPercent, ObservableCollection<ObservableCollection<MatrixElement>> matrixFields, ObservableCollection<ObservableInt> transportFields, ResultsViewModel resultsViewModel,MainViewModel mainViewModel)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                // Save input data
               // writer.WriteLine($"Тип матрицы: {selectedMatrixType}");
                writer.WriteLine($"Количество городов: {numberOfCities}");
                writer.WriteLine($"Количество транспортных средств: {numberOfTransports}");
                writer.WriteLine($"Процент замены чисел в разреженной матрице: {numberOfPercent}");
                // Сохранение транспортных полей
                writer.WriteLine("Минимальное количество посещенных городов для каждого транспортного средства:");
                foreach (var field in transportFields)
                {
                    writer.WriteLine(field.Value);
                }
                // Сохранение матрицы
                writer.WriteLine("Матрица:");
                foreach (var row in matrixFields)
                {
                    writer.WriteLine(string.Join(" ", row.Select(m => m.Value)));
                }

                // Save results
                writer.WriteLine("\nРезультаты:");
                if ((mainViewModel.IsBaseMatrix) && (mainViewModel.IsReplaceMatrix))
                {
                    writer.WriteLine($"Точность: {resultsViewModel.Accuracy}");
                }
                if (mainViewModel.IsBaseMatrix) 
                {
                   writer.WriteLine("ДЛЯ БАЗОВОЙ МАТРИЦЫ");
                   writer.WriteLine($"Оптимальная длина маршрута: {resultsViewModel.OptimalValue}");
                    writer.WriteLine($"Время: {resultsViewModel.Times}");
                    //   writer.WriteLine("RouteMatrix:");
                    /* foreach (var route in resultsViewModel.RouteMatrix)
                     {
                         writer.WriteLine(route);
                     }*/
                    writer.WriteLine("Оптимальные маршруты:");
                    foreach (var route in resultsViewModel.Routes)
                    {
                        writer.WriteLine(route);
                    }
                    writer.WriteLine("Оптимальная длина маршрута для каждого транспорта:");
                    foreach (var distance in resultsViewModel.Distances)
                    {
                        writer.WriteLine(distance);
                    }
                }

                if (mainViewModel.IsReplaceMatrix)
                {
                    writer.WriteLine("ДЛЯ РАЗРЕЖЕННОЙ МАТРИЦЫ");
                    writer.WriteLine($"Оптимальная длина маршрута: {resultsViewModel.OptimalValue2}");
                    writer.WriteLine($"Время расчета разреженной матрицы: {resultsViewModel.Times2}");
                    // writer.WriteLine("RouteMatrix2:");
                    /* foreach (var route in resultsViewModel.RouteMatrix2)
                     {
                         writer.WriteLine(route);
                     }*/
                    writer.WriteLine("Оптимальные маршруты:");
                    foreach (var route in resultsViewModel.Routes2)
                    {
                        writer.WriteLine(route);
                    }
                    writer.WriteLine("Оптимальная длина маршрута для каждого транспорта:");
                    foreach (var distance in resultsViewModel.Distances2)
                    {
                        writer.WriteLine(distance);
                    }
                }
               
            }
        }



        public static void LoadFromFile(string filePath, out string selectedMatrixType,  out int numberOfCities, out int numberOfTransports, out int numberOfPercent, out ObservableCollection<int> transportFields, out ObservableCollection<ObservableCollection<double>> matrixFields)
        {
            selectedMatrixType = string.Empty;
            numberOfCities = 0;
            numberOfTransports = 0;
            numberOfPercent = 0;
            transportFields = new ObservableCollection<int>();
            matrixFields = new ObservableCollection<ObservableCollection<double>>();

            var lines = File.ReadAllLines(filePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

            if (Path.GetExtension(filePath).ToLower() == ".tsp")
            {
                selectedMatrixType = lines.First(line => line.StartsWith("EDGE_WEIGHT_TYPE")).Split(':')[1].Trim();
                numberOfCities = int.Parse(lines.First(line => line.StartsWith("DIMENSION")).Split(':')[1].Trim());

                if (selectedMatrixType == "EUC_2D")
                {
                    selectedMatrixType = "Матрица координат";
                    int matrixStartIndex = Array.FindIndex(lines, line => line.StartsWith("NODE_COORD_SECTION")) + 1;
                    for (int i = matrixStartIndex; i < matrixStartIndex + numberOfCities; i++)
                    {
                        var coordinates = lines[i].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray();
                        var row = new ObservableCollection<double>(coordinates);
                        matrixFields.Add(row);
                    }
                }
                else if (selectedMatrixType == "EXPLICIT")
                {
                    selectedMatrixType = "Матрица расстояний";
                    int matrixStartIndex = Array.FindIndex(lines, line => line.StartsWith("EDGE_WEIGHT_SECTION")) + 1;
                    for (int i = matrixStartIndex; i < matrixStartIndex + numberOfCities; i++)
                    {
                        var distances = lines[i].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray();
                        var row = new ObservableCollection<double>(distances);
                        matrixFields.Add(row);
                    }
                }
            }
            else {
                //var lines = File.ReadAllLines(filePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
                //selectedMatrixType = (lines.First(line => line.StartsWith("Тип матрицы:")).Split(':')[1].Trim());
                // Read number of cities

                selectedMatrixType = lines.First(line => line.StartsWith("Тип матрицы:")).Split(':')[1].Trim();
                numberOfCities = int.Parse(lines.First(line => line.StartsWith("Количество городов:")).Split(':')[1].Trim());

                // Read number of transports
                numberOfTransports = int.Parse(lines.First(line => line.StartsWith("Количество транспортных средств:")).Split(':')[1].Trim());

                // Read percentage of number replacement in sparse
                // 
                numberOfPercent = int.Parse(lines.First(line => line.StartsWith("Процент замены чисел в разреженной матрице:")).Split(':')[1].Trim());

                // Read minimum number of visited cities for each transport
                int transportFieldsIndex = Array.FindIndex(lines, line => line.StartsWith("Минимальное количество посещенных городов для каждого транспортного средства:"));
                for (int i = transportFieldsIndex + 1; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("Матрица:")) break;
                    transportFields.Add(int.Parse(lines[i].Trim()));
                }

                // Read matrix
                int matrixIndex = Array.FindIndex(lines, line => line.StartsWith("Матрица:")) + 1;
                for (int i = matrixIndex; i < lines.Length; i++)
                {
                    var row = new ObservableCollection<double>(lines[i].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Select(s => double.Parse(s, CultureInfo.InvariantCulture)));
                    matrixFields.Add(row);
                }
            }
        }


           
    }
}
