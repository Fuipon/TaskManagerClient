using System.Net.Http;
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
            _http.BaseAddress = new Uri("https://taskmanagerapi-lnfc.onrender.com/");
        }

        public async Task<List<TaskItemDTO>> GetTasksAsync()
        {
            var response = await _http.GetAsync("api/task");
            await HandleErrorResponseAsync(response);

            var tasks = await response.Content.ReadFromJsonAsync<List<TaskItemDTO>>();
            return tasks ?? new List<TaskItemDTO>();
        }

        public async Task AddTaskAsync(TaskItemDTO task)
        {
            var response = await _http.PostAsJsonAsync("api/task", task);
            await HandleErrorResponseAsync(response);
        }

        public async Task DeleteTaskAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/task/{id}");
            await HandleErrorResponseAsync(response);
        }

        public async Task UpdateTaskAsync(TaskItemDTO task)
        {
            var response = await _http.PutAsJsonAsync($"api/task/{task.Id}", task);
            await HandleErrorResponseAsync(response);
        }

        // ⬇️ Общий обработчик ошибок
        private async Task HandleErrorResponseAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                try
                {
                    var problem = JsonSerializer.Deserialize<ProblemDetails>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    throw new Exception(problem?.Title ?? "Произошла ошибка при запросе к API.");
                }
                catch (JsonException)
                {
                    throw new Exception("Ошибка при обработке ответа сервера.");
                }
            }
        }
    }
}
