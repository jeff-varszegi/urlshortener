using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace UrlShortener.Common.Utility
{
    /// <summary>Provides utility logic for URL keys</summary>
    public static class UrlUtility
    {
        /// <summary>Characters to use in key conversions</summary>
        private const string CharacterSet = "0123456789abcdefghijklmnopqrstuvwxyz";

        /// <summary>Used in key conversions</summary>
        private static readonly long[] PowersOf36 = new long[10];

        /// <summary>Initializes this class</summary>
        static UrlUtility()
        {
            long number = 1;
            for (int x = 0; x < PowersOf36.Length; x++)
            {
                PowersOf36[x] = number;
                number *= 36;
            }
        }

        /// <summary>Converts the specified ID to a string</summary>
        /// <param name="id">The ID to convert</param>
        /// <returns>A string suitable for use as an external key</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ConvertIdToKey(long id)
        {
            StringBuilder sb = new StringBuilder(20);

            int numeral;
            long power;
            char c;
            for (int x = PowersOf36.Length - 1; x >= 0; x--)
            {
                power = PowersOf36[x];

                numeral = (int)(id / power);
                if (sb.Length > 0 || numeral > 0)
                    sb.Append(CharacterSet[numeral]);

                id -= power * numeral;
            }

            return sb.ToString();
        }

        /// <summary>Converts the specified string key to an integer ID</summary>
        /// <param name="key">The key to convert</param>
        /// <returns>An integer version of the specified key</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ConvertKeyToId(string key)
        {
            key = key.ToLower();
            int keyLength = key.Length;
            if (keyLength > 10) keyLength = 10;

            long id = 0L;
            int addition, power = 0;
            for (int x = keyLength - 1; x >= 0; x--)
            {
                switch (key[x])
                {
                    case '1': addition = 1; break;
                    case '2': addition = 2; break;
                    case '3': addition = 3; break;
                    case '4': addition = 4; break;
                    case '5': addition = 5; break;
                    case '6': addition = 6; break;
                    case '7': addition = 7; break;
                    case '8': addition = 8; break;
                    case '9': addition = 9; break;
                    case 'a': addition = 10; break;
                    case 'b': addition = 11; break;
                    case 'c': addition = 12; break;
                    case 'd': addition = 13; break;
                    case 'e': addition = 14; break;
                    case 'f': addition = 15; break;
                    case 'g': addition = 16; break;
                    case 'h': addition = 17; break;
                    case 'i': addition = 18; break;
                    case 'j': addition = 19; break;
                    case 'k': addition = 20; break;
                    case 'l': addition = 21; break;
                    case 'm': addition = 22; break;
                    case 'n': addition = 23; break;
                    case 'o': addition = 24; break;
                    case 'p': addition = 25; break;
                    case 'q': addition = 26; break;
                    case 'r': addition = 27; break;
                    case 's': addition = 28; break;
                    case 't': addition = 29; break;
                    case 'u': addition = 30; break;
                    case 'v': addition = 31; break;
                    case 'w': addition = 32; break;
                    case 'x': addition = 33; break;
                    case 'y': addition = 34; break;
                    case 'z': addition = 35; break;
                    default: addition = 0; break;
                }

                id += addition * PowersOf36[power];
                power++;
            }

            return id;
        }

        /// <summary>Tests a URL for validity</summary>
        /// <param name="url">The URL to test</param>
        /// <returns>true if the specified URL is valid, otherwise false</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsUrlValid(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
}
