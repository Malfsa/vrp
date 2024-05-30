using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp2
{
    public class GeneticAlgorithm
    {
        public static ObservableCollection<ObservableCollection<int>> FindOptimalRoutes(int numCities, int numTransport, double[,] distances, int[] minCityCounts)
        {
          int depotCityIndex = 0;
          
            if (minCityCounts.Sum() < numCities)
            {
                Console.WriteLine("Ошибка: Сумма значений minCityCounts превышает количество доступных городов.");
                Environment.Exit(1);
            }

            var routes = new ObservableCollection<ObservableCollection<int>>();
            var routeLengths = new List<double>(); // Список для хранения длин маршрутов

            for (int i = 0; i < numTransport; i++)
            {
                routes.Add(new ObservableCollection<int>());
            }

            // Initialize citiesVisited to keep track of visited cities (except depot)
            var citiesVisited = new HashSet<int> { depotCityIndex };

            // Initialize list to keep track of unvisited cities
            var unvisitedCities = new List<int>();
            for (int i = 0; i < numCities; i++)
            {
                if (i != depotCityIndex) // Exclude depot city
                {
                    unvisitedCities.Add(i);
                }
            }

            // Sort unvisited cities by distance from the depot
            unvisitedCities.Sort((a, b) => distances[depotCityIndex, a].CompareTo(distances[depotCityIndex, b]));

            // Assign depot city to each transport's initial route
            for (int transportIdx = 0; transportIdx < numTransport; transportIdx++)
            {
                routes[transportIdx].Add(depotCityIndex);
            }

            // Main loop to assign cities to transports ensuring minimum visits
            foreach (var city in unvisitedCities)
            {
                int transportIdx = FindTransportWithMinimumCities(routes, minCityCounts);
                routes[transportIdx].Add(city);
                citiesVisited.Add(city);
            }

            // Ensure that each transport meets its minimum city count requirement
            for (int transportIdx = 0; transportIdx < numTransport; transportIdx++)
            {
                while (routes[transportIdx].Count - 1 < minCityCounts[transportIdx]) // -1 because depot is included
                {
                    int nextCity = GetClosestUnvisitedCity(distances, routes[transportIdx], citiesVisited);
                    if (nextCity == -1) break; // No unvisited cities left
                    routes[transportIdx].Add(nextCity);
                    citiesVisited.Add(nextCity);
                }
            }

            // Complete routes by returning to depot city and calculate route lengths
            for (int transportIdx = 0; transportIdx < numTransport; transportIdx++)
            {
                routes[transportIdx].Add(depotCityIndex);
                // Вычисляем длину маршрута
                double routeLength = 0;
                for (int i = 0; i < routes[transportIdx].Count - 1; i++)
                {
                    int city1 = routes[transportIdx][i];
                    int city2 = routes[transportIdx][i + 1];
                    routeLength += distances[city1, city2];
                }
                routeLengths.Add(routeLength);
            }

            // Выводим длину каждого маршрута
            for (int i = 0; i < numTransport; i++)
            {
               MessageBox.Show($"Длина маршрута {i + 1}: {routeLengths[i]}");
            }

            return routes;
        }

        static int FindTransportWithMinimumCities(ObservableCollection<ObservableCollection<int>> routes, int[] minCityCounts)
        {
            int minIndex = -1;
            int minCities = int.MaxValue;

            for (int i = 0; i < routes.Count; i++)
            {
                int citiesCount = routes[i].Count - 1; // -1 to exclude depot
                if (citiesCount < minCityCounts[i] && citiesCount < minCities)
                {
                    minIndex = i;
                    minCities = citiesCount;
                }
            }

            return minIndex == -1 ? 0 : minIndex; // If no transport needs more cities, return the first one
        }

        static int GetClosestUnvisitedCity(double[,] distances, ObservableCollection<int> route, HashSet<int> citiesVisited)
        {
            int lastCity = route[route.Count - 1];
            int closestCity = -1;
            double minDistance = int.MaxValue;

            for (int city = 0; city < distances.GetLength(0); city++)
            {
                if (!citiesVisited.Contains(city) && distances[lastCity, city] < minDistance)
                {
                    closestCity = city;
                    minDistance = distances[lastCity, city];
                }
            }

            return closestCity;
        }

    }
}
