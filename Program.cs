using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using Tweetinvi;

namespace InternetSpeedTweetBot
{
    class Program
    {
        static void Main(string[] args)
        {
            const string tempfile = "tempfile.tmp";
            WebClient webClient = new WebClient();

            Console.WriteLine("Downloading file....");

            Stopwatch sw = Stopwatch.StartNew();
            webClient.DownloadFile("http://dl.google.com/googletalk/googletalk-setup.exe", tempfile);
            sw.Stop();

            FileInfo fileInfo = new FileInfo(tempfile);
            long speed = fileInfo.Length / sw.Elapsed.Seconds;

            Console.WriteLine("Download duration: {0}", sw.Elapsed);
            Console.WriteLine("File size: {0}", fileInfo.Length.ToString("N0"));
            Console.WriteLine("Speed: {0} bps ", speed.ToString("N0"));

            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }
    }
}
