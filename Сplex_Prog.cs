using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ILOG.Concert;
using ILOG.CPLEX;
using Exception = ILOG.Concert.Exception;
namespace WpfApp2
{
    public class Cplex_Prog
    {
        public (Cplex cplex, INumVar[][][] x, INumVar[] Distance) MyCplex(int n, int m, double[,] startc1, double[,] c, int[] w)
        {
            double[,] startc = CopyMatrix(startc1, n);

            try
            {
                Cplex cplex = new Cplex();

                // Создание переменных
                INumVar[][][] x = CreateDecisionVariables(cplex, n, m);
                INumVar[] Distance = cplex.NumVarArray(m, 0, double.MaxValue);
                INumVar[][] y = CreateBinaryMatrix(cplex, n, m);
                INumVar[] s = cplex.NumVarArray(n - 1, 0, double.MaxValue);

                // Целевая функция
                AddObjectiveFunction(cplex, Distance, m);

                // Ограничения
                AddConstraints(cplex, n, m, x, y, s, c, Distance, w);

                // Решение модели
                if (cplex.Solve())
                {
                    return (cplex, x, Distance);
                }
                else
                {
                    return (null, null, null);
                }
            }
            catch (ILOG.Concert.Exception e)
            {
                throw new Exception("Error: " + e.Message);
            }
        }

        private double[,] CopyMatrix(double[,] source, int size)
        {
            double[,] copy = new double[size, size];
            Array.Copy(source, copy, source.Length);
            return copy;
        }

        private INumVar[][][] CreateDecisionVariables(Cplex cplex, int n, int m)
        {
            INumVar[][][] x = new INumVar[m][][];
            for (int k = 0; k < m; k++)
            {
                x[k] = new INumVar[n][];
                for (int i = 0; i < n; i++)
                {
                    x[k][i] = cplex.BoolVarArray(n);
                }
            }
            return x;
        }

        private INumVar[][] CreateBinaryMatrix(Cplex cplex, int n, int m)
        {
            INumVar[][] y = new INumVar[n][];
            for (int i = 0; i < n; i++)
            {
                y[i] = cplex.BoolVarArray(m);
            }
            return y;
        }

        private void AddObjectiveFunction(Cplex cplex, INumVar[] Distance, int m)
        {
            ILinearNumExpr objExpr = cplex.LinearNumExpr();
            for (int k = 0; k < m; k++)
            {
                objExpr.AddTerm(1, Distance[k]);
            }
            cplex.AddMinimize(objExpr);
        }

        private void AddConstraints(Cplex cplex, int n, int m, INumVar[][][] x, INumVar[][] y, INumVar[] s, double[,] c, INumVar[] Distance, int[] w)
        {
            // Constraint 1: Each transport reaches the client and depot once
            for (int i = 1; i < n; i++)
            {
                ILinearNumExpr expr = cplex.LinearNumExpr();
                for (int k = 0; k < m; k++)
                {
                    expr.AddTerm(1, y[i][k]);
                }
                cplex.AddEq(expr, 1);
            }

            // Constraint 2: Each vehicle departs from depot only once
            for (int k = 0; k < m; k++)
            {
                for (int j = 0; j < n; j++)
                {
                    ILinearNumExpr expr = cplex.LinearNumExpr();
                    for (int i = 0; i < n; i++)
                    {
                        if (i != j)
                            expr.AddTerm(1, x[k][i][j]);
                    }
                    cplex.AddEq(expr, y[j][k]);
                }
            }

            // Constraint 3: Each vehicle starts and ends at the depot
            for (int k = 0; k < m; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    ILinearNumExpr expr = cplex.LinearNumExpr();
                    for (int j = 0; j < n; j++)
                    {
                        if (i != j)
                            expr.AddTerm(1, x[k][i][j]);
                    }
                    cplex.AddEq(expr, y[i][k]);
                }
            }

            // Constraint 4: Each vehicle starts and ends at the depot
            for (int k = 0; k < m; k++)
            {
                AddDepotConstraints(cplex, x, k, n);
            }

            // Constraint 5: Distance calculation for each vehicle
            for (int k = 0; k < m; k++)
            {
                ILinearNumExpr expr1 = cplex.LinearNumExpr();
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        expr1.AddTerm(c[i, j], x[k][i][j]);
                    }
                }
                cplex.AddEq(Distance[k], expr1);
            }

            // Constraint 6: Sub-tour elimination
            AddSubTourEliminationConstraints(cplex, x, s, n, m);

            // Constraint 7: Service constraints
            for (int i = 0; i < n - 1; i++)
            {
                cplex.AddGe(s[i], 1);
                cplex.AddLe(s[i], 100);
            }

            // Constraint 8: Transport weight constraints
            for (int k = 0; k < m; k++)
            {
                ILinearNumExpr expr = cplex.LinearNumExpr();
                for (int i = 0; i < n; i++)
                {
                    expr.AddTerm(1, y[i][k]);
                }
                cplex.AddLe(expr, n);
                cplex.AddGe(expr, w[k]);
            }
        }

        private void AddDepotConstraints(Cplex cplex, INumVar[][][] x, int k, int n)
        {
            ILinearNumExpr expr1 = cplex.LinearNumExpr();
            ILinearNumExpr expr2 = cplex.LinearNumExpr();
            for (int i = 1; i < n; i++)
            {
                expr1.AddTerm(1, x[k][i][0]);
                expr2.AddTerm(1, x[k][0][i]);
            }
            cplex.AddLe(expr1, 1);
            cplex.AddLe(expr2, 1);
        }

        private void AddSubTourEliminationConstraints(Cplex cplex, INumVar[][][] x, INumVar[] s, int n, int m)
        {
            for (int k = 0; k < m; k++)
            {
                for (int i = 1; i < n; i++)
                {
                    for (int j = 1; j < n; j++)
                    {
                        if (i != j)
                            cplex.AddLe(cplex.Sum(s[i - 1], cplex.Prod(-1, s[j - 1]), cplex.Prod(100, x[k][i][j])), 99);
                    }
                }
            }
        }
    }
}
