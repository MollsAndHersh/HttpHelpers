using System;
using System.Net.Http;
using Xunit;

namespace HttpHelpers.Tests
{
    public class BaseHttpClientTests
    {
        [Fact]
        public void CreateHttpRequestMessage_Success()
        {
            var mockHttpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://test.com")
            };

            TestObject testObject = new TestObject(mockHttpClient);
            var httpRequestMessage = testObject.CreateHttpRequestMessage(HttpMethod.Get, "/TestPath");

            Assert.Equal(HttpMethod.Get, httpRequestMessage.Method);
            Assert.Equal("/TestPath", httpRequestMessage.RequestUri.OriginalString);
        }

        [Fact]
        public void CreateHttpRequestMessage_WithContent_Success()
        {
            var mockHttpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://test.com")
            };

            TestContent testContent = new TestContent("Test Value");

            TestObject testObject = new TestObject(mockHttpClient);
            var httpRequestMessage = testObject.CreateHttpRequestMessage<TestContent>(HttpMethod.Post, "/TestPath", testContent);

            Assert.Equal(HttpMethod.Post, httpRequestMessage.Method);
            Assert.Equal("/TestPath", httpRequestMessage.RequestUri.OriginalString);
            Assert.NotNull(httpRequestMessage.Content);
        }
    }

    public class TestObject : BaseHttpClient
    {
        public TestObject(HttpClient httpClient)
            : base(httpClient)
        {
            
        }

        public new HttpRequestMessage CreateHttpRequestMessage(HttpMethod httpMethod, string relativePath)
        {
            return base.CreateHttpRequestMessage(httpMethod, relativePath);
        }

        public HttpRequestMessage CreateHttpRequestMessage<TContent>(HttpMethod httpMethod, string relativePath, TContent content)
        {
            return base.CreateHttpRequestMessage<TContent>(httpMethod, relativePath, content);
        }
    }

    public class TestContent
    {
        public string TestProperty { get; set; }

        public TestContent(string testValue)
        {
            TestProperty = testValue;
        }
    }
}
