
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    namespace WpfApp2
    {
        public class ResultsViewModel : INotifyPropertyChanged
        {
            private string _optimalValue;
            private ObservableCollection<string> _routeMatrix;
            private ObservableCollection<string> _routes;
            private ObservableCollection<string> _distances;

            public string OptimalValue
            {
                get => _optimalValue;
                set
                {
                    _optimalValue = value;
                    OnPropertyChanged();
                }
            }

            public ObservableCollection<string> RouteMatrix
            {
                get => _routeMatrix;
                set
                {
                    _routeMatrix = value;
                    OnPropertyChanged();
                }
            }

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
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

