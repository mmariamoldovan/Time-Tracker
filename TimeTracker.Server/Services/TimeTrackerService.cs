using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTracker.Core.DTOs;
using TimeTracker.Core.Models;

namespace TimeTracker.Server.Services
{
    public class TimeTrackerService : ITimeTrackerService
    {
        private readonly IWorkSessionRepository _repository;

        
    public TimeTrackerService(IWorkSessionRepository repository)
        {
     _repository = repository;
        }
        public async Task<int> StartSessionAsync()
        {
            var activeSession = await _repository.GetActiveSessionAsync();
            if (activeSession != null)
            return activeSession.Id;

            return await _repository.StartNewSessionAsync();
        }

        public async Task<bool> StopActiveSessionAsync()
        {
            var activeSession = await _repository.GetActiveSessionAsync();
            
            if (activeSession == null)
                return false;
                
            return await _repository.StopSessionAsync(activeSession.Id);
        }

        
        public async Task<bool> StopSessionAsync(int sessionId)
        {
      return await _repository.StopSessionAsync(sessionId);
        }
        
      
        public async Task<bool> DeleteSessionAsync(int sessionId)
        {
            return await _repository.DeleteSessionAsync(sessionId);
        }

              public async Task<WorkSessionDTO> GetActiveSessionAsync()
        {
            var session = await _repository.GetActiveSessionAsync();
            
            if (session == null)
                return null;
                
            return ConvertToDto(session);
        }

public async Task<List<WorkSessionDTO>> GetAllSessionsAsync()
        {
            var sessions = await _repository.GetAllSessionsAsync();
            return sessions.Select(ConvertToDto).ToList();
        }
        public async Task<TimeReportDTO> GetDailyReportAsync(DateTime date)
        {
            var startDate = date.Date;
            var endDate = date.Date;
            
            var sessions = await _repository.GetSessionsInDateRangeAsync(startDate, endDate);
            
            var report = new TimeReportDTO
            {
                StartDate = startDate,
                EndDate = endDate,
                Sessions = sessions.Select(ConvertToDto).ToList(),
                TotalDuration = CalculateTotalDuration(sessions)
            };
            
            return report;
        }

        
        public async Task<TimeReportDTO> GetWeeklyReportAsync(DateTime weekStartDate)
        {
            var startDate = weekStartDate.Date;
            var endDate = startDate.AddDays(6);
            
            var sessions = await _repository.GetSessionsInDateRangeAsync(startDate, endDate);
            
            var report = new TimeReportDTO
            {
                StartDate = startDate,
                EndDate = endDate,
                Sessions = sessions.Select(ConvertToDto).ToList(),
                TotalDuration = CalculateTotalDuration(sessions)
            };
            
            return report;
        }

        public async Task<TimeReportDTO> GetMonthlyReportAsync(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            
            var sessions = await _repository.GetSessionsInDateRangeAsync(startDate, endDate);
            
            var report = new TimeReportDTO
            {
                StartDate = startDate,
                EndDate = endDate,
                Sessions = sessions.Select(ConvertToDto).ToList(),
                TotalDuration = CalculateTotalDuration(sessions)
            };
            
            return report;
        }
        
       
        public async Task<List<DailyReportItemDTO>> GetDailyBreakdownAsync(DateTime startDate, DateTime endDate)
        {
            var result = new List<DailyReportItemDTO>();
            var currentDate = startDate.Date;
            
            while (currentDate <= endDate.Date)
            {
                var dailyReport = await GetDailyReportAsync(currentDate);
                
                result.Add(new DailyReportItemDTO
                {
                    Date = currentDate,
                    Duration = dailyReport.TotalDuration,
                    DayOfWeek = currentDate.DayOfWeek
                });
                
                currentDate = currentDate.AddDays(1);
            }
            
            return result;
        }

      
        private WorkSessionDTO ConvertToDto(WorkSession session)
        {
            return new WorkSessionDTO
            {
                Id = session.Id,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                Duration = CalculateDuration(session)
            };
        }

        private TimeSpan CalculateDuration(WorkSession session)
        {
            return session.EndTime.HasValue
                ? session.EndTime.Value - session.StartTime
                : DateTime.Now - session.StartTime;
        }

        
        private TimeSpan CalculateTotalDuration(List<WorkSession> sessions)
        {
            var totalDuration = TimeSpan.Zero;
            
            foreach (var session in sessions)
            {
                totalDuration += CalculateDuration(session);
            }
            
            return totalDuration;
        }
    }
}