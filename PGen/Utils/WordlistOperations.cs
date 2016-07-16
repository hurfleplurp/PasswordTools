namespace PGen.Utils
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public class WordlistOperations
    {
        public static void CreateUniqueShitlist(ref HashSet<string> wordlist)
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string ResourceName = "PGen.Resources.shitlist.txt";

            using (var stream = assembly.GetManifestResourceStream(ResourceName))
            {
                if (stream == null) return;
                using (var slrdr = new StreamReader(stream))
                {
                    var line = "";

                    do
                    {
                        if (line.Count() > 3)
                            wordlist.Add(line);

                        line = slrdr.ReadLine();
                    }
                    while (line != null);
                }
            }
        }

        public static List<HashSet<string>> SplitLargeList(ref HashSet<string> wordlist)
        {
            var hashSetList = new List<HashSet<string>>();

            int counter = 0;

            var tempWordList = new List<string>();

            Parallel.ForEach(wordlist, word =>
                {
                    if (counter < 10000)
                    {
                        tempWordList.Add(word);
                        counter++;
                    }
                    else
                    {
                        hashSetList.Add(new HashSet<string>(tempWordList));
                        tempWordList.Clear();
                        counter = 0;
                    }
                });

            return hashSetList;
        }
    }
}