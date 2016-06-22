using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STRING_Matching;

namespace PGen
{
    public static class ComplexityCalc
    {
        static string shitlist = @"..\..\Resources\shitlist.txt";

        public static HashSet<string> wordlist = new HashSet<string>();

        public static void CreateUniqueShitlist()
        {
            using (StreamReader slrdr = new StreamReader(shitlist))
            {
                string line = "";

                do
                {
                    wordlist.Add(line);
                    line = slrdr.ReadLine();
                }
                while (line != null);
            }
        }

        public static List<HashSet<string>> SplitLargeList()
        {
            var HashSetList = new List<HashSet<string>>();

            int counter = 0;
            int i = 0;
            var tempWordList = new List<string>();

            foreach (var word in wordlist)
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
            //tempHashSet.Clear();
            return HashSetList;
        }


        public static HashSet<string> GetConsecutiveSubstrings(string password)
        {
            var substrings = new HashSet<string>();
            char[] letters = password.ToCharArray();
            for (int size = 1; size < letters.Length; ++size)
            {
                // get substrings of 'size' words
                for (int start = 0; start <= letters.Length - size; ++start)
                {
                    char[] before = new char[start];
                    char[] destination = new char[size];
                    char[] after = new char[letters.Length - (size + start)];
                    Array.Copy(letters, 0, before, 0, before.Length);
                    Array.Copy(letters, start, destination, 0, destination.Length);
                    Array.Copy(letters, start + destination.Length, after, 0, after.Length);

                    substrings.Add(string.Join("",before));
                    substrings.Add(string.Join("", destination));
                    substrings.Add(string.Join("", after));
                }
            }
            return substrings;
        }

        public static void ShitlistCheck()
        {
            Console.WriteLine("=======================direct Manner===============");

            Exact_Matching S = new Exact_Matching();
            string pattern = "abcdaaabchoabcd";
            char[] Stest = pattern.ToCharArray();
            char[] Stest1;
            int z = 0;


            /// Direct test using the Z-block function 
            int[] M = S.Zblock(Stest);

            for (int i = 0; i < M.Length; i++)
            {
                Console.WriteLine(M[i].ToString());
            }
            Console.WriteLine("=======================Indirect Manner===============");
            /// Indirect test using the Zcalculate function
            Console.WriteLine("the first z value = 0");
            for (int i = 1; i < Stest.Length; i++)
            {
                Stest1 = S.Ssplit(i, Stest);
                z = S.Zcalculate(Stest, Stest1);
                Console.WriteLine(z);
            }
        }
    }
}
