using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTracker.Core.DTOs;

namespace TimeTracker.Client.Services
{
   
  
    public interface ITimeTrackerApiClient
    {
       
        Task<ApiResponse<int>> StartSessionAsync();
        
       
        Task<ApiResponse<bool>> StopActiveSessionAsync();
        
   
        Task<ApiResponse<bool>> StopSessionAsync(int sessionId);
        Task<ApiResponse<bool>> DeleteSessionAsync(int sessionId);
        
    
        Task<ApiResponse<WorkSessionDTO>> GetActiveSessionAsync();
        
        
        Task<ApiResponse<List<WorkSessionDTO>>> GetAllSessionsAsync();
        
   
        Task<ApiResponse<TimeReportDTO>> GetDailyReportAsync(DateTime? date = null);
        
      
        Task<ApiResponse<TimeReportDTO>> GetWeeklyReportAsync(DateTime? startDate = null);
        
     
        Task<ApiResponse<TimeReportDTO>> GetMonthlyReportAsync(int? year = null, int? month = null);
        
 
        Task<ApiResponse<List<DailyReportItemDTO>>> GetDailyBreakdownAsync(DateTime? startDate = null, DateTime? endDate = null);
    }
}