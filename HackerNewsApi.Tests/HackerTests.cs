using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Moq;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using Moq.Protected;
using RichardSzalay.MockHttp;

namespace HackerNewsApi.Tests
{
    [TestClass]
    public class HackerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Http_Null_ClientFactory_Throws_Exception()
        {
            var loggerMock = new Mock<ILogger<Http>>();

            IHttpClientFactory clientFactory = null;
            Http http = new Http(loggerMock.Object, clientFactory);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Http_Null_Logger_Throws_Exception()
        {
            var clientFactoryMock = new Mock<IHttpClientFactory>();

            ILogger<Http> logger = null;
            Http http = new Http(logger, clientFactoryMock.Object);

        }

    }
}
