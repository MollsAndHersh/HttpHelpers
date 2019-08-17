using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace HttpHelpers
{
    public class BaseHttpClient
    {
        protected readonly HttpClient _client;

        protected BaseHttpClient(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Create HttpRequest
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        protected HttpRequestMessage CreateHttpRequestMessage(HttpMethod httpMethod, string requestUri)
        {
            return new HttpRequestMessage(httpMethod, requestUri);
        }

        protected virtual HttpRequestMessage CreateHttpRequestMessage<TContent>(HttpMethod httpMethod, string requestUri, TContent content, MediaTypeFormatter mediaTypeFormatter = null)
        {
            // Default mediaTypeFormatter is JsonMediaTypeFormatter.
            mediaTypeFormatter = mediaTypeFormatter ?? new JsonMediaTypeFormatter();

            HttpRequestMessage returnValue = new HttpRequestMessage(httpMethod, requestUri);
            returnValue.Content = new ObjectContent<TContent>(content, mediaTypeFormatter);
            return returnValue;
        }

        protected async Task SendHttpRequestMessageAsync(HttpRequestMessage requestMessage)
        {
            HttpResponseMessage response = await _client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
        }

        protected async Task<TResponse> SendHttpRequestMessageAsync<TResponse>(HttpRequestMessage requestMessage)
        {
            HttpResponseMessage response = await _client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<TResponse>(new List<MediaTypeFormatter>()
            {
                new XmlMediaTypeFormatter { UseXmlSerializer = true },
                new JsonMediaTypeFormatter()
            });
        }
    }
}
