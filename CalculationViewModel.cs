using ILOG.Concert;
using ILOG.CPLEX;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace WpfApp2
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ResultsViewModel _resultsViewModel;
        private string _selectedMatrixType;
        private int _numberOfCities;
        private int _numberOfTransports;
        private int _numberOfPercent; // Изменен тип на double
        private bool _isMethodSelected;

        private bool isMethod1Selected;
        private bool isMethod2Selected;

        private bool _isRandomFillEnabled;
        private bool _isMatrixVisible;
        private bool _isTransportVisible;
        private ObservableCollection<ObservableInt> _transportFields;
        // private ObservableCollection<ObservableCollection<double>> _matrixFields;
        private ObservableCollection<ObservableCollection<MatrixElement>> _matrixFields;
        private readonly Random random = new Random();
        private bool isBaseMatrix;
        private bool isReplaceMatrix;
        public double[] matrixfun = new double[2];

      /*  private string insertionMethodResult;

        public string InsertionMethodResult
        {
            get => insertionMethodResult;
            set
            {
                insertionMethodResult = value;
                OnPropertyChanged(nameof(InsertionMethodResult));
            }
        }
      */

        // private double[] _matrixfun=new double[2];

        public MainViewModel()
        {
            MatrixTypes = new ObservableCollection<string> { "Матрица координат", "Матрица расстояний" };
            TransportFields = new ObservableCollection<ObservableInt>();
            MatrixFields = new ObservableCollection<ObservableCollection<MatrixElement>>();

            SelectMethod1Command = new RelayCommand(param => SelectMethod1());
            SelectMethod2Command = new RelayCommand(param => SelectMethod2());

            _resultsViewModel = new ResultsViewModel();
            SaveCommand = new RelayCommand(param => Save());
            SaveResCommand= new RelayCommand(param => SaveRes());
            SolveCommand = new RelayCommand(param => Solve(), param => CanSolve());
            LoadCommand = new RelayCommand(param => Load());
        }



        public ObservableCollection<string> MatrixTypes { get; }
        public ObservableCollection<ObservableInt> TransportFields
        {
            get => _transportFields;
            set
            {
                _transportFields = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ObservableCollection<MatrixElement>> MatrixFields
        {
            get { return _matrixFields; }
            set
            {
                _matrixFields = value;
                OnPropertyChanged();
            }
        }

    public bool IsMethod1Selected
    {
        get => isMethod1Selected;
        set
        {
              
            isMethod1Selected = value;
            OnPropertyChanged(nameof(IsMethod1Selected));
            OnPropertyChanged(nameof(IsMethod2Selected));
        }
    }

    public bool IsMethod2Selected
    {
        get => isMethod2Selected;
        set
        {
            isMethod2Selected = value;
            OnPropertyChanged(nameof(IsMethod2Selected));
            OnPropertyChanged(nameof(IsMethod1Selected));
        }
    }
        public ResultsViewModel ResultsViewModel_
        {
            get => _resultsViewModel;
            set
            {
                _resultsViewModel = value;
                OnPropertyChanged();
            }
        }
        public bool IsRandomFillEnabled
        {
            get => _isRandomFillEnabled;
            set
            {
                _isRandomFillEnabled = value;
                OnPropertyChanged();
                UpdateMatrixFields();
            }
        }

        public string SelectedMatrixType
        {
            get => _selectedMatrixType;
            set
            {
                _selectedMatrixType = value;
                OnPropertyChanged();

            }
        }

        public int NumberOfCities
        {
            get => _numberOfCities;
            set
            {
                _numberOfCities = value;
                OnPropertyChanged();
                UpdateMatrixFields();
            }
        }

        public int NumberOfTransports
        {
            get => _numberOfTransports;
            set
            {
                _numberOfTransports = value;
                OnPropertyChanged();
                UpdateTransportFields();
            }
        }
        public int NumberOfPercent // Изменен тип на double
        {
            get => _numberOfPercent;
            set
            {
                _numberOfPercent = value;
                OnPropertyChanged();
            }
        }

        public bool IsMethodSelected
        {
            get => _isMethodSelected;
            set
            {
                _isMethodSelected = value;
                OnPropertyChanged();
            }
        }

        public bool IsMatrixVisible
        {
            get => _isMatrixVisible;
            set
            {
                _isMatrixVisible = value;
                OnPropertyChanged();
            }
        }
        public bool IsTransportVisible
        {
            get => _isTransportVisible;
            set
            {
                _isTransportVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsBaseMatrix
        {
            get { return isBaseMatrix; }
            set
            {
                if (isBaseMatrix != value)
                {
                    isBaseMatrix = value;
                    OnPropertyChanged(nameof(IsBaseMatrix));
                }
            }
        }

        public bool IsReplaceMatrix
        {
            get { return isReplaceMatrix; }
            set
            {
                if (isReplaceMatrix != value)
                {
                    isReplaceMatrix = value;
                    OnPropertyChanged(nameof(IsReplaceMatrix));
                }
            }
        }
        public ICommand SelectMethod1Command { get; }
        public ICommand SelectMethod2Command { get; }
        public ICommand SaveCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand SolveCommand { get; }

        public ICommand SaveResCommand { get; }

        private void SelectMethod1()
        {
            IsMethodSelected = true;
            IsMethod1Selected = true;
            IsMethod2Selected = false;
        }

        private void SelectMethod2()
        {
            IsMethodSelected = true;
            IsMethod1Selected = false;
            IsMethod2Selected = true;
        }
      /*  private void SelectMethod1()
        {
            IsMethodSelected = true;
           
        }

        private void SelectMethod2()
        {
            IsMethodSelected = true;
        }*/

        private void Save()
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "All Files (*.*)|*.*|Text Files (*.txt)|*.txt"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                FileHandler.SaveToFile(saveFileDialog.FileName, SelectedMatrixType, NumberOfCities, NumberOfTransports, NumberOfPercent, MatrixFields, TransportFields);
            }
        }
        private void SaveRes()
        {
            
         Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
        {
            Filter = "All Files (*.*)|*.*|Text Files (*.txt)|*.txt"
        };
            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                FileHandler.SaveData(filePath, SelectedMatrixType, NumberOfCities, NumberOfTransports, NumberOfPercent, MatrixFields, TransportFields, ResultsViewModel_,this);
            }
        }

        private void Load()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text file (*.txt)|*.txt|TSP file (*.tsp)|*.tsp|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                FileHandler.LoadFromFile(openFileDialog.FileName, out string selectedMatrixType, out int numberOfCities, out int numberOfTransports, out int numberOfPercent, out ObservableCollection<int> transportFields, out ObservableCollection<ObservableCollection<double>> loadedMatrixFields);

                SelectedMatrixType = selectedMatrixType;
                NumberOfCities = numberOfCities;
                NumberOfTransports = numberOfTransports;
                NumberOfPercent = numberOfPercent;

                TransportFields.Clear();
                foreach (var field in transportFields)
                {
                    TransportFields.Add(new ObservableInt { Value = int.Parse(field.ToString()) });
                }

                MatrixFields.Clear();
                foreach (var row in loadedMatrixFields)
                {
                    var newRow = new ObservableCollection<MatrixElement>();
                    foreach (var value in row)
                    {
                        newRow.Add(new MatrixElement { Value = value.ToString() });
                    }
                    MatrixFields.Add(newRow);
                }
            }
        }

        private void UpdateTransportFields()
        {
            IsTransportVisible = true;
            TransportFields.Clear();
            for (int i = 0; i < NumberOfTransports; i++)
            {
                TransportFields.Add(new ObservableInt { Value = 0 });
            }
        }

        private void UpdateMatrixFields()
        {
            IsMatrixVisible = true;
            MatrixFields.Clear();

            switch (SelectedMatrixType)
            {
                case "Матрица расстояний":
                    UpdateDistanceMatrix();
                    break;
                case "Матрица координат":
                    UpdateCoordinateMatrix();
                    break;
            }
        }
        private void UpdateDistanceMatrix()
        {
            for (int i = 0; i < NumberOfCities; i++)
            {
                var row = new ObservableCollection<MatrixElement>();
                for (int j = 0; j < NumberOfCities; j++)
                {
                    row.Add(new MatrixElement { Value = (IsRandomFillEnabled ? random.Next(10, 50) : 0.0).ToString() });
                }
                MatrixFields.Add(row);
            }
        }
        private void UpdateCoordinateMatrix()
        {
            for (int i = 0; i < NumberOfCities; i++)
            {
                var row = new ObservableCollection<MatrixElement>
        {
            new MatrixElement { Value = (IsRandomFillEnabled ? random.Next(10, 50) : 0.0).ToString()},
            new MatrixElement { Value = (IsRandomFillEnabled ? random.Next(10, 50) : 0.0).ToString() }
        };
                MatrixFields.Add(row);
            }
        }

        private bool CanSolve()
        {

            return true;
        }

        private void Solve()
        {
            if (CanSolve())
            {
                double[,] matrix = ReadMatrixFromFields();
                if (matrix == null) return;

                double[,] intMatrix = CreateIntegerMatrix(matrix);
                int[] transportWeights = GetTransportWeights();
                
                if (IsBaseMatrix)
                    {
                        try
                        {
                            if (IsMethod1Selected)
                                SolveWithCplex(matrix, intMatrix, transportWeights);
                            else
                        {

                
                                var optimalRoutes = GeneticAlgorithm.FindOptimalRoutes(NumberOfCities, NumberOfTransports, matrix, transportWeights);


                      
                                var routes = new ObservableCollection<string>();


                                // Console.WriteLine("Optimal Routes:");
                                for (int i = 0; i < optimalRoutes.Count; i++)
                                {
                                   // Console.Write($"Transport {i + 1}: ");
                                    foreach (var city in optimalRoutes[i])
                                    {
                                    routes.Add((city.ToString())); 
                                    }
                                   // Console.WriteLine();
                                }
                            ResultsViewModel_.Routes = routes;

                            //var result = InsertMethod.SolveUsingInsertionMethod(NumberOfCities, matrix, intMatrix);
                            // foreach (var item in result)
                            //{
                            //    ResultsViewModel_.Routes.Add(item);
                            //}


                            ResultsViewModel resultViewModel = new ResultsViewModel
                            {
                                IsBaseMatrix = true,
                                // Accuracy = rs.Accuracy,
                                // OptimalValue2 = rs.OptimalValue2,
                                // Times2 = $"Время выполнения: {rs.Times2} мс",
                                // RouteMatrix2 = routematrix,

                                Routes = ResultsViewModel_.Routes
                              // Distances2 = rs.Distances2
                          };
                           
                            ResultsWindow resultWindow = new ResultsWindow(resultViewModel);
                            resultWindow.Show();
                        }
                                
                           
                            
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show("Ошибка: " + ex.Message);
                        }
                    }
                    if (IsReplaceMatrix)
                    {
                        try
                        {
                            SolveWithCplex_(intMatrix, matrix, transportWeights);
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show("Ошибка: " + ex.Message);
                        }
                    }
                    if ((!IsReplaceMatrix) && (!IsBaseMatrix))
                    {
                        MessageBox.Show("Выберите матрицы для расчета(базовая, разреженная)");
                        return;
                    }

                
                else if (IsMethod2Selected)
                {
                  
                    var optimalRoutes =   GeneticAlgorithm.FindOptimalRoutes(NumberOfCities, NumberOfTransports, matrix, transportWeights);
                    Console.WriteLine("Optimal Routes:");
                    for (int i = 0; i < optimalRoutes.Count; i++)
                    {
                        Console.Write($"Transport {i + 1}: ");
                        foreach (var city in optimalRoutes[i])
                        {
                            Console.Write($"{city} ");
                        }
                        Console.WriteLine();
                    }
                }
               

            }
        }

        private double[,] ReadMatrixFromFields()
        {
            double[,] matrix = new double[NumberOfCities, NumberOfCities];
            
                if (SelectedMatrixType == "Матрица координат")
                {
                    for (int i = 0; i < NumberOfCities; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                        try
                        {
                            matrix[i, j] = double.Parse(MatrixFields[i][j].Value);
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show("Ошибка: " + ex.Message);
                            matrix = null;
                            return matrix;
                        }
                    }
                    }
                    matrix = ConverCoordinateMatrix.ConvertCoordinatesToDistanceMatrix(matrix);
                }
                else if (SelectedMatrixType == "Матрица расстояний")
                {
                for (int i = 0; i < NumberOfCities; i++)
                {
                    for (int j = 0; j < NumberOfCities; j++)
                    {
                        try
                        {
                            matrix[i, j] = double.Parse(MatrixFields[i][j].Value);
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show("Ошибка: " + ex.Message);
                            matrix = null;
                            return matrix;
                        }
                    }
                }
            }
            return matrix;
        }

        private double[,] CreateIntegerMatrix(double[,] matrix)
        {
            double[,] intMatrix = new double[NumberOfCities, NumberOfCities];
            for (int i = 0; i < NumberOfCities; i++)
            {
                for (int j = 0; j < NumberOfCities; j++)
                {
                    intMatrix[i, j] = matrix[i, j];
                }
            }
            return intMatrix;
        }

        private int[] GetTransportWeights()
        {
            return TransportFields.Select(o => o.Value).ToArray();
        }

        private void SolveWithCplex(double[,] matrix, double[,] intMatrix, int[] transportWeights)
        {
          
            Cplex_Prog cplexProg = new Cplex_Prog();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var (cplex, x, Distance) = cplexProg.MyCplex(NumberOfCities, NumberOfTransports, matrix, intMatrix, transportWeights);
            stopwatch.Stop();
            TimeSpan elapsedTime = stopwatch.Elapsed;
            

            if (cplex != null)
            {
                matrixfun[0] = cplex.GetObjValue();

                ResultsViewModel_.OptimalValue = OptimalForSpirse(cplex, x, matrix);
                ResultsViewModel_.Times = elapsedTime.TotalMilliseconds.ToString();
                //ResultsViewModel_.RouteMatrix = new ObservableCollection<string>(FormatRouteMatrix(cplex, x));
                ResultsViewModel_.Routes = new ObservableCollection<string>(FormatRoutes(cplex, x));
                ResultsViewModel_.Distances = new ObservableCollection<string>(FormatDistances(cplex, Distance));
               // ResultsViewModel_.IsBaseMatrix = true;
                ShowResultsBase(ResultsViewModel_);
            }
            else
            {
                MessageBox.Show("Решение не найдено.");
            }
        }
        private void SolveWithCplex_(double[,] matrix, double[,] intMatrix, int[] transportWeights)
        {
            //  double[,] starc = CreateIntegerMatrix(intMatrix);
            Cplex_Prog cplexProg = new Cplex_Prog();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            double[,] cringe = MatrixReplace.ReplaceTopValuesWith(intMatrix, 200000, NumberOfPercent);
            var (cplex, x, Distance) = cplexProg.MyCplex(NumberOfCities, NumberOfTransports, matrix, cringe, transportWeights);
            stopwatch.Stop();
            TimeSpan elapsedTime = stopwatch.Elapsed;
           
            if (cplex != null)
            {
                string accuracy;
                matrixfun[1] = OptimalForSpirse(cplex, x, matrix);
                if ((matrixfun[0] != 0) && (matrixfun[1] != 0))
                {
                    accuracy = (Math.Abs((matrixfun[1] - matrixfun[0]) / matrixfun[0]) * 100).ToString();
                }
                else accuracy = "Расчитайте оптимальное для обеих матриц, чтобы получить точность";

                ResultsViewModel_.OptimalValue2 = OptimalForSpirse(cplex, x, matrix);
                 ResultsViewModel_.Times2= elapsedTime.TotalMilliseconds.ToString();
                 ResultsViewModel_.Accuracy = accuracy;
                // ResultsViewModel_.RouteMatrix2 = new ObservableCollection<string>(FormatRouteMatrix(cplex, x));
                 ResultsViewModel_.Routes2 = new ObservableCollection<string>(FormatRoutes(cplex, x));
                 ResultsViewModel_.Distances2 = new ObservableCollection<string>(FormatDistances(cplex, Distance));
                //ResultsViewModel_.IsReplaceMatrix = IsReplaceMatrix;
                ShowResultsSpire(ResultsViewModel_);
            }
            else
            {
                MessageBox.Show("Решение не найдено.");
            }
        }

        private void ShowResultsSpire(ResultsViewModel rs)
        {

            ResultsViewModel resultViewModel = new ResultsViewModel
            {
                IsReplaceMatrix = true,
                Accuracy = rs.Accuracy,
                OptimalValue2 = rs.OptimalValue2,
                Times2 = $"Время выполнения: {rs.Times2} мс",
                // RouteMatrix2 = routematrix,
                Routes2 =rs.Routes2,
                Distances2 =rs.Distances2

            };
         
            ResultsWindow resultWindow = new ResultsWindow(resultViewModel);
            resultWindow.Show();
        }
        private void ShowResultsBase(ResultsViewModel rs)
        {

            ResultsViewModel resultViewModel = new ResultsViewModel
            {

                IsBaseMatrix = true,
                //Accuracy = rs.Accuracy,
                OptimalValue = rs.OptimalValue,
                Times = $"Время выполнения: {rs.Times} мс",
                // RouteMatrix2 = routematrix,
                Routes = rs.Routes,
                Distances = rs.Distances
            };
            ResultsWindow resultWindow = new ResultsWindow(resultViewModel);
            resultWindow.Show();
        }


        private double OptimalForSpirse(Cplex cplex, INumVar[][][] x, double[,] startc)
        {
            double optimalValue = 0;
            for (int k = 0; k < x.Length; k++)
            {
              //  yield return $"Транспортное средство {i + 1}: ";

                var routes = new List<(int From, int To)>();
                for (int i = 0; i < x[k].Length; i++)
                {
                    for (int j = 0; j < x[k][i].Length;j++)
                    {
                        if (cplex.GetValue(x[k][i][j]) > 0.5)
                        {
                            optimalValue += cplex.GetValue(x[k][i][j]) * startc[i, j];
                        }
                    }
                }
            }
            return optimalValue;
        }
       /* private IEnumerable<string> FormatRouteMatrix(Cplex cplex, INumVar[][][] x)
        {
            for (int i = 0; i < x.Length; i++)
            {
                for (int j = 0; j < x[i].Length; j++)
                {
                    for (int k = 0; k < x[i][j].Length; k++)
                    {
                        if (cplex.GetValue(x[i][j][k]) > 0.5)
                        {

                            yield return $"{i} : {j}-{k}: {cplex.GetValue(x[i][j][k])}";
                        }
                    }
                }
            }
        }*/

        private IEnumerable<string> FormatRoutes(Cplex cplex, INumVar[][][] x)
        {
            for (int i = 0; i < x.Length; i++)
            {
                yield return $"Транспортное средство {i + 1}: ";

                var routes = new List<(int From, int To)>();
                for (int j = 0; j < x[i].Length; j++)
                {
                    for (int k = 0; k < x[i][j].Length; k++)
                    {
                        if (cplex.GetValue(x[i][j][k]) > 0.5)
                        {
                            routes.Add((j, k));
                        }
                    }
                }

                // Поиск маршрута начиная с узла 0
                var sortedRoutes = new List<(int From, int To)>();
                int current = 0;

                while (routes.Count > 0)
                {
                    var nextRoute = routes.FirstOrDefault(r => r.From == current);
                    if (nextRoute.Equals(default((int, int))))
                    {
                        break;
                    }
                    sortedRoutes.Add(nextRoute);
                    routes.Remove(nextRoute);
                    current = nextRoute.To;
                }

                // Форматирование вывода
                foreach (var route in sortedRoutes)
                {
                    yield return $"{route.From + 1} -> {route.To + 1}";
                }
                yield return "\n";
            }
        }

        private IEnumerable<string> FormatDistances(Cplex cplex, INumVar[] Distance)
        {
            for (int i = 0; i < Distance.Length; i++)
            {
                yield return $"Транспортное средство {i + 1}: {cplex.GetValue(Distance[i])}\n";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}