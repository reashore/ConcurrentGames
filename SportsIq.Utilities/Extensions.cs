using System;

namespace SportsIq.Utilities
{
    public static class Extensions
    {
        public static bool IsEqualToZero(this double value, double tolerance = .001)
        {
            return Math.Abs(value) < tolerance;
        }

        public static bool IsNotEqualToZero(this double value, double tolerance = .001)
        {
            return !IsEqualToZero(value, tolerance);
        }

        //------------------------------------------------------------------------

        public static bool IsEqualTo(this double value1, double value2, double tolerance = .001)
        {
            return Utils.AreEqual(value1, value2, tolerance);
        }

        public static bool IsNotEqualTo(this double value1, double value2, double tolerance = .001)
        {
            return !Utils.AreEqual(value1, value2, tolerance);
        }

        //------------------------------------------------------------------------

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsNotNullOrWhiteSpace(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}
