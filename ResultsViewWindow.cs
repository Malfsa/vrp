
using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    namespace WpfApp2
    {
        public class ResultsViewModel : INotifyPropertyChanged
        {
            private double? _optimalValue;
            private string _times;
           // private ObservableCollection<string> _routeMatrix;
            private ObservableCollection<string> _routes;
            private ObservableCollection<string> _distances;
            private double? _optimalValue2;
            private string _times2;
           // private ObservableCollection<string> _routeMatrix2;
            private ObservableCollection<string> _routes2;
            private ObservableCollection<string> _distances2;
            private bool isBaseMatrix;
            private bool isReplaceMatrix;
            private string _accuracy;

        public bool IsBaseMatrix
        {
            get { return isBaseMatrix; }
            set
            {
                if (isBaseMatrix != value)
                {
                    isBaseMatrix = value;
                    OnPropertyChanged();
                 //   OnPropertyChanged(nameof(IsReplaceMatrix));
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
                    OnPropertyChanged();
                  //  OnPropertyChanged(nameof(IsBaseMatrix));
                }
            }
        }
        public double? OptimalValue
            {
                get => _optimalValue;
                set
                {
                    _optimalValue = value;
                    OnPropertyChanged();
            }
            }

        public string Times
        {
            get => _times;
            set
            {
                _times = value;
                OnPropertyChanged();
            }
        }

       /* public ObservableCollection<string> RouteMatrix
            {
                get => _routeMatrix;
                set
                {
                    _routeMatrix = value;
                    OnPropertyChanged();
                }
            }*/

            public ObservableCollection<string> Routes
            {
                get => _routes;
                set
                {
                    _routes = value;
                    OnPropertyChanged();
                }
            }

            public ObservableCollection<string> Distances
            {
                get => _distances;
                set
                {
                    _distances = value;
                    OnPropertyChanged();
                    //UpdateFields()
              
            }
            }
        public double? OptimalValue2
        {
            get => _optimalValue2;
            set
            {
                _optimalValue2 = value;
                OnPropertyChanged();
            }
        }
        public string Times2
        {
            get => _times2;
            set
            {
                _times2 = value;
                OnPropertyChanged();
            }
        }
     /*   public ObservableCollection<string> RouteMatrix2
        {
            get => _routeMatrix2;
            set
            {
                _routeMatrix2 = value;
                OnPropertyChanged();
            }
        }*/

        public ObservableCollection<string> Routes2
        {
            get => _routes2;
            set
            {
                _routes2 = value;
                OnPropertyChanged();

            }
        }

        public ObservableCollection<string> Distances2
        {
            get => _distances2;
            set
            {
                _distances2 = value;
                OnPropertyChanged();
            }
        }

        public string Accuracy
        {
            get => _accuracy;
            set
            {
                _accuracy = value;
            }
        }

      /*  private void UpdateFields()
        {

            string s = "";
            for (int i = 0; i < Distances.Count; i++)
            {
                Distances.Add(new MatrixElement { Value });
            }
        }*/

        // public bool IsCheck => OptimalValue.HasValue && OptimalValue2.HasValue;
        public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

