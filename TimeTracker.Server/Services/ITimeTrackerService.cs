using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTracker.Core.DTOs;

namespace TimeTracker.Server.Services
{
  
    public interface ITimeTrackerService
    {
       
        Task<int> StartSessionAsync();
        
   
        Task<bool> StopActiveSessionAsync();
      
        Task<WorkSessionDTO> GetActiveSessionAsync();
        
      
        Task<bool> StopSessionAsync(int sessionId);
        
      
        Task<bool> DeleteSessionAsync(int sessionId);
        Task<List<WorkSessionDTO>> GetAllSessionsAsync();
        

        Task<TimeReportDTO> GetDailyReportAsync(DateTime date);
        
        Task<TimeReportDTO> GetWeeklyReportAsync(DateTime weekStartDate);
        
     
        Task<TimeReportDTO> GetMonthlyReportAsync(int year, int month);
        
        
        Task<List<DailyReportItemDTO>> GetDailyBreakdownAsync(DateTime startDate, DateTime endDate);
    }
}