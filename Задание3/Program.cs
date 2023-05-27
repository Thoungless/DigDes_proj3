using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace WordCountClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string apiUrl = "https://localhost:44301/wordcount/post";

            try
            {
                string book = (@"http://az.lib.ru/t/tolstoj_lew_nikolaewich/text_0040.shtml");
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\book.txt";
                Dictionary<string, int> res = new Dictionary<string, int>();

                WebRequest req = WebRequest.Create(book);
                WebResponse resp = req.GetResponse();
                Stream stream = resp.GetResponseStream();
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                StreamReader sr = new StreamReader(stream, Encoding.GetEncoding("windows-1251"));
                string text = sr.ReadToEnd();


                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsJsonAsync(apiUrl, text);

                    if (response.IsSuccessStatusCode)
                    {
                        var wordCounts = await response.Content.ReadFromJsonAsync<Dictionary<string, int>>();
                        foreach (var item in wordCounts)
                        {
                            using (StreamWriter sw = new StreamWriter(path, true))
                            {
                                sw.WriteLine(item);
                            }
                        }

                    }
                    else
                    {
                        Console.WriteLine("Ошибка при вызове веб-сервиса: " + response.ReasonPhrase);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка: " + ex.Message);
            }

            Console.ReadLine();
        }
    }
}