using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Text;

namespace WordCounter
{
    public class WordCounter
    {
        char[] delimiters = { ' ', '.', ',', ';', '!', '?' };

        string[] words;

        private Dictionary<string, int> CountWords(string text)
        {
            StringBuilder sb = new StringBuilder(text);
            string l = "<dd>&nbsp;&nbsp;";
            sb.Replace(sb.ToString(), sb.ToString().Substring(text.IndexOf(l) + l.Length));

            sb.Replace(sb.ToString(), Regex.Replace(sb.ToString(), "<.*?>", String.Empty));
            sb.Replace(sb.ToString(), Regex.Replace(sb.ToString(), @"\r", String.Empty));
            sb.Replace(sb.ToString(), Regex.Replace(sb.ToString(), @"\n", String.Empty));
            sb.Replace(sb.ToString(), Regex.Replace(sb.ToString(), @"&nbsp;", String.Empty));

            Dictionary<string, int> wordCounts = new Dictionary<string, int>();


            words = sb.ToString().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);


            foreach (string word in words)
            {
                string lowercaseWord = word.ToLower();

                if (wordCounts.ContainsKey(lowercaseWord))
                {
                    wordCounts[lowercaseWord]++;
                }
                else
                {
                    wordCounts[lowercaseWord] = 1;
                }
            }

            var wordCountsDesc = wordCounts.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            return wordCountsDesc;
       
        }

        public Dictionary<string, int> CountWordsMultiThreaded(string text)
        {
            StringBuilder sb = new StringBuilder(text);

            string l = "<dd>&nbsp;&nbsp;";
            sb.Replace(sb.ToString(), sb.ToString().Substring(text.IndexOf(l) + l.Length));

            sb.Replace(sb.ToString(), Regex.Replace(sb.ToString(), "<.*?>", String.Empty));
            sb.Replace(sb.ToString(), Regex.Replace(sb.ToString(), @"\r", String.Empty));
            sb.Replace(sb.ToString(), Regex.Replace(sb.ToString(), @"\n", String.Empty));
            sb.Replace(sb.ToString(), Regex.Replace(sb.ToString(), @"&nbsp;", String.Empty));

            char[] delimiters = { ' ', '.', ',', ';', '!', '?' };

            words = sb.ToString().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            ConcurrentDictionary<string, int> parallelWordCounts = new ConcurrentDictionary<string, int>();

            Parallel.ForEach(words, word =>
            {
                string lowercaseWord = word.ToLower();
                parallelWordCounts.AddOrUpdate(lowercaseWord, 1, (_, count) => count + 1);
            });

            Dictionary<string, int> wordCountsDesc = parallelWordCounts.OrderByDescending(x => x.Value)
                .ToDictionary(x => x.Key, x => x.Value);

            return wordCountsDesc;
        }

    }
}
