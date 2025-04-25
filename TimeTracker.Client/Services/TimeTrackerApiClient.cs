using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TimeTracker.Core.DTOs;

namespace TimeTracker.Client.Services
{
  
    public class TimeTrackerApiClient : ITimeTrackerApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

    
        public TimeTrackerApiClient(HttpClient httpClient, string baseUrl)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl.TrimEnd('/');
        }

        public async Task<ApiResponse<int>> StartSessionAsync()
        {
            try
            {
                var response = await _httpClient.PostAsync($"{_baseUrl}/api/TimeTracker/sessions/start", null);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResponse<int>>();
                }
                
                return ApiResponse<int>.Error($"Error: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Error($"API client error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> StopActiveSessionAsync()
        {
            try
            {
                var response = await _httpClient.PutAsync($"{_baseUrl}/api/TimeTracker/sessions/stop-active", null);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
                }
                
                return ApiResponse<bool>.Error($"Error: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Error($"API client error: {ex.Message}");
            }
        }

       
        public async Task<ApiResponse<bool>> StopSessionAsync(int sessionId)
        {
            try
            {
                var response = await _httpClient.PutAsync($"{_baseUrl}/api/TimeTracker/sessions/{sessionId}/stop", null);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
                }
                
                return ApiResponse<bool>.Error($"Error: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Error($"API client error: {ex.Message}");
            }
        }

      
        public async Task<ApiResponse<bool>> DeleteSessionAsync(int sessionId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/TimeTracker/sessions/{sessionId}");
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
                }
                
                return ApiResponse<bool>.Error($"Error: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Error($"API client error: {ex.Message}");
            }
        }

        
        public async Task<ApiResponse<WorkSessionDTO>> GetActiveSessionAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/TimeTracker/sessions/active");
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResponse<WorkSessionDTO>>();
                }
                
                return ApiResponse<WorkSessionDTO>.Error($"Error: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                return ApiResponse<WorkSessionDTO>.Error($"API client error: {ex.Message}");
            }
        }

       
        public async Task<ApiResponse<List<WorkSessionDTO>>> GetAllSessionsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/TimeTracker/sessions");
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResponse<List<WorkSessionDTO>>>();
                }
                
                return ApiResponse<List<WorkSessionDTO>>.Error($"Error: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<WorkSessionDTO>>.Error($"API client error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TimeReportDTO>> GetDailyReportAsync(DateTime? date = null)
        {
            try
            {
                string url = $"{_baseUrl}/api/TimeTracker/reports/daily";
                
                if (date.HasValue)
                {
                    url += $"?date={date.Value:yyyy-MM-dd}";
                }
                
                var response = await _httpClient.GetAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResponse<TimeReportDTO>>();
                }
                
                return ApiResponse<TimeReportDTO>.Error($"Error: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                return ApiResponse<TimeReportDTO>.Error($"API client error: {ex.Message}");
            }
        }

      
        public async Task<ApiResponse<TimeReportDTO>> GetWeeklyReportAsync(DateTime? startDate = null)
        {
            try
            {
                string url = $"{_baseUrl}/api/TimeTracker/reports/weekly";
                
                if (startDate.HasValue)
                {
                    url += $"?startDate={startDate.Value:yyyy-MM-dd}";
                }
                
                var response = await _httpClient.GetAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResponse<TimeReportDTO>>();
                }
                
                return ApiResponse<TimeReportDTO>.Error($"Error: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                return ApiResponse<TimeReportDTO>.Error($"API client error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TimeReportDTO>> GetMonthlyReportAsync(int? year = null, int? month = null)
        {
            try
            {
                var url = $"{_baseUrl}/api/TimeTracker/reports/monthly";
                var queryParams = new List<string>();
                
                if (year.HasValue)
                {
                    queryParams.Add($"year={year.Value}");
                }
                
                if (month.HasValue)
                {
                    queryParams.Add($"month={month.Value}");
                }
                
                if (queryParams.Count > 0)
                {
                    url += "?" + string.Join("&", queryParams);
                }
                
                var response = await _httpClient.GetAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResponse<TimeReportDTO>>();
                }
                
                return ApiResponse<TimeReportDTO>.Error($"Error: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                return ApiResponse<TimeReportDTO>.Error($"API client error: {ex.Message}");
            }
        }

      
        public async Task<ApiResponse<List<DailyReportItemDTO>>> GetDailyBreakdownAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var url = $"{_baseUrl}/api/TimeTracker/reports/breakdown";
                var queryParams = new List<string>();
                
                if (startDate.HasValue)
                {
                    queryParams.Add($"startDate={startDate.Value:yyyy-MM-dd}");
                }
                
                if (endDate.HasValue)
                {
                    queryParams.Add($"endDate={endDate.Value:yyyy-MM-dd}");
                }
                
                if (queryParams.Count > 0)
                {
                    url += "?" + string.Join("&", queryParams);
                }
                
                var response = await _httpClient.GetAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResponse<List<DailyReportItemDTO>>>();
                }
                
                return ApiResponse<List<DailyReportItemDTO>>.Error($"Error: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<DailyReportItemDTO>>.Error($"API client error: {ex.Message}");
            }
        }
    }
}