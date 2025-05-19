using System.Net.Http.Json;
using System.Text.Json;
using TaskManagerClient.Models;
using TaskManagerClient.Shared;

namespace TaskManagerClient.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;

        public ApiService(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri("https://localhost:7045"); 
        }

        public async Task<List<TaskItemDTO>> GetTasksAsync()
        {
            var tasks = await _http.GetFromJsonAsync<List<TaskItemDTO>>("/api/task");
            return tasks ?? new List<TaskItemDTO>();
        }

        public async Task AddTaskAsync(TaskItemDTO task)
        {
            await _http.PostAsJsonAsync("/api/task", task);
        }

        public async Task DeleteTaskAsync(int id)
        {
            await _http.DeleteAsync($"/api/task/{id}");
        }

        public async Task UpdateTaskAsync(TaskItemDTO task)
        {
            await _http.PutAsJsonAsync($"/api/task/{task.Id}", task);
        }
    }
}
