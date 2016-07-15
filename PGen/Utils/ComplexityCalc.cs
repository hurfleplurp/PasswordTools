using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PGen.Utils
{
    public static class ComplexityCalc
    {
        public static HashSet<string> Wordlist = new HashSet<string>();

        public static void CreateUniqueShitlist()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "PGen.Resources.shitlist.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader slrdr = new StreamReader(stream))
            {
                string line = "";

                do
                {
                    if (line.Count() > 3)
                        Wordlist.Add(line);

                    line = slrdr.ReadLine();
                }
                while (line != null);
            }
        }

        public static List<HashSet<string>> SplitLargeList()
        {
            var HashSetList = new List<HashSet<string>>();

            int counter = 0;

            var tempWordList = new List<string>();

            foreach (var word in Wordlist)
            {
                if (counter < 10000)
                {
                    tempWordList.Add(word);
                    counter++;
                }
                else
                {
                    HashSetList.Add(new HashSet<string>(tempWordList));
                    tempWordList.Clear();
                    counter = 0;
                }
            }
            
            return HashSetList;
        }


        public static HashSet<string> GetConsecutiveSubstrings(string password)
        {
            var substrings = new HashSet<string>();
            char[] letters = password.ToCharArray();
            for (int size = 1; size < letters.Length; ++size)
            {
                // get substrings of 'size' words
                for (int start = 1; start <= letters.Length - size; ++start)
                {
                    char[] before = new char[start];
                    char[] destination = new char[size];
                    char[] after = new char[letters.Length - (size + start)];
                                        
                    Array.Copy(letters, 0, before, 0, before.Length);
                    Array.Copy(letters, start, destination, 0, destination.Length);
                    Array.Copy(letters, start + destination.Length, after, 0, after.Length);

                    if (before.Length > 2)
                        substrings.Add(string.Join("", before));
                    if (destination.Length > 2)
                        substrings.Add(string.Join("", destination));
                    if (after.Length > 2)
                        substrings.Add(string.Join("", after));
                }
            }
            return substrings;
        }

        // Z-Algoritm, matches pattern in target in O(m) time
        public static bool zAlgorithm(string pattern, string target)
        {
            string s = pattern + '$' + target;
            int n = s.Length;
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
