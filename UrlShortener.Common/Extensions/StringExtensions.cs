namespace UrlShortener
{
    /// <summary>Implements utility/extension logic for strings</summary>
    public static partial class StringExtensions
    {
        /// <summary>Gets a persistent hash code that is stable across restarts</summary>
        /// <param name="s">this string</param>
        /// <returns>A stable hash code</returns>
        public static int GetStableHashCode(this string s)
        {
            // Original: https://stackoverflow.com/questions/36845430/persistent-hashcode-for-strings
            unchecked
            {
                int hash1 = 5381;
                int hash2 = hash1;

                for (int i = 0; i < s.Length && s[i] != '\0'; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ s[i];
                    if (i == s.Length - 1 || s[i + 1] == '\0')
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ s[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }
    }
}
