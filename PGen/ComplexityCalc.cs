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
