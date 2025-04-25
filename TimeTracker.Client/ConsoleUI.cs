using System;
using System.Linq;
using System.Threading.Tasks;
using TimeTracker.Client.Services;
using TimeTracker.Core.DTOs;

namespace TimeTracker.Client
{

    public class ConsoleUI
    {
        private readonly ITimeTrackerApiClient _apiClient;
        
     
        public ConsoleUI(ITimeTrackerApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        
        public async Task RunAsync()
        {
         Console.WriteLine("=== Work Time Tracker Client ===");
            Console.WriteLine("Connecting to TimeTracker server...");
            
            
            try {
                var response = await _apiClient.GetAllSessionsAsync();
                if (!response.Success)
                {
                    Console.WriteLine($"Warning: {response.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to server: {ex.Message}");
                Console.WriteLine("Please make sure the server is running and try again.");
                return;
            }
            
            Console.WriteLine("Connected to TimeTracker server successfully.");
            bool exit = false;
            while (!exit)
            {
                await DisplayMenuAsync();
             var choice = GetUserChoice();
                switch (choice)
                {
                    case 1:
                        await StartSessionAsync();
                        break;
                    case 2:
                        await StopSessionAsync();
                        break;
                    case 3:
                        await ShowDailyReportAsync();
                        break;
                    case 4:
                        await ShowWeeklyReportAsync();
                        break;
                    case 5:
                        await ShowMonthlyReportAsync();
                        break;
                    case 6:
                        await ShowAllSessionsAsync();
                        break;
                    case 0:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
                
                if (!exit)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            
            Console.WriteLine("Thank you for using Work Time Tracker!");
        }
        
   
        private async Task DisplayMenuAsync()
        {
            await DisplayActiveSessionAsync();
            
            Console.WriteLine("\nSelect an option:");
            Console.WriteLine("1. Start a new work session");
            Console.WriteLine("2. Stop current work session");
            Console.WriteLine("3. View daily report");
            Console.WriteLine("4. View weekly report");
            Console.WriteLine("5. View monthly report");
            Console.WriteLine("6. View all sessions");
            Console.WriteLine("0. Exit");
            Console.Write("\nEnter your choice: ");
        }
        
    
        private async Task DisplayActiveSessionAsync()
        {
            var response = await _apiClient.GetActiveSessionAsync();
            
            if (response.Success && response.Data != null)
            {
                var session = response.Data;
                Console.WriteLine("\nCurrent session:");
                Console.WriteLine($"Started at: {session.StartTime}");
                Console.WriteLine($"Duration: {FormatTimeSpan(session.Duration)}");
            }
            else
            {
                Console.WriteLine("\nNo active work session.");
            }
        }
        
   
        private int GetUserChoice()
        {
            var input = Console.ReadLine();
            
            if (int.TryParse(input, out int choice))
            {
                return choice;
            }
            
            return -1;
        }
        
      
        private async Task StartSessionAsync()
        {
            Console.WriteLine("\nStarting new session...");
            
            var response = await _apiClient.StartSessionAsync();
            
            if (response.Success)
            {
                Console.WriteLine($"Started new work session at {DateTime.Now} (ID: {response.Data})");
            }
            else
            {
                Console.WriteLine($"Error: {response.Message}");
            }
        }
        
      
        private async Task StopSessionAsync()
        {
            Console.WriteLine("\nStopping active session...");
            
            var response = await _apiClient.StopActiveSessionAsync();
            
            if (response.Success)
            {
                Console.WriteLine("Work session stopped successfully.");
                
                var activeSessionResponse = await _apiClient.GetActiveSessionAsync();
                if (!activeSessionResponse.Success || activeSessionResponse.Data == null)
                {
                 
                    var allSessionsResponse = await _apiClient.GetAllSessionsAsync();
                    if (allSessionsResponse.Success && allSessionsResponse.Data.Any())
                    {
                        var lastSession = allSessionsResponse.Data.First();
                        Console.WriteLine($"Session duration: {FormatTimeSpan(lastSession.Duration)}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.Message}");
            }
        }
     
        private async Task ShowDailyReportAsync()
        {
            Console.Write("\nEnter date (yyyy-MM-dd), or press Enter for today: ");
            var input = Console.ReadLine();
            
            DateTime? date = null;
            if (!string.IsNullOrWhiteSpace(input) && DateTime.TryParse(input, out DateTime parsedDate))
            {
                date = parsedDate.Date;
            }
            
            Console.WriteLine("\nFetching daily report...");
            
            var response = await _apiClient.GetDailyReportAsync(date);
            
            if (response.Success)
            {
                var report = response.Data;
                
                Console.WriteLine($"\nDaily Report for {report.StartDate:yyyy-MM-dd}");
                Console.WriteLine(new string('-', 70));
                Console.WriteLine("| {0,-5} | {1,-20} | {2,-20} | {3,-15} |", "ID", "Start Time", "End Time", "Duration");
                Console.WriteLine(new string('-', 70));
                
                foreach (var session in report.Sessions)
                {
                    Console.WriteLine("| {0,-5} | {1,-20} | {2,-20} | {3,-15} |", 
                        session.Id, 
                        session.StartTime.ToString("yyyy-MM-dd HH:mm"), 
                        session.EndTime?.ToString("yyyy-MM-dd HH:mm") ?? "Running...", 
                        FormatTimeSpan(session.Duration));
                }
                
                Console.WriteLine(new string('-', 70));
                Console.WriteLine($"Total duration: {FormatTimeSpan(report.TotalDuration)}");
            }
            else
            {
                Console.WriteLine($"Error: {response.Message}");
            }
        }
        
       
        private async Task ShowWeeklyReportAsync()
        {
            Console.Write("\nEnter start date of week (yyyy-MM-dd), or press Enter for this week: ");
            var input = Console.ReadLine();
            
            DateTime? startDate = null;
            if (!string.IsNullOrWhiteSpace(input) && DateTime.TryParse(input, out DateTime parsedDate))
            {
                startDate = parsedDate.Date;
            }
            
            Console.WriteLine("\nFetching weekly report...");
            
            var breakdownResponse = await _apiClient.GetDailyBreakdownAsync(
                startDate ?? DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek),
                (startDate ?? DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek)).AddDays(6));
                
            var reportResponse = await _apiClient.GetWeeklyReportAsync(startDate);
            
            if (breakdownResponse.Success && reportResponse.Success)
            {
                var breakdown = breakdownResponse.Data;
                var report = reportResponse.Data;
                
                Console.WriteLine($"\nWeekly Report from {report.StartDate:yyyy-MM-dd} to {report.EndDate:yyyy-MM-dd}");
                Console.WriteLine(new string('-', 50));
                Console.WriteLine("| {0,-12} | {1,-15} | {2,-15} |", "Date", "Hours Worked", "");
                Console.WriteLine(new string('-', 50));
                
                foreach (var day in breakdown)
                {
                    Console.WriteLine("| {0,-12} | {1,-15} | {2,-15} |", 
                        day.Date.ToString("yyyy-MM-dd"), 
                        FormatTimeSpan(day.Duration),
                        day.DayOfWeek);
                }
                
                Console.WriteLine(new string('-', 50));
                Console.WriteLine($"Total for week: {FormatTimeSpan(report.TotalDuration)}");
            }
            else
            {
                Console.WriteLine($"Error: {(breakdownResponse.Success ? reportResponse.Message : breakdownResponse.Message)}");
            }
        }
        
      
        private async Task ShowMonthlyReportAsync()
        {
            Console.Write("\nEnter month (yyyy-MM), or press Enter for current month: ");
            var input = Console.ReadLine();
            
            int? year = null;
            int? month = null;
            
            if (!string.IsNullOrWhiteSpace(input))
            {
                var parts = input.Split('-');
                if (parts.Length == 2 && int.TryParse(parts[0], out int y) && int.TryParse(parts[1], out int m))
                {
                    year = y;
                    month = m;
                }
            }
            
            Console.WriteLine("\nFetching monthly report...");
            
            var response = await _apiClient.GetMonthlyReportAsync(year, month);
            
            if (response.Success)
            {
                var report = response.Data;
                
                Console.WriteLine($"\nMonthly Report for {report.StartDate:yyyy-MM}");
                Console.WriteLine(new string('-', 40));
                Console.WriteLine($"Total hours worked: {FormatTimeSpan(report.TotalDuration)}");
                Console.WriteLine(new string('-', 40));
                
               
                var workDays = report.Sessions
                    .Select(s => s.StartTime.Date)
                    .Distinct()
                    .Count(d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday);
                
                if (workDays > 0)
                {
                    var dailyAverage = TimeSpan.FromTicks(report.TotalDuration.Ticks / workDays);
                    Console.WriteLine($"Working days: {workDays}");
                    Console.WriteLine($"Daily average: {FormatTimeSpan(dailyAverage)}");
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.Message}");
            }
        }
        
      
        private async Task ShowAllSessionsAsync()
        {
            Console.WriteLine("\nFetching all sessions...");
            
            var response = await _apiClient.GetAllSessionsAsync();
            
            if (response.Success)
            {
                var sessions = response.Data;
                
                Console.WriteLine("\nAll Work Sessions");
                Console.WriteLine(new string('-', 70));
                Console.WriteLine("| {0,-5} | {1,-20} | {2,-20} | {3,-15} |", "ID", "Start Time", "End Time", "Duration");
                Console.WriteLine(new string('-', 70));
                
                foreach (var session in sessions)
                {
                    Console.WriteLine("| {0,-5} | {1,-20} | {2,-20} | {3,-15} |", 
                        session.Id, 
                        session.StartTime.ToString("yyyy-MM-dd HH:mm"), 
                        session.EndTime?.ToString("yyyy-MM-dd HH:mm") ?? "Running...", 
                        FormatTimeSpan(session.Duration));
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.Message}");
            }
        }
        
        
        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            return $"{(int)timeSpan.TotalHours}h {timeSpan.Minutes}m";
        }
    }
}