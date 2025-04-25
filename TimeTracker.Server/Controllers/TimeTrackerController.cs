using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Core.DTOs;
using TimeTracker.Server.Services;

namespace TimeTracker.Server.Controllers
{
 
    [ApiController]
    [Route("api/[controller]")]
    public class TimeTrackerController : ControllerBase
    {
        private readonly ITimeTrackerService _timeTrackerService;
        public TimeTrackerController(ITimeTrackerService timeTrackerService)
        {
            _timeTrackerService = timeTrackerService;
        }
        [HttpPost("sessions/start")]
        public async Task<ActionResult<ApiResponse<int>>> StartSession()
        {
            try
            {
                var sessionId = await _timeTrackerService.StartSessionAsync();
                return Ok(ApiResponse<int>.Ok(sessionId, "Session started successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<int>.Error($"Error starting session: {ex.Message}"));
            }
        }
        
        
        [HttpPut("sessions/stop-active")]
        public async Task<ActionResult<ApiResponse<bool>>> StopActiveSession()
        {
            try
            {
                var result = await _timeTrackerService.StopActiveSessionAsync();
                
                if (result)
                {
                    return Ok(ApiResponse<bool>.Ok(true, "Active session stopped successfully"));
                }
                
                return NotFound(ApiResponse<bool>.Error("No active session found"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.Error($"Error stopping session: {ex.Message}"));
            }
        }
        
      
        [HttpPut("sessions/{id}/stop")]
        public async Task<ActionResult<ApiResponse<bool>>> StopSession(int id)
        {
            try
            {
                var result = await _timeTrackerService.StopSessionAsync(id);
                
                if (result)
                {
                    return Ok(ApiResponse<bool>.Ok(true, $"Session {id} stopped successfully"));
                }
                
                return NotFound(ApiResponse<bool>.Error($"Session {id} not found or already stopped"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.Error($"Error stopping session: {ex.Message}"));
            }
        }
        
     
        [HttpDelete("sessions/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteSession(int id)
        {
            try
            {
                var result = await _timeTrackerService.DeleteSessionAsync(id);
                
                if (result)
                {
                    return Ok(ApiResponse<bool>.Ok(true, $"Session {id} deleted successfully"));
                }
                
                return NotFound(ApiResponse<bool>.Error($"Session {id} not found"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.Error($"Error deleting session: {ex.Message}"));
            }
        }
     
        [HttpGet("sessions/active")]
        public async Task<ActionResult<ApiResponse<WorkSessionDTO>>> GetActiveSession()
        {
            try
            {
                var session = await _timeTrackerService.GetActiveSessionAsync();
                
                if (session != null)
                {
                    return Ok(ApiResponse<WorkSessionDTO>.Ok(session));
                }
                
                return NotFound(ApiResponse<WorkSessionDTO>.Error("No active session found"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<WorkSessionDTO>.Error($"Error retrieving active session: {ex.Message}"));
            }
        }
        
     
        [HttpGet("sessions")]
        public async Task<ActionResult<ApiResponse<List<WorkSessionDTO>>>> GetAllSessions()
        {
            try
            {
                var sessions = await _timeTrackerService.GetAllSessionsAsync();
                return Ok(ApiResponse<List<WorkSessionDTO>>.Ok(sessions));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<WorkSessionDTO>>.Error($"Error retrieving sessions: {ex.Message}"));
            }
        }
        
    
        [HttpGet("reports/daily")]
        public async Task<ActionResult<ApiResponse<TimeReportDTO>>> GetDailyReport([FromQuery] DateTime? date = null)
        {
            try
            {
                var reportDate = date?.Date ?? DateTime.Today;
                var report = await _timeTrackerService.GetDailyReportAsync(reportDate);
                
                return Ok(ApiResponse<TimeReportDTO>.Ok(report));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<TimeReportDTO>.Error($"Error generating daily report: {ex.Message}"));
            }
        }
        
        
        [HttpGet("reports/weekly")]
        public async Task<ActionResult<ApiResponse<TimeReportDTO>>> GetWeeklyReport([FromQuery] DateTime? startDate = null)
        {
            try
            {
                var reportStartDate = startDate?.Date ?? 
                    DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                    
                var report = await _timeTrackerService.GetWeeklyReportAsync(reportStartDate);
                
                return Ok(ApiResponse<TimeReportDTO>.Ok(report));
            }catch (Exception ex) {
                return StatusCode(500, ApiResponse<TimeReportDTO>.Error($"Error generating weekly report: {ex.Message}"));
            }
        }
        
        
        [HttpGet("reports/monthly")]
        public async Task<ActionResult<ApiResponse<TimeReportDTO>>> GetMonthlyReport([FromQuery] int? year = null, [FromQuery] int? month = null)
        {
            try{
                int reportYear = year ?? DateTime.Today.Year;
            int reportMonth = month ?? DateTime.Today.Month;
                
                var report = await _timeTrackerService.GetMonthlyReportAsync(reportYear, reportMonth);
                
                return Ok(ApiResponse<TimeReportDTO>.Ok(report));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<TimeReportDTO>.Error($"Error generating monthly report: {ex.Message}"));
            }
        }
        [HttpGet("reports/breakdown")]
        public async Task<ActionResult<ApiResponse<List<DailyReportItemDTO>>>> GetDailyBreakdown(
            [FromQuery] DateTime? startDate = null, 
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var today = DateTime.Today;
                var reportStartDate = startDate?.Date ?? today.AddDays(-6);
                var reportEndDate = endDate?.Date ?? today;
                
                var breakdown = await _timeTrackerService.GetDailyBreakdownAsync(reportStartDate, reportEndDate);
                
                return Ok(ApiResponse<List<DailyReportItemDTO>>.Ok(breakdown));
            }
            catch (Exception ex)
            {
         return StatusCode(500, ApiResponse<List<DailyReportItemDTO>>.Error($"Error generating daily breakdown: {ex.Message}"));
            }
        }
    }
}