using System;

namespace TimeTracker.Core.DTOs
{
  
    public class WorkSessionDTO
    {
        
        public int Id { get; set; }
        
       
        public DateTime StartTime { get; set; }
        
       
        public DateTime? EndTime { get; set; }
      
        public TimeSpan Duration { get; set; }
    }
}