using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoTime
{
    public static class Utils
    {
        public static Vector2 VectorFromString(string vectorString)
        {
            string[] split = vectorString.Split(',');
            return new Vector2(float.Parse(split[0]), float.Parse(split[1]));
        }
        public static TimeSpan TimeFromStringMs(string timeString)
        {
            return TimeSpan.FromMilliseconds(double.Parse(timeString));
        }
        public static Random Random = new Random();
        public static float RandomF => (float)Random.NextDouble();
    }
}
