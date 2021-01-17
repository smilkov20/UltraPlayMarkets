using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Timers;
using UltraPlayMarkets.Utilities;

namespace UltraPlayMarkets
{
    public class Program
    {

        private static readonly Timer _dataRefreshTimer = new Timer(60000); // 60 sec

        public static void Main(string[] args)
        {
            MarketsUpdate();
            _dataRefreshTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            _dataRefreshTimer.Start();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            MarketsUpdate();
        }
        private static void MarketsUpdate()
        {
            XMLReader reader = new XMLReader();
            reader.Read();
        }
    }
}
