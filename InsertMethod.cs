using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WpfApp2
{           
    public class GeneticAlgorithm
    {
        private static Random random = new Random();

        // Фитнес-функция: возвращает длину пути
        public static double FitnessFunction(int[] route, double[,] distanceMatrix)
        {
            double totalDistance = 0.0;
            for (int i = 0; i < route.Length - 1; i++)
            {
                totalDistance += distanceMatrix[route[i], route[i + 1]];
            }
            // Возвращаемся в начальную точку
            totalDistance += distanceMatrix[route[route.Length - 1], route[0]];
            return totalDistance;
        }

        // Инициализация популяции
        public static List<int[]> InitializePopulation(int populationSize, int numberOfCities)
        {
            List<int[]> population = new List<int[]>();
            for (int i = 0; i < populationSize; i++)
            {
                int[] route = new int[numberOfCities];
                route[0] = 0; // Начальный город фиксирован
                var remainingCities = Enumerable.Range(1, numberOfCities - 1).ToList();
                Shuffle(remainingCities);
                for (int j = 1; j < numberOfCities; j++)
                {
                    route[j] = remainingCities[j - 1];
                }
                population.Add(route);
            }
            return population;
        }

        // Случайное перемешивание списка
        private static void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        // Селекция: турнирный отбор
        public static int[] TournamentSelection(List<int[]> population, double[,] distanceMatrix)
        {
            int tournamentSize = 5;
            List<int[]> tournament = new List<int[]>();
            for (int i = 0; i < tournamentSize; i++)
            {
                int randomIndex = random.Next(population.Count);
                tournament.Add(population[randomIndex]);
            }
            return tournament.OrderBy(route => FitnessFunction(route, distanceMatrix)).First();
        }

        // Кроссинговер: упорядоченный кроссинговер (OX1)
        public static int[] Crossover(int[] parent1, int[] parent2)
        {
            int numberOfCities = parent1.Length;
            int start = random.Next(1, numberOfCities - 1);
            int end = random.Next(start, numberOfCities - 1);

            int[] child = new int[numberOfCities];
            Array.Fill(child, -1);
            child[0] = 0; // Начальный город фиксирован

            for (int i = start; i < end; i++)
            {
                child[i] = parent1[i];
            }

            int parent2Index = 1;
            for (int i = 1; i < numberOfCities; i++)
            {
                if (child[i] == -1)
                {
                    while (child.Contains(parent2[parent2Index]))
                    {
                        parent2Index++;
                    }
                    child[i] = parent2[parent2Index];
                }
            }

            return child;
        }

        // Мутация: случайная инверсия подотрезка маршрута (кроме первого города)
        public static void Mutate(int[] route)
        {
            int index1 = random.Next(1, route.Length);
            int index2 = random.Next(1, route.Length);
            if (index1 > index2)
            {
                var temp = index1;
                index1 = index2;
                index2 = temp;
            }

            Array.Reverse(route, index1, index2 - index1 + 1);
        }

        // Основной метод генетического алгоритма
        public static int[] Run(double[,] distanceMatrix, int populationSize, int generations, double mutationRate, double elitismRate)
        {
            int numberOfCities = distanceMatrix.GetLength(0);
            List<int[]> population = InitializePopulation(populationSize, numberOfCities);

            int elitismCount = (int)(populationSize * elitismRate);

            for (int generation = 0; generation < generations; generation++)
            {
                List<int[]> newPopulation = new List<int[]>();

                // Элитизм: сохранение лучших решений
                var sortedPopulation = population.OrderBy(route => FitnessFunction(route, distanceMatrix)).ToList();
                newPopulation.AddRange(sortedPopulation.Take(elitismCount));

                while (newPopulation.Count < populationSize)
                {
                    int[] parent1 = TournamentSelection(population, distanceMatrix);
                    int[] parent2 = TournamentSelection(population, distanceMatrix);
                    int[] child = Crossover(parent1, parent2);

                    if (random.NextDouble() < mutationRate)
                    {
                        Mutate(child);
                    }

                    newPopulation.Add(child);
                }

                population = newPopulation;
            }

            return population.OrderBy(route => FitnessFunction(route, distanceMatrix)).First();
        }

    }
    }