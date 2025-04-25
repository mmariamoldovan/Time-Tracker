using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTracker.Core.Models;

namespace TimeTracker.Server.Services
{
    public interface IWorkSessionRepository
    {
        Task<WorkSession> GetActiveSessionAsync();
        
          Task<WorkSession> GetSessionByIdAsync(int id);
       
        Task<List<WorkSession>> GetAllSessionsAsync();
        
      
        Task<List<WorkSession>> GetSessionsInDateRangeAsync(DateTime startDate, DateTime endDate);
        
        
        Task<int> StartNewSessionAsync();
        Task<bool> StopSessionAsync(int sessionId);
        
     
        Task<bool> DeleteSessionAsync(int sessionId);
    }
}