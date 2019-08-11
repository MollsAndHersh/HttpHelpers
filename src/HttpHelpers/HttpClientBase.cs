using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace HttpHelpers
{
    public class HttpClientBase
    {
        private readonly HttpClient _httpClient; 

        public HttpClientBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected virtual HttpRequestMessage CreateHttpRequestMessage(HttpMethod httpMethod, string relativePath)
        {
            HttpRequestMessage returnValue = new HttpRequestMessage(httpMethod, relativePath);
            return returnValue;
        }

        protected virtual HttpRequestMessage CreateHttpRequestMessage<TContent>(HttpMethod httpMethod, string relativePath, TContent content, MediaTypeFormatter mediaTypeFormatter = null)
        {
            // Default mediaTypeFormatter is JsonMediaTypeFormatter.
            mediaTypeFormatter = mediaTypeFormatter ?? new JsonMediaTypeFormatter();

            HttpRequestMessage returnValue = new HttpRequestMessage(httpMethod, relativePath);
            returnValue.Content = new ObjectContent<TContent>(content, mediaTypeFormatter);
            return returnValue;
        }

        protected async Task<TResponse> SendAsync<TResponse>(HttpRequestMessage request)
        {
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<TResponse>(new List<MediaTypeFormatter>()
            {
                new XmlMediaTypeFormatter { UseXmlSerializer = true },
                new JsonMediaTypeFormatter()
            });
        }
    }
}
