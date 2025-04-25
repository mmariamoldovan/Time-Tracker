using System;
using System.Collections.Generic;

namespace TimeTracker.Core.DTOs
{
    public class TimeReportDTO
    {
    
        public DateTime StartDate { get; set; }
        
    
        public DateTime EndDate { get; set; }
        
     
        public TimeSpan TotalDuration { get; set; }
        
   
        public List<WorkSessionDTO> Sessions { get; set; } = new List<WorkSessionDTO>();
    }
}