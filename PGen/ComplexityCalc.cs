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
    }
}
