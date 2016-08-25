using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace DragNDropTxtWithUrlsHereToOpenInIETabs
{
    class Program
    {
        static void Main(string[] args)
        {
            // Print a status message and declare a collection and default delay time.
            IList<string> urlsToOpenInIETabs = new List<string>();
            int threadSleepDelayMS = 1500;
            Console.WriteLine("Reading .txt files...");

            if (args != null && args.Length > 0)
            {
                // Get urls from .txt files dragged and dropped on this .exe file.
                try
                {
                    foreach (string filePath in args)
                        if (!string.IsNullOrEmpty(filePath))
                        {
                            if (Path.GetExtension(filePath) == ".txt")
                            {
                                Console.WriteLine(string.Format("Reading file: {0}", filePath));
                                if (File.Exists(filePath))
                                {
                                    Console.WriteLine("File exists.");
                                    foreach (string url in File.ReadAllLines(filePath, Encoding.Default))
                                        if (!string.IsNullOrEmpty(url))
                                            urlsToOpenInIETabs.Add(url);
                                }
                                else
                                {
                                    Console.WriteLine("The file does not exist!");
                                }
                            }
                            else if (IsNumeric(filePath))
                                threadSleepDelayMS = Convert.ToInt32(filePath);
                        }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }

                // Open urls in IE with a short delay in between. 
                // Hopefully the urls will open in separate tabs in the same browser window.
                try
                {
                    if (urlsToOpenInIETabs != null && urlsToOpenInIETabs.Count > 0)
                    {
                        Console.WriteLine("Preparing to open urls in IE.");
                        Console.WriteLine(string.Format("Delay setting: {0} milliseconds.", threadSleepDelayMS));
                        Console.WriteLine(string.Format("Urls found: {0}", urlsToOpenInIETabs.Count));

                        foreach (string urlToOpenInIETab in urlsToOpenInIETabs)
                        {
                            Console.WriteLine(string.Format("Sleeping for {0} milliseconds...", threadSleepDelayMS));
                            Thread.Sleep(threadSleepDelayMS);
                            Console.WriteLine(string.Format("Opening url: {0}", urlToOpenInIETab));
                            Process.Start(string.Format("{0}", urlToOpenInIETab));
                        }
                    }
                    else
                    {
                        Console.WriteLine("Files were empty.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("No .txt filepath args passed to program.");
            }
        }

        private static bool IsNumeric(string filePath)
        {
            int temp;
            return int.TryParse(filePath, out temp);
        }
    }
}