using System;
using System.Linq;

namespace MethodsLibrary
{
    /// <summary>
    /// Реализация библиотеки численных методов решения системы нелинейных обыкновенных уравнений
    /// </summary>
    class NonlinearEquationsLibrary
    {
        /// <summary>
        /// Метод половинного деления (Дихотомии/бисекции)
        /// </summary>
        /// <param name="maxIterations"> Максимальное количество итераций </param>
        /// <param name="a"> Левая граница отрезка </param>
        /// <param name="b"> Правая граница отрезка </param>
        /// <param name="eps"> Точность решения </param>
        /// <param name="func"> Функция, для которой требуется найти корень </param>
        /// <returns> Корень функции на заданном отрезке </returns>
        public static double BisectionMethod(int maxIterations, double a, double b, double eps, Func<double, double> func)
        {
            if (func(a) * func(b) >= 0)
                throw new ArgumentException("Функция должна иметь разные знаки на концах отрезка [a, b]");

            if (func(a) == 0)
                return a;
            if (func(b) == 0)
                return b;

            double c;
            int iterations = 0;

            do
            {
                c = (a + b) / 2;

                if (func(a) * func(c) < 0)
                    b = c;
                else
                    a = c;

                iterations++;
            }
            while (Math.Abs(b - a) >= eps && iterations < maxIterations);

            return c;
        }

        /// <summary>
        /// Метод секущих (Хорд)
        /// </summary>
        /// <param name="maxIterations"> Максимальное количество итераций </param>
        /// <param name="a"> Левая граница отрезка </param>
        /// <param name="b"> Правая граница отрезка </param>
        /// <param name="eps"> Точность решения </param>
        /// <param name="func"> Функция, для которой требуется найти корень </param>
        /// <returns> Корень функции на заданном отрезке </returns>
        public static double SecantMethod(int maxIterations, double a, double b, double eps, Func<double, double> func)
        {
            double c;
            int iterations = 0;

            do
            {
                c = a - (func(a) * (b - a) / (func(b) - func(a)));

                if (func(a) * func(c) > 0)
                    a = c;
                else
                    b = c;

                iterations++;
            }
            while (Math.Abs(func(c)) >= eps && iterations < maxIterations);

            return c;
        }

        /// <summary>
        /// Метод Ньютона для решения систем нелинейных уравнений
        /// </summary>
        /// <param name="maxIterations"> Максимальное количество итераций </param>
        /// <param name="x0"> Начальное приближение </param>
        /// <param name="eps"> Точность решения </param>
        /// <param name="funcs"> Массив функций, задающих систему уравнений </param>
        /// <returns> Решение системы уравнений </returns>
        public static double[] NewtonMethod(int maxIterations, double[] x0, double eps, Func<double[], double>[] funcs)
        {
            int n = x0.Length;
            double[][] jacobian = new double[n][];
            for (int i = 0; i < n; i++)
                jacobian[i] = new double[n];

            double[] delta = new double[n];
            double[] f = new double[n];
            double[] x = new double[n];
            Array.Copy(x0, x, n);

            for (int iterations = 0; iterations < maxIterations; iterations++)
            {
                // Вычисление матрицы Якоби
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        double dx = 1e-8;
                        double[] xPlusDx = new double[n];
                        Array.Copy(x, xPlusDx, n);
                        xPlusDx[j] += dx;

                        jacobian[i][j] = (funcs[i](xPlusDx) - funcs[i](x)) / dx;
                    }
                }

                // Решение системы линейных уравнений J * delta = -F
                for (int i = 0; i < n; i++)
                    f[i] = -funcs[i](x);

                delta = GaussMethod(jacobian, f);

                // Обновление x
                for (int i = 0; i < n; i++)
                    x[i] += delta[i];

                // Проверка условия выхода
                if (Math.Abs(delta.Max()) < eps)
                    break;
            }

