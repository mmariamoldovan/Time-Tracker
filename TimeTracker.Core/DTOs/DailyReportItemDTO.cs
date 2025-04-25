using System;

namespace TimeTracker.Core.DTOs
{
  
    public class DailyReportItemDTO
    {

        public DateTime Date { get; set; }
    
        public TimeSpan Duration { get; set; }
        
      
        public DayOfWeek DayOfWeek { get; set; }
    }
}