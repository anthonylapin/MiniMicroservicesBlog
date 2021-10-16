using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace HttpClients.Helpers
{
    public class HttpHelpers
    {
        public static HttpRequestMessage GetJsonMessage(string url, HttpMethod httpMethod, object body,
            UriKind uriKind = UriKind.RelativeOrAbsolute)
        {
            var jsonContent = JsonSerializer.Serialize(body);

            Console.WriteLine($"Json content: {jsonContent}");

            var message = new HttpRequestMessage
            {
                Method = httpMethod,
                RequestUri = new Uri(url, uriKind),
                Content = new StringContent(jsonContent, Encoding.UTF8, MediaTypeNames.Application.Json)
            };
            
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            return message;
        }
    }
}