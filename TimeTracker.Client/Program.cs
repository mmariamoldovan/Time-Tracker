using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TimeTracker.Client.Services;

namespace TimeTracker.Client
{
 
    class Program
    {
        
        private const string ApiBaseUrl = "http://localhost:5033";
        
      
        static async Task Main(string[] args)
        {
            Console.WriteLine("TimeTracker Client starting...");
            Console.WriteLine($"API Server URL: {ApiBaseUrl}");
            
            try
            {
                var host = CreateHostBuilder(args).Build();
                
              
                if (args.Length > 0 && args[0] == "generate-reports")
                {
                    await GenerateReportsAsync(host.Services);
                }
                else
                {
                  
                    var consoleUI = host.Services.GetRequiredService<ConsoleUI>();
                    await consoleUI.RunAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting application: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
        
      
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    
                    services.AddHttpClient();
                services.AddTransient<HttpClientHandler>(sp => 
                    {
                        return new HttpClientHandler
                        {
                           
                            ServerCertificateCustomValidationCallback = 
                                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                        };
                    });
                    
                  
                    services.AddSingleton<ITimeTrackerApiClient>(sp =>
                    {
                        var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                        var handler = sp.GetRequiredService<HttpClientHandler>();
                        var httpClient = httpClientFactory.CreateClient();
                        httpClient.BaseAddress = new Uri(ApiBaseUrl);
                        
                        return new TimeTrackerApiClient(httpClient, ApiBaseUrl);
                    });
                    
                  
                    services.AddSingleton<ConsoleUI>(); });
        private static async Task GenerateReportsAsync(IServiceProvider services)
        {
            Console.WriteLine("Generating reports in batch mode...");
            
            var apiClient = services.GetRequiredService<ITimeTrackerApiClient>();
            var dailyReport = await apiClient.GetDailyReportAsync(DateTime.Today);
            if (dailyReport.Success)
            {
                Console.WriteLine($"Daily report for {DateTime.Today:yyyy-MM-dd} generated successfully.");
                Console.WriteLine($"Total hours worked today: {FormatTimeSpan(dailyReport.Data.TotalDuration)}");
            }
            else
            {
                Console.WriteLine($"Error generating daily report: {dailyReport.Message}");
            }
            
          
            var weekStartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            var weeklyReport = await apiClient.GetWeeklyReportAsync(weekStartDate);
            if (weeklyReport.Success) {
                Console.WriteLine($"\nWeekly report for {weekStartDate:yyyy-MM-dd} to {weekStartDate.AddDays(6):yyyy-MM-dd} generated successfully.");
            Console.WriteLine($"Total hours worked this week: {FormatTimeSpan(weeklyReport.Data.TotalDuration)}");
            }
            else
            {
                Console.WriteLine($"Error generating weekly report: {weeklyReport.Message}");
            }
            
          
            var monthlyReport = await apiClient.GetMonthlyReportAsync(DateTime.Today.Year, DateTime.Today.Month);
            if (monthlyReport.Success)
            {
                Console.WriteLine($"\nMonthly report for {DateTime.Today:yyyy-MM} generated successfully.");
                Console.WriteLine($"Total hours worked this month: {FormatTimeSpan(monthlyReport.Data.TotalDuration)}");
            }
            else{
            Console.WriteLine($"Error generating monthly report: {monthlyReport.Message}");
            }
            
         Console.WriteLine("\nReport generation completed.");
        }
        
     
        private static string FormatTimeSpan(TimeSpan timeSpan)
        {return $"{(int)timeSpan.TotalHours}h {timeSpan.Minutes}m";
        }
    }
}