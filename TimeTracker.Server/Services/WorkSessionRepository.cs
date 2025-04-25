using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Core.Models;
using TimeTracker.Server.Data;

namespace TimeTracker.Server.Services
{
    public class WorkSessionRepository : IWorkSessionRepository
    {
        private readonly TimeTrackerDbContext _context;
        public WorkSessionRepository(TimeTrackerDbContext context)
        {
    _context = context;
        }

       
        public async Task<WorkSession> GetActiveSessionAsync()
        {
            return await _context.WorkSessions
                .Where(s => s.EndTime == null)
            .OrderByDescending(s => s.Id)
                .FirstOrDefaultAsync();
        }

    
        public async Task<WorkSession> GetSessionByIdAsync(int id)
        {
         return await _context.WorkSessions.FindAsync(id);
        }

        
        public async Task<List<WorkSession>> GetAllSessionsAsync()
        {
            return await _context.WorkSessions
                .OrderByDescending(s => s.StartTime)
                .ToListAsync();
        }
        public async Task<List<WorkSession>> GetSessionsInDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            
            DateTime inclusiveEndDate = endDate.AddDays(1);
            return await _context.WorkSessions
             .Where(s => s.StartTime >= startDate && s.StartTime < inclusiveEndDate)
                .OrderByDescending(s => s.StartTime)
                .ToListAsync();
        }

      
        public async Task<int> StartNewSessionAsync()
        {
            var session = new WorkSession
            {
                StartTime = DateTime.Now,
                EndTime = null};

            _context.WorkSessions.Add(session);
            await _context.SaveChangesAsync();
            
            return session.Id;
        }

        public async Task<bool> StopSessionAsync(int sessionId)
        {
            var session = await _context.WorkSessions.FindAsync(sessionId);
            
            if (session == null || session.EndTime.HasValue)
            {
                return false;
            }

            session.EndTime = DateTime.Now;
            await _context.SaveChangesAsync();
            
            return true;
        }
        public async Task<bool> DeleteSessionAsync(int sessionId)
        {
            var session = await _context.WorkSessions.FindAsync(sessionId);
            
            if (session == null)
            {
                return false;
            }

            _context.WorkSessions.Remove(session);
            await _context.SaveChangesAsync();
            
            return true;
        }
    }
}