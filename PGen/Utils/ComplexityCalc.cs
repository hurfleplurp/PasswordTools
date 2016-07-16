using System;
using System.Collections.Generic;

namespace PGen.Utils
{
    public static partial class ComplexityCalc
    {

        public static HashSet<string> GetConsecutiveSubstrings(string password)
        {
            var substrings = new HashSet<string>();
            var letters = password.ToCharArray();
            for (var size = 1; size < letters.Length; ++size)
            {
                // get substrings of 'size' words
                for (var start = 1; start <= letters.Length - size; ++start)
                {
                    var before = new char[start];
                    var destination = new char[size];
                    var after = new char[letters.Length - (size + start)];
                                        
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

    }
}
