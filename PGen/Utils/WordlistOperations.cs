namespace PGen.Utils
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using System.Text;
    using System.Threading.Tasks;

    using Properties;

    public class WordlistOperations
    {
        const string ResourceName = "PGen.Resources.shitlist.txt";

        public static void CreateUniqueShitlist(ref HashSet<string> wordlist)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(ResourceName))
            {
                if (stream == null) return;
                using (var slrdr = new StreamReader(stream))
                {
                    string line;

                    do
                    {
                        line = slrdr.ReadLine();

                        if (line?.Count() > 3)
                            wordlist.Add(line);
                    }
                    while (line != null);
                }
            }
        }

        //public static void CompressedWordlistToHashset(ref HashSet<string> wordlist)
        //{
            
        //    var assembly = Assembly.GetExecutingAssembly();
        //    var decompressedList = CompressionHandler.DecompressBase64ToUnicode(Resources.shitlist);
        //    //File.WriteAllText("C:\\Users\\Herp\\Documents\\compressedList.txt", decompressedList);

        //    using (var stream = new StringReader(decompressedList))
        //    {
        //        var line = "";
        //        do
        //        {
        //            wordlist.Add(line);

        //            line = stream.ReadLine();
        //        }
        //        while (line != null);
        //    }
        //    int a = 0;
        //    int b = 1;
        //    int c = b / a;
        //}

        //public static List<HashSet<string>> SplitLargeList(ref HashSet<string> wordlist)
        //{
        //    var hashSetList = new List<HashSet<string>>();

        //    var counter = 0;

        //    var tempWordList = new List<string>();

        //    Parallel.ForEach(wordlist, word =>
        //        {
        //            if (counter < 10000)
        //            {
        //                tempWordList.Add(word);
        //                counter++;
        //            }
        //            else
        //            {
        //                hashSetList.Add(new HashSet<string>(tempWordList));
        //                tempWordList.Clear();
        //                counter = 0;
        //            }
        //        });

        //    return hashSetList;
        //}

        //public static void DumpPrunedZippedWordlist(HashSet<string> wordlist)
        //{
        //    var wordlistString = "";
        //    Parallel.ForEach(wordlist, word => wordlistString += $"{word}{Environment.NewLine}");
        //    var builder = new StringBuilder();
        //    builder.Append(CompressionHandler.CompressUnicodeToBase64(wordlistString));
        //    File.WriteAllText("C:\\Users\\Herp\\Documents\\compressedList.txt", builder.ToString());

        //}
    }
}