using Microsoft.Extensions.Configuration;
using Management_Web_Application.Services.StaffService;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Management_Web_Application.DomainModel;
using Newtonsoft.Json;
using System.Linq;
using Management_Web_Application.Services.PurchaseService;

namespace Management_Web_App_xUnit_Tests
{
    public class SendPurchaseRequestServiceTests
    {
        private Mock<HttpMessageHandler> CreateHttpMock(HttpStatusCode expectedCode,
                                                        string expectedJson)
        {
            var response = new HttpResponseMessage
            {
                StatusCode = expectedCode
            };
            if (expectedJson != null)
            {
                response.Content = new StringContent(expectedJson,
                                                     Encoding.UTF8,
                                                     "application/json");
            }
            var mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response)
                .Verifiable();
            return mock;
        }

        private IThirdPartyStockService SendPurcahseReqeust(HttpClient client)
        {
            var mockConfiguration = new Mock<IConfiguration>(MockBehavior.Strict);
            mockConfiguration.Setup(c => c["PURCHASE_BASE_URL"])
                             .Returns("https://t7077222-staging-purchase.azurewebsites.net/api/Third-Party/");
            return new ThirdPartyStockService(mockConfiguration.Object, client);
        }

        [Fact]
        public async void SendPurchaseRequest_True()
        {
            //Arrange
            var expectedResult = new SendPurchaseRequestDomainModel { ProductId = 1, AccountName = "Test Account", CardNumber = "1111222233334444", Quantity = 5 };
            var expectedJson = JsonConvert.SerializeObject(expectedResult);
            var expectedUri = new Uri("https://t7077222-staging-purchase.azurewebsites.net/api/Third-Party/Send-Purchase-Request");
            var mock = CreateHttpMock(HttpStatusCode.OK, expectedJson);
            var client = new HttpClient(mock.Object);
            var service = SendPurcahseReqeust(client);

            //Act
            var result = await service.SendPurchaseRequest(expectedResult);

            //Assert
            Assert.NotNull(result);
            Assert.True(result);

            mock.Protected()
                .Verify("SendAsync",
                        Times.Once(),
                        ItExpr.Is<HttpRequestMessage>(
                            req => req.Method == HttpMethod.Post
                                   && req.RequestUri == expectedUri),
                        ItExpr.IsAny<CancellationToken>()
                        );
        }

        [Fact]
        public async void SendPurchaseRequest_False()
        {
            //Arrange
            var expectedUri = new Uri("https://t7077222-staging-purchase.azurewebsites.net/api/Third-Party/Send-Purchase-Request");
            var mock = CreateHttpMock(HttpStatusCode.NotFound, string.Empty);
            var client = new HttpClient(mock.Object);
            var service = SendPurcahseReqeust(client);

            //Act
            var result = await service.SendPurchaseRequest(null);

            //Assert
            Assert.False(result);

            mock.Protected()
                .Verify("SendAsync",
                        Times.Never(),
                        ItExpr.Is<HttpRequestMessage>(
                            req => req.Method == HttpMethod.Post
                                   && req.RequestUri == expectedUri),
                        ItExpr.IsAny<CancellationToken>()
                        );
        }
    }
}
