using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Text;

namespace MessageFacadeProvider
{
    public static class HttpClientRequest
    {
        private static readonly IHttpClientFactory factory;
        private const string BASE_URL = "https://localhost:7277/api/";

        static HttpClientRequest()
        {
            var services = new ServiceCollection();
            services.AddHttpClient();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            factory = container.Resolve<IHttpClientFactory>();
        }

        public static async Task<T1> CreateHttpRequest<T1, T2>(List<T2>? values, string source)
        {
            var httpClient = factory.CreateClient();
            var serializer = new JsonSerializer()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            StringBuilder queryString = new();

            if (values != null)
            {
                foreach (var item in values)
                {
                    queryString.Append(item + "/");
                }
            }

            var request = new HttpRequestMessage(HttpMethod.Get, BASE_URL + $"{source}/{queryString}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStreamAsync();

            using var reader = new StreamReader(content);
            using var textReader = new JsonTextReader(reader);

            return serializer.Deserialize<T1>(textReader);
        }
    }
}