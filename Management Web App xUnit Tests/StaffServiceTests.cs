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

namespace Management_Web_App_xUnit_Tests
{
    public class StaffServiceTests
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

        private IStaffService StaffService(HttpClient client)
        {
            var mockConfiguration = new Mock<IConfiguration>(MockBehavior.Strict);
            mockConfiguration.Setup(c => c["STAFF_BASE_URL"])
                             .Returns("https://t7077222-staging-staff.azurewebsites.net/api/staff/");
            return new StaffService(mockConfiguration.Object, client);
        }

        [Fact]
        public async void GetAllStaff_True()
        {
            //Arrange
            var expectedResult = new StaffDTO[]
            {
                new StaffDTO() {StaffID = 1, StaffFirstName = "Jacob", StaffLastName = "Jardine", StaffEmailAddress = "Jacob.Jardine@ThAmCo.com"},
                new StaffDTO() {StaffID = 1, StaffFirstName = "Ben", StaffLastName = "Souch", StaffEmailAddress = "Ben.Souch@ThAmCo.com"}
            };
            var expectedJson = JsonConvert.SerializeObject(expectedResult);
            var expectedUri = new Uri("https://t7077222-staging-staff.azurewebsites.net/api/staff/GetAllStaff");
            var mock = CreateHttpMock(HttpStatusCode.OK, expectedJson);
            var client = new HttpClient(mock.Object);
            var service = StaffService(client);

            //Act
            var result = (await service.GetAllStaffAsync(String.Empty)).ToArray();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Length, result.Length);
            for (int i = 0; i < result.Length; i++)
            {
                Assert.Equal(expectedResult[i].StaffID, result[i].StaffID);
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
        public async void GetAllStaff_False()
        {
            //Arrange
            var expectedUri = new Uri("https://t7077222-staging-staff.azurewebsites.net/api/staff/GetAllStaff");
            var mock = CreateHttpMock(HttpStatusCode.NotFound, String.Empty);
            var client = new HttpClient(mock.Object);
            var service = StaffService(client);

            //Act
            var result = await service.GetAllStaffAsync(String.Empty);

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

        [Fact]
        public async void GetStaffById_True()
        {
            //Arrange
            var expectedResult = new StaffDTO{StaffID = 1, StaffFirstName = "Jacob", StaffLastName = "Jardine", StaffEmailAddress = "Jacob.Jardine@ThAmCo.com" };
            var expectedJson = JsonConvert.SerializeObject(expectedResult);
            var id = expectedResult.StaffID;
            var expectedUri = new Uri($"https://t7077222-staging-staff.azurewebsites.net/api/staff/{id}");
            var mock = CreateHttpMock(HttpStatusCode.OK, expectedJson);
            var client = new HttpClient(mock.Object);
            var service = StaffService(client);

            //Act
            var result = await service.GetStaffByIDAsnyc(id, string.Empty);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.StaffID, result.StaffID);

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
        public async void GetStaffById_False()
        {
            //Arrange
            var expectedUri = new Uri("https://t7077222-staging-staff.azurewebsites.net/api/staff/" + 1);
            var mock = CreateHttpMock(HttpStatusCode.NotFound, String.Empty);
            var client = new HttpClient(mock.Object);
            var service = StaffService(client);

            //Act
            var result = await service.GetStaffByIDAsnyc(1, string.Empty);

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

        [Fact]
        public async void DeletStaff_True()
        {
            //Arrange
            var expectedUri = new Uri("https://t7077222-staging-staff.azurewebsites.net/api/staff/delete/" + 1);
            var mock = CreateHttpMock(HttpStatusCode.NotFound, String.Empty);
            var client = new HttpClient(mock.Object);
            var service = StaffService(client);

            //Act
            var result = await service.DeleteStaff(1, String.Empty);

            //Assert
            Assert.False(result);

            mock.Protected()
                .Verify("SendAsync",
                        Times.Once(),
                        ItExpr.Is<HttpRequestMessage>(
                            req => req.Method == HttpMethod.Delete
                                   && req.RequestUri == expectedUri),
                        ItExpr.IsAny<CancellationToken>()
                        );
        }

        [Fact]
        public async void DeletStaff_False()
        {
            //Arrange
            var expectedUri = new Uri("https://t7077222-staging-staff.azurewebsites.net/api/staff/delete/" + 1);
            var mock = CreateHttpMock(HttpStatusCode.NotFound, String.Empty);
            var client = new HttpClient(mock.Object);
            var service = StaffService(client);

            //Act
            var result = await service.DeleteStaff(1, String.Empty);

            //Assert
            Assert.False(result);

            mock.Protected()
                .Verify("SendAsync",
                        Times.Once(),
                        ItExpr.Is<HttpRequestMessage>(
                            req => req.Method == HttpMethod.Delete
                                   && req.RequestUri == expectedUri),
                        ItExpr.IsAny<CancellationToken>()
                        );
        }

        [Fact]
        public async void CreateStaff_True()
        {
            //Arrange
            var expectedResult = new StaffDTO { StaffID = 1, StaffFirstName = "Jacob", StaffLastName = "Jardine", StaffEmailAddress = "Jacob.Jardine@ThAmCo.com" };
            var expectedJson = JsonConvert.SerializeObject(expectedResult);
            var id = expectedResult.StaffID;
            var expectedUri = new Uri($"https://t7077222-staging-staff.azurewebsites.net/api/staff/CreateStaff");
            var mock = CreateHttpMock(HttpStatusCode.OK, expectedJson);
            var client = new HttpClient(mock.Object);
            var service = StaffService(client);

            //Act
            var result = await service.CreateStaffAsync(expectedResult, string.Empty);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.StaffID, result.StaffID);
            mock.Protected()
                .Verify("SendAsync",
                        Times.Never(),
                        ItExpr.Is<HttpRequestMessage>(
                            req => req.Method == HttpMethod.Get
                                   && req.RequestUri == expectedUri),
                        ItExpr.IsAny<CancellationToken>()
                        );
        }

        [Fact]
        public async void CreateStaff_Null()
        {
            //Arrange
            var expectedUri = new Uri($"https://t7077222-staging-staff.azurewebsites.net/api/staff/CreateStaff");
            var mock = CreateHttpMock(HttpStatusCode.NotFound, string.Empty);
            var client = new HttpClient(mock.Object);
            var service = StaffService(client);

            //Act
            var result = await service.CreateStaffAsync(null, string.Empty);

            //Assert
            Assert.Null(result);

            mock.Protected()
                .Verify("SendAsync",
                        Times.Never(),
                        ItExpr.Is<HttpRequestMessage>(
                            req => req.Method == HttpMethod.Get
                                   && req.RequestUri == expectedUri),
                        ItExpr.IsAny<CancellationToken>()
                        );
        }

        [Fact]
        public async void UpdateStaff_True()
        {
            //Arrange
            var expectedResult = new StaffUpdateDTO { StaffFirstName = "Jacob", StaffLastName = "Jardine"};
            var expectedJson = JsonConvert.SerializeObject(expectedResult);
            var id = expectedResult.StaffID;
            var expectedUri = new Uri($"https://t7077222-staging-staff.azurewebsites.net/api/staff/Update/{id}");
            var mock = CreateHttpMock(HttpStatusCode.OK, expectedJson);
            var client = new HttpClient(mock.Object);
            var service = StaffService(client);

            //Act
            var result = await service.UpdateStaff(expectedResult, string.Empty);

            //Assert
            Assert.NotNull(result);

            mock.Protected()
                .Verify("SendAsync",
                        Times.Never(),
                        ItExpr.Is<HttpRequestMessage>(
                            req => req.Method == HttpMethod.Put
                                   && req.RequestUri == expectedUri),
                        ItExpr.IsAny<CancellationToken>()
                        );
        }

        [Fact]
        public async void UpdateStaff_False()
        {
            //Arrange
            var expectedResult = new StaffUpdateDTO { StaffFirstName = "Jacob", StaffLastName = "Jardine" };
            var expectedJson = JsonConvert.SerializeObject(expectedResult);
            var id = expectedResult.StaffID;
            var expectedUri = new Uri($"https://t7077222-staging-staff.azurewebsites.net/api/staff/Update/{id}");
            var mock = CreateHttpMock(HttpStatusCode.NotFound, string.Empty);
            var client = new HttpClient(mock.Object);
            var service = StaffService(client);

            //Act
            var result = await service.UpdateStaff(expectedResult, string.Empty);

            //Assert
            Assert.False(result);

            mock.Protected()
                .Verify("SendAsync",
                        Times.Never(),
                        ItExpr.Is<HttpRequestMessage>(
                            req => req.Method == HttpMethod.Put
                                   && req.RequestUri == expectedUri),
                        ItExpr.IsAny<CancellationToken>()
                        );
        }
    }
}
