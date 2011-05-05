namespace Core.Utils
{
    using System;

    public static class MathUtils
    {
        public static double GetTrueRandom()
        {
            var seed = (int) DateTime.Now.Ticks;
            var random = new Random(seed);
            return random.NextDouble();
        }

        public static double GetTrueRandom(double low, double high)
        {
            return low + (high - low)*GetTrueRandom();
        }

        public static int GetTrueRandom(int low, int high)
        {
            return (int) Math.Round(GetTrueRandom(low, (double) high));
        }

        public static double GetScale(double max)
        {
            // based on Excel graphs, first adapted for ProphIT
            // this implementation assumes everything is positive
            var i = 1;
            var state = 0;
            while (true)
            {
                i *= 10;
                if (max < i)
                {
                    state = 1;
                    break;
                }

                if (max < (2*i))
                {
                    state = 2;
                    break;
                }

                if (max < (5*i))
                {
                    state = 5;
                    break;
                }
            }

            var stepSize = Convert.ToInt32(i/10)*state;
            var numSteps = Convert.ToInt32(Math.Ceiling(max/stepSize)) + 1;
            return (double) numSteps*stepSize;
        }

        public static double Pow(double x, int n)
        {
            if (n == 0)
            {
                return 1.0;
            }

            var ans = 1.0;
            var N = Math.Abs(n);
            for (var i = 0; i < N; i++)
            {
                ans *= x;
            }

            return (n > 0) ? ans : 1.0/ans;
        }
    }
}