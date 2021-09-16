using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegralLib
{
    /// <summary>
    /// Функция одной переменной.
    /// </summary>
    /// <param name="x">Независимый параметр.</param>
    /// <returns>Значение функции.</returns>
    public delegate double Function(double x);

    /// <summary>
    /// Класс для вычисления определенного интеграла.
    /// </summary>
    public class Integral
    {
        /// <summary>
        /// Вычисление определенного интеграла.
        /// </summary>
        /// <param name="f">Функция одной переменной.</param>
        /// <param name="a">Нижний предел интегрирования.</param>
        /// <param name="b">Верхний предел интегрирования.</param>
        /// <param name="split">Число точек в разбиении.</param>
        /// <returns>Значение интеграла.</returns>
        public static double Get(Function f, double a, double b, int split = 100)
        {
            int N = Convert.ToInt32((b - a) * split);
            double S = 0;
            double dx = (b - a) / N;

            for (int i = 1; i < N; i++)
            {
                S += f(dx * i + a);
            }

            S += (f(a) + f(b)) / 2;
            S *= dx;

            return S;
        }

        /// <summary>
        /// Параллельное вычисление определенного интеграла (первый способ).
        /// </summary>
        /// <param name="f">Функция одной переменной.</param>
        /// <param name="a">Нижний предел интегрирования.</param>
        /// <param name="b">Верхний предел интегрирования.</param>
        /// <param name="split">Число точек в разбиении.</param>
        /// <returns>Значение интеграла.</returns>
        public static double GetParallel1(Function f, double a, double b, int split = 100)
        {
            double c = (a + b) / 2;

            var tasks = new List<Task<double>>
            {
                Task.Run(() => Get(f, a, c, split)), // интегрируем от a до c
                Task.Run(() => Get(f, c, b, split))  // интегрируем от c до b
            };

            Task.WaitAll(tasks.ToArray());

            return tasks.Sum(x => x.Result);
        }

        /// <summary>
        /// Параллельное вычисление определенного интеграла (второй способ через Parallel.For).
        /// </summary>
        /// <param name="f">Функция одной переменной.</param>
        /// <param name="a">Нижний предел интегрирования.</param>
        /// <param name="b">Верхний предел интегрирования.</param>
        /// <param name="split">Число точек в разбиении.</param>
        /// <returns>Значение интеграла.</returns>
        public static double GetParallel2(Function f, double a, double b, int split = 100)
        {
            double S = 0;
            object mutex = new object();
            double[] pars = new double[] { a, (a + b) / 2, b };

            Parallel.For(0, 2, i =>
            {
                int index = i;
                var v = Get(f, pars[index], pars[index + 1], split);
                lock (mutex)
                {
                    S += v;
                }
            });

            return S;
        }
    }
}
