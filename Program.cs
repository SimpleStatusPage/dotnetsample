using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace updateStatusExample
{
    public enum StatusEnum
    {
        Good = 1,
        Warning = 2,
        Error = 3,
        Information = 4
    }

    class StatusUpdate
    {
        public Guid ServiceId { get; set; }

        public StatusEnum Status { get; set; }

        // the message is optional
        public string Message { get; set; }
    }

    class StatusUpdateRequest
    {
        public IReadOnlyCollection<StatusUpdate> StatusChanges { get; set; }
    }

    class Program
    {
        private static void Main()
        {
            UpdateStatus().Wait();
        }

        private static async Task UpdateStatus()
        {
            // Your API key can be obtained in the API section of the management portal
            const string apiAccessKey = "your-api-key";
            Uri apiUri = new Uri("https://statusapi.simplestatuspage.com/api/v1/status");

            StatusUpdateRequest request = new StatusUpdateRequest
            {
                StatusChanges = new List<StatusUpdate>
                {
                    // service IDs can be found in the API section of the management portal
                    new StatusUpdate {ServiceId = Guid.Parse("your-service-id"), Status = StatusEnum.Warning, Message = "Sample client update"}
                }
            };
            string requestJson = JsonConvert.SerializeObject(request);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("api", apiAccessKey);
            HttpResponseMessage response = await client.PostAsync(apiUri, new StringContent(requestJson, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }
    }
}
