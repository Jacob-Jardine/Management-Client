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
using Management_Web_Application.Services.ProductService;

namespace Management_Web_App_xUnit_Tests
{
    public class ProductServiceTests
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

        private IProductService ProductService(HttpClient client)
        {
            var mockConfiguration = new Mock<IConfiguration>(MockBehavior.Strict);
            mockConfiguration.Setup(c => c["PRODUCT_BASE_URL"])
                             .Returns("https://thamco-staging-products.azurewebsites.net/api/products/");
            return new ProductService(mockConfiguration.Object, client);
        }

        [Fact]
        public async void GetAllProducts_True()
        {
            //Arrange
            var expectedResult = new PostToProductServiceDomainModel[]
            { 
                new PostToProductServiceDomainModel() {productID = 1, productName = "Product Name", productDescription = "Product Desc", productPrice = 100, productQuantity = 10},
                new PostToProductServiceDomainModel() {productID = 2, productName = "Product Name", productDescription = "Product Desc", productPrice = 10, productQuantity = 5}
            };
            var expectedJson = JsonConvert.SerializeObject(expectedResult);
            var expectedUri = new Uri("https://thamco-staging-products.azurewebsites.net/api/products/");
            var mock = CreateHttpMock(HttpStatusCode.OK, expectedJson);
            var client = new HttpClient(mock.Object);
            var service = ProductService(client);

            //Act
            var result = (await service.GetProducts(String.Empty)).ToArray();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Length, result.Length);
            for (int i = 0; i < result.Length; i++)
            {
                Assert.Equal(expectedResult[i].productID, result[i].productID);
            }

            mock.Protected()
                .Verify("SendAsync",
                        Times.Once(),
                        ItExpr.Is<HttpRequestMessage>(
                            req => req.Method == HttpMethod.Get
                                   && req.RequestUri == expectedUri),
                        ItExpr.IsAny<CancellationToken>()
                        );
        }

        [Fact]
        public async void GetAllProducts_False()
        {
            //Arrange
            var expectedUri = new Uri("https://thamco-staging-products.azurewebsites.net/api/products/");
            var mock = CreateHttpMock(HttpStatusCode.NotFound, string.Empty);
            var client = new HttpClient(mock.Object);
            var service = ProductService(client);

            //Act
            var result = await service.GetProducts(String.Empty);

            //Assert
            Assert.Null(result);

            mock.Protected()
                .Verify("SendAsync",
                        Times.Once(),
                        ItExpr.Is<HttpRequestMessage>(
                            req => req.Method == HttpMethod.Get
                                   && req.RequestUri == expectedUri),
                        ItExpr.IsAny<CancellationToken>()
                        );
        }
    }
}
