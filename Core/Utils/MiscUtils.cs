namespace Core.Utils
{
    using System;

    public class MiscUtils
    {
        public static T ParseEnum<T>(string name)
        {
            return (T) Enum.Parse(typeof (T), name);
        }

        public static void Swap<T>(ref T a, ref T b)
        {
            var tmp = a;
            a = b;
            b = tmp;
        }
    }
}