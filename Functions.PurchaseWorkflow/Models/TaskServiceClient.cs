using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Functions.PurchaseWorkflow.Models
{
    public class TaskServiceClient
    {
        private readonly HttpClient _http;
        private readonly TaskServiceOptions _options;

        public TaskServiceClient(HttpClient http, IOptions<TaskServiceOptions> options)
        {
            _http = http;
            _options = options.Value;
        }

        public Task<HttpResponseMessage> CreateTaskAsync(CreateTaskDto dto, CancellationToken ct)
        {
            var url = $"{_options.BaseUrl.TrimEnd('/')}/api/tasks";
            return _http.PostAsJsonAsync(url, dto, ct);
        }
    }

}


