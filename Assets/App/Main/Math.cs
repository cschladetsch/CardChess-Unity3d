using System;

namespace App
{
    public static class Math
    {
        public static int Max(int a, int b) { return a > b ? a : b; }
        public static int Min(int a, int b) { return a < b ? a : b; }
        public static System.Random Rand = new Random();

        public static int RandomRanged(int min, int max)
        {
            return Rand.Next(min, max);
        }

        public static int Clamp(int min, int max, int val)
        {
            return val < min ? min : (val > max ? max : val);
        }
    }
}
