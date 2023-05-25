using System;

namespace MethodsLibrary
{
    public class Solve
    {
        public static double Func(double x)
        {
            return Math.Cos(x) / x;
        }

        // Метод касательных (Ньютона)
        public static double dFunc(double x)
        {
            return -(x * Math.Sin(x) + Math.Cos(x) / Math.Pow(x, 2));
        }

        public static double d2Func(double x)
        {
            return -(Math.Pow(x, 2) * Math.Cos(x) - 2 * x * Math.Sin(x) - 2 * Math.Cos(x) / Math.Pow(x, 3));
        }

        // Первая система нелинейных обыкновенных уравнений: метод Ньютона. При h = 0.001, x0 = {1; 5}
        public static double Func1(double[] x)
        {
            return x[0] + x[1] - 3;
        }

        public static double Func2(double[] x)
        {
            return x[0] * x[0] + x[1] * x[1] - 9;
        }

        // Вторая система нелинейных обыкновенных уравнений: метод Ньютона. При h = 0.005, x0 = {0.5; 0.5; 0.5}
        public static double Func3(double[] x)
        {
            return x[0] * x[0] + x[1] * x[1] + x[2] * x[2] - 1;
        }

        public static double Func4(double[] x)
        {
            return 2 * x[0] * x[0] + x[1] * x[1] - 4 * x[2];
        }

        public static double Func5(double[] x)
        {
            return 3 * x[0] * x[0] - 4 * x[1] + x[2] * x[2];
        }

        // Система нелинейных обыкновенных уравнений: метод секущих. При h = 0.001, x0 = {1; 5}
        public static double Func6(double[] x)
        {
            return x[0] + x[1] - 3;
        }

        public static double Func7(double[] x)
        {
            return x[0] * x[0] + x[1] * x[1] - 9;
        }

        // Система нелинейных обыкновенных уравнений: метод Зейделя. При h = 0.001, x0 = {3.5; 2.2}
        public static double Func8(double[] x)
        {
            return 2 * x[0] * x[0] - x[0] * x[1] - 5 * x[0] + 1;
        }

        public static double Func9(double[] x)
        {
            return x[0] + 3 * Math.Log10(x[0]) - x[1] * x[1];
        }
    }
}