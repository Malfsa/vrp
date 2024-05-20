using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace WpfApp2
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _selectedMatrixType;
        private int _numberOfCities;
        private int _numberOfTransports;
        private double _numberOfPercent;
        private bool _isMethodSelected;
        private bool isRandomFillEnabled;
        private bool isMatrixVisible;
        private bool isTransportVisible;


      Random random = new Random();
        public MainViewModel()
        {
            MatrixTypes = new ObservableCollection<string> { "Матрица координат", "Матрица расстояний" };
            TransportFields = new ObservableCollection<int>();
            MatrixFields = new ObservableCollection<ObservableCollection<double>>();
            SelectMethod1Command = new RelayCommand(param => SelectMethod1());
            SelectMethod2Command = new RelayCommand(param => SelectMethod2());
            SaveCommand = new RelayCommand(param => Save());
            SolveCommand = new RelayCommand(param => Solve(), param => CanSolve());
           // RandomizeMatrixCommand = new RelayCommand(param=>RandomizeMatrix());
           

        }

        public ObservableCollection<string> MatrixTypes { get; }


        public bool IsRandomFillEnabled
        {
            get { return isRandomFillEnabled; }
            set
            {
                isRandomFillEnabled = value;
              //  OnPropertyChanged(nameof(IsRandomFillEnabled));
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
                UpdateMatrixFields();
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

        public double NumberOfPercent
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
            get { return isMatrixVisible; }
            set
            {
                if (isMatrixVisible != value)
                {
                    isMatrixVisible = value;
                    OnPropertyChanged(nameof(IsMatrixVisible));
                }
            }
        }

        public bool IsTransportVisible
        {
            get { return isTransportVisible; }
            set
            {
                if (isTransportVisible != value)
                {
                    isTransportVisible = value;
                    OnPropertyChanged(nameof(IsTransportVisible));
                }
            }
        }
        public ObservableCollection<int> TransportFields { get; }
        public ObservableCollection<ObservableCollection<double>> MatrixFields { get; }

        public ICommand SelectMethod1Command { get; }
        public ICommand SelectMethod2Command { get; }
        public ICommand SaveCommand { get; }
        public ICommand SolveCommand { get; }
        public ICommand RandomizeMatrixCommand { get; private set; }

        private void SelectMethod1()
        {
            IsMethodSelected = true;
        }

        private void SelectMethod2()
        {
            IsMethodSelected = true;
        }

        private void Save()
        {
            // Открытие диалогового окна для выбора пути сохранения
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "CSV file (*.csv)|*.csv|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                FileHandler.SaveToFile(saveFileDialog.FileName, SelectedMatrixType, NumberOfCities, NumberOfTransports, NumberOfPercent, MatrixFields, TransportFields);
            }
        }

        private void Load()
        {
            // Открытие диалогового окна для выбора файла загрузки
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "CSV file (*.csv)|*.csv|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                FileHandler.LoadFromFile(openFileDialog.FileName, out string selectedMatrixType, out int numberOfCities, out int numberOfTransports, out double numberOfPercent, out ObservableCollection<ObservableCollection<double>> matrixFields, out ObservableCollection<int> transportFields);

                SelectedMatrixType = selectedMatrixType;
                NumberOfCities = numberOfCities;
                NumberOfTransports = numberOfTransports;
                NumberOfPercent = numberOfPercent;
                MatrixFields.Clear();
                foreach (var row in matrixFields)
                {
                    MatrixFields.Add(row);
                }
                TransportFields.Clear();
                foreach (var field in transportFields)
                {
                    TransportFields.Add(field);
                }
            }
        }

        private bool CanSolve()
        {
            // Проверка валидности полей
            return true;
        }

        private void Solve()
        {
            // Логика решения задачи
        }

        private void UpdateTransportFields()
        {
            IsTransportVisible = true;
            TransportFields.Clear();
            for (int i = 0; i < NumberOfTransports; i++)
            {
                TransportFields.Add(0);
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
                default:
                    break;
            }
        }

        private void UpdateDistanceMatrix()
        {
            for (int i = 0; i < NumberOfCities; i++)
            {
                var row = new ObservableCollection<double>();
                for (int j = 0; j < NumberOfCities; j++)
                {
                    if (IsRandomFillEnabled)
                    {
                        row.Add(random.Next(10, 50));
                    }
                    else
                        row.Add(0.0);
                }
                MatrixFields.Add(row);
            }
        }

        private void UpdateCoordinateMatrix()
        {
            for (int i = 0; i < NumberOfCities; i++)
            {
                var row = new ObservableCollection<double> { 0.0, 0.0 };
                if (IsRandomFillEnabled)
                {
                    row = new ObservableCollection<double> { random.Next(10, 50), random.Next(10, 50) };
                    MatrixFields.Add(row);
                }
                else
                    MatrixFields.Add(row);
            }
        }








        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}