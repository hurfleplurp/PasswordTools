namespace PGen.Utils
{
    using System.Collections.Generic;

    public partial class Matching
    {
        // Z-Algoritm, matches pattern in target in O(m) time
        public static bool zAlgorithm(string pattern, string target)
        {
            var s = pattern + '$' + target;
            var n = s.Length;
            var z = new List<int>(new int[n]);

            int goal = pattern.Length;
            int r = 0, l = 0, i;
            for (int k = 1; k < n; k++)
            {
                if (k > r)
                {
                    for (i = k; i < n && s[i] == s[i - k]; i++) ;
                    if (i > k)
                    {
                        z[k] = i - k;
                        l = k;
                        r = i - 1;
                    }
                }
                else
                {
                    int kt = k - l, b = r - k + 1;
                    if (z[kt] > b)
                    {
                        for (i = r + 1; i < n && s[i] == s[i - k]; i++) ;
                        z[k] = i - k;
                        l = k;
                        r = i - 1;
                    }
                }
                if ((z[k] == goal))
                    return true;
            }
            return false;
        }
    }
}