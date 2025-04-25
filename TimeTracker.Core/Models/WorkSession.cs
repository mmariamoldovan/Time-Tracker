using System;

namespace TimeTracker.Core.Models
{
    
    public class WorkSession
    {
        
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        
     
        public DateTime? EndTime { get; set; }
    }
}