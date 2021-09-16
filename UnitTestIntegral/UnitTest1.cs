using System;
using IntegralLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestIntegral
{
    [TestClass]
    public class UnitTest1
    {
        double Epsilon => 0.001;

        [TestMethod]
        [Description("Тестируем интеграл от y=x^2 на отрезке от 0 до 2.")]
        public void TestMethod1()
        {
            double S = Integral.Get(x => x * x, 0, 2);
            Assert.IsTrue(Math.Abs(S - 8.0 / 3) < Epsilon);
        }

        [TestMethod]
        [Description("Тестируем интеграл от y=sin(x) на отрезке от -pi/2 до pi/2.")]
        public void TestMethod2()
        {
            double S = Integral.Get(x => Math.Sin(x), 0, Math.PI);
            Assert.IsTrue(Math.Abs(S - 2) < Epsilon);
        }

        [TestMethod]
        [Description("Убеждаемся, что синхронный метод вычисления интеграла и параллельный (1) дают одинаковые результаты.")]
        public void TestIntegralSyncAndParallel1()
        {
            const double edge = 1000;
            const double epsilon = 0.0001;

            double s1 = Integral.Get(x => 1.0 / (1.0 + x * x), -edge, edge);
            double s2 = Integral.GetParallel1(x => 1.0 / (1.0 + x * x), -edge, edge);

            Assert.IsTrue(Math.Abs(s1 - s2) < epsilon);
        }

        [TestMethod]
        [Description("Убеждаемся, что синхронный метод вычисления интеграла и параллельный (2) дают одинаковые результаты.")]
        public void TestIntegralSyncAndParallel2()
        {
            const double edge = 1000;
            const double epsilon = 0.0001;

            double s1 = Integral.Get(x => 1.0 / (1.0 + x * x), -edge, edge);
            double s2 = Integral.GetParallel2(x => 1.0 / (1.0 + x * x), -edge, edge);

            Assert.IsTrue(Math.Abs(s1 - s2) < epsilon);
        }

        [TestMethod]
        [Description("Убеждаемся, что паралльное (1) вычисление интеграла работает быстрее.")]
        public void TestIntegralPerformance1()
        {
            const double edge = 20000;
            const int split = 1000;

            System.Diagnostics.Stopwatch sw1 = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch sw2 = new System.Diagnostics.Stopwatch();

            for (int i = 0; i < 10; i++)
            {
                sw1.Start();
                double s1 = Integral.Get(x => 1.0 / (1.0 + x * x), -edge, edge, split);
                sw1.Stop();

                sw2.Start();
                double s2 = Integral.GetParallel1(x => 1.0 / (1.0 + x * x), -edge, edge, split);
                sw2.Stop();
            }

            long timeSync = sw1.ElapsedMilliseconds;
            long timeParallel = sw2.ElapsedMilliseconds;

            Assert.IsTrue(timeParallel < timeSync);
        }

        [TestMethod]
        [Description("Убеждаемся, что паралльное (2) вычисление интеграла работает быстрее.")]
        public void TestIntegralPerformance2()
        {
            const double edge = 20000;
            const int split = 1000;

            System.Diagnostics.Stopwatch sw1 = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch sw2 = new System.Diagnostics.Stopwatch();

            for (int i = 0; i < 10; i++)
            {
                sw1.Start();
                double s1 = Integral.Get(x => 1.0 / (1.0 + x * x), -edge, edge, split);
                sw1.Stop();

                sw2.Start();
                double s2 = Integral.GetParallel2(x => 1.0 / (1.0 + x * x), -edge, edge, split);
                sw2.Stop();
            }

            long tSync = sw1.ElapsedMilliseconds;
            long tParallel = sw2.ElapsedMilliseconds;

            Assert.IsTrue(tParallel < tSync);
        }
    }
}
