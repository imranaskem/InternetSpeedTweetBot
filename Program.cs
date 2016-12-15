using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Threading;
using Tweetinvi;

namespace InternetSpeedTweetBot
{
    class Program
    {
        static TimeSpan timeBetweenTests = new TimeSpan(0, 30, 0);

        static void Main(string[] args)
        {            
            while (true)
            {                
                MeasureSpeed();

                Thread.Sleep(timeBetweenTests);
            }
        }

        private static void MeasureSpeed()
        {
            const string tempfile = "tempfile.tmp";
            const double threshold = 100;            

            Console.WriteLine("Downloading file...");
            Console.WriteLine();

            Stopwatch sw = new Stopwatch();

            using (WebClient webClient = new WebClient())
            {
                sw.Start(); 
                webClient.DownloadFile("https://dl.dropboxusercontent.com/u/2517348/testfile.tmp", tempfile);
                sw.Stop();
            }

            FileInfo fileInfo = new FileInfo(tempfile);
            double fileSize = fileInfo.Length;
            fileSize = Math.Round((fileSize / 1024 / 1024), 2);
            var speed = Math.Round(fileSize / sw.Elapsed.TotalSeconds, 2);

            Console.WriteLine($"Download duration: {sw.Elapsed}");
            Console.WriteLine($"File size: {fileSize} MB");
            Console.WriteLine($"Speed: {speed} MB/s ");

            var internetSpeed = speed * 8;
            Console.WriteLine($"Internet Speed: {internetSpeed} Mbps ");
            Console.WriteLine();

            if (internetSpeed < threshold)
            {
                Console.WriteLine("Current speed is below threshold, sending tweet...");
                var tweetHappened = TweetISP(internetSpeed);

                if (tweetHappened)
                {
                    Console.WriteLine("Tweet sent!");
                }
                else
                {
                    Console.WriteLine("Something went wrong, the tweet wasn't sent");
                }
            } 
            else
            {
                Console.WriteLine("Speed is above threshold");
            }

            Console.WriteLine();
            Console.WriteLine($"Next test is in {timeBetweenTests.Minutes} minutes");
        }

        private static bool TweetISP(double speed)
        {
            var consumerKey = "7yVIvWbiabDBYn133iF5xiLeS";
            var consumerSecretKey = "n0qLRdDLZrLZZQqBs6qLFSw9HOPQ0G8zhEDOFFw1vdaZriKq0A";
            var accessToken = "809384501570838528-yhSq3uu7SL3oYQb3CeXSz2J1ebLgT7t";
            var accessSecretToken = "YlsIUe49BtA80kUA07DWpqFkEMvUCdsP4vZXg5OBMNP09";

            Auth.SetUserCredentials(consumerKey, consumerSecretKey, accessToken, accessSecretToken);

            var tweetMessage = $"@virginmedia My current download speed is {speed}Mbps but I'm paying for 200Mbps!";            

            ExceptionHandler.SwallowWebExceptions = false;

            var tweet = Tweet.PublishTweet(tweetMessage);

            return tweet.IsTweetPublished;
        }
    }
}