            return x;
        }

        /// <summary>
        /// Метод Гаусса
        /// </summary>
        /// <param name="matrix"> Матрица системы уравнений (коэффициенты перед неизвестными) </param>
        /// <param name="vector"> Вектор свободных членов </param>
        /// <returns> Решение системы уравнений </returns>
        private static double[] GaussMethod(double[][] matrix, double[] vector)
        {
            int n = vector.Length;

            // Прямой ход метода Гаусса
            for (int k = 0; k < n - 1; k++)
            {
                for (int i = k + 1; i < n; i++)
                {
                    double coeff = matrix[i][k] / matrix[k][k];

                    // Обновление коэффициентов в матрице
                    for (int j = k; j < n; j++)
                        matrix[i][j] -= coeff * matrix[k][j];

                    // Обновление вектора свободных членов
                    vector[i] -= coeff * vector[k];
                }
            }

            // Обратный ход метода Гаусса
            double[] result = new double[n];
            for (int i = n - 1; i >= 0; i--)
            {
                double sum = 0;

                for (int j = i + 1; j < n; j++)
                    sum += matrix[i][j] * result[j];

                result[i] = (vector[i] - sum) / matrix[i][i];
            }

            return result;
        }

        /// <summary>
        /// Метод секущих для решения систем нелинейных уравнений
        /// </summary>
        /// <param name="maxIterations"> Максимальное количество итераций </param>
        /// <param name="x0"> Начальное приближение </param>
        /// <param name="eps"> Точность решения </param>
        /// <param name="funcs"> Массив функций, задающих систему уравнений </param>
        /// <returns> Решение системы уравнений </returns>
        public static double[] SystemsOfEquationsSecantMethod(int maxIterations, double[] x0, double eps, Func<double[], double>[] funcs)
        {
            int n = x0.Length;
            double[] x1 = new double[n];
            double[] deltaX = new double[n];

            Array.Copy(x0, x1, n);

            int iterations = 0;

            while (iterations < maxIterations && deltaX.Max() > eps)
            {
                iterations++;

                // Сохраняем значения предыдущей итерации
                double[] prevX = new double[n];
                Array.Copy(x0, prevX, n);
                Array.Copy(x1, x0, n);

                // Обновляем значения переменных
                for (int i = 0; i < n; i++)
                {
                    if (i == 0)
                        x1[i] = x0[i] - funcs[i](x0) * (x0[i + 1] - x0[i]) / (funcs[i](x0) - funcs[i](prevX));
                    else if (i == n - 1)
                        x1[i] = x0[i] - funcs[i](x0) * (x0[i] - x0[i - 1]) / (funcs[i](x0) - funcs[i](prevX));
                    else
                        x1[i] = x0[i] - funcs[i](x0) * (x0[i + 1] - x0[i - 1]) / (funcs[i](x0) - funcs[i](prevX));

                    deltaX[i] = Math.Abs(x1[i] - x0[i]);
                }

                // Проверка условия выхода
                if (deltaX.Max() < eps)
                    break;
            }

            return x1;
        }

        /// <summary>
        /// Метод Зейделя для решения систем нелинейных уравнений
        /// </summary>
        /// <param name="maxIterations"> Максимальное количество итераций </param>
        /// <param name="x0"> Начальное приближение </param>
        /// <param name="eps"> Точность решения </param>
        /// <param name="funcs"> Массив функций, задающих систему уравнений </param>
        /// <returns> Решение системы уравнений </returns>
        public static double[] ZeidelMethod(int maxIterations, double[] x0, double eps, Func<double[], double>[] funcs)
        {
            int n = x0.Length;
            double[][] jacobian = new double[n][];
            for (int i = 0; i < n; i++)
                jacobian[i] = new double[n];

            double[] delta = new double[n];
            double[] f = new double[n];
            double[] x = new double[n];
            Array.Copy(x0, x, n);

            for (int iterations = 0; iterations < maxIterations; iterations++)
            {
                // Вычисление матрицы Якоби
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        double dx = 1e-8;
                        double[] xPlusDx = new double[n];
                        double[] xMinusDx = new double[n];
                        Array.Copy(x, xPlusDx, n);
                        Array.Copy(x, xMinusDx, n);

                        xPlusDx[j] += dx;
                        xMinusDx[j] -= dx;

                        jacobian[i][j] = (funcs[i](xPlusDx) - funcs[i](xMinusDx)) / (2 * dx);
                    }
                }

                // Решение системы линейных уравнений J * delta = -F
                for (int i = 0; i < n; i++)
                    f[i] = -funcs[i](x);

                delta = GaussMethod(jacobian, f);

                // Обновление x
                for (int i = 0; i < n; i++)
                    x[i] += delta[i];

                // Проверка условия выхода
                if (Math.Abs(delta.Max()) < eps)
                    break;
            }

            return x;
        }
    }
}