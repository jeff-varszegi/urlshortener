using System;

namespace UrlShortener
{
    /// <summary>Provides application/wide extension logic for numeric types</summary>
    public static partial class NumericExtensions
    {
        /// <summary>Restricts this number to the specified range</summary>
        /// <param name="i">this number</param>
        /// <param name="lowerBound">The lower bound to which to restrict</param>
        /// <param name="higherBound">The upper bound to which to restrict</param>
        /// <returns>A number within the specified range</returns>
        public static int RestrictTo(this int i, int lowerBound, int higherBound)
        {
            if (higherBound < lowerBound)
                return Math.Min(Math.Max(i, higherBound), lowerBound);
            else
                return Math.Min(Math.Max(i, lowerBound), higherBound);
        }
    }
}
