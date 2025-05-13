using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using RaftLab_Assignment.Controllers;
using RaftLab_Assignment.Interfaces;
using RaftLab_Assignment.Models;
using RaftLab_Assignment.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RaftLab_Assignment.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IReqResInterface> _mockService;
        private readonly Mock<ILogger<UsersController>> _mockLogger;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockService = new Mock<IReqResInterface>();
            _mockLogger = new Mock<ILogger<UsersController>>();
            var mockOptions = Options.Create(new ReqResAPISettings { BaseUrl = "https://reqres.in/api" });

            _controller = new UsersController(_mockService.Object, mockOptions, _mockLogger.Object);
        }

        [Fact]
        public async Task GetUserById_ReturnsOk_WhenUserExists()
        {
            // Arrange
            var user = new User
            {
                id = 1,
                email = "george.bluth@reqres.in",
                first_name = "George",
                last_name = "Bluth",
                avatar = "https://reqres.in/img/faces/1-image.jpg"
            };
            _mockService.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(user);

            // Act
            var result = await _controller.GetUserById(1);

            // Assert
            var ok = result.Should().BeOfType<OkObjectResult>().Subject;
            var returned = ok.Value.Should().BeOfType<User>().Subject;
            returned.id.Should().Be(1);
        }

        [Fact]
        public async Task GetUserById_ReturnsNotFound_WhenUserNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetUserByIdAsync(99))
                        .ThrowsAsync(new KeyNotFoundException("User not found"));

            // Act
            var result = await _controller.GetUserById(99);

            // Assert
            var notFound = result.Should().BeOfType<NotFoundObjectResult>().Subject;
            var message = notFound.Value.Should().BeAssignableTo<dynamic>().Subject;
        }

        [Fact]
        public async Task GetUserById_ReturnsBadRequest_WhenInvalidId()
        {
            // Arrange
            _mockService.Setup(s => s.GetUserByIdAsync(0))
                        .ThrowsAsync(new ArgumentException("Invalid ID"));

            // Act
            var result = await _controller.GetUserById(0);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetAllUsers_ReturnsOk_WhenUsersExist()
        {
            // Arrange
            var users = new List<User>
            {
               new User
            {
                id = 1,
                email = "george.bluth@reqres.in",
                first_name = "George",
                last_name = "Bluth",
                avatar = "https://reqres.in/img/faces/1-image.jpg"
            },
            new User
            {
                id = 2,
                email = "janet.weaver@reqres.in",
                first_name = "Janet",
                last_name = "Weaver",
                avatar = "https://reqres.in/img/faces/2-image.jpg"
            },
            new User
            {
                id = 3,
                email = "emma.wong@reqres.in",
                first_name = "Emma",
                last_name = "Wong",
                avatar = "https://reqres.in/img/faces/3-image.jpg"
            },
            new User
            {
                id = 4,
                email = "eve.holt@reqres.in",
                first_name = "Eve",
                last_name = "Holt",
                avatar = "https://reqres.in/img/faces/4-image.jpg"
            },
            new User
            {
                id = 5,
                email = "charles.morris@reqres.in",
                first_name = "Charles",
                last_name = "Morris",
                avatar = "https://reqres.in/img/faces/5-image.jpg"
            },
            new User
            {
                id = 6,
                email = "tracey.ramos@reqres.in",
                first_name = "Tracey",
                last_name = "Ramos",
                avatar = "https://reqres.in/img/faces/6-image.jpg"
            }
            };
            _mockService.Setup(s => s.GetAllUsersAsync(1)).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsers(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;

            okResult?.Value.Should().NotBeNull();
            var returnedUsers = okResult?.Value as IEnumerable<User>;
            returnedUsers.Should().HaveCount(6);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsBadRequest_WhenPageIsInvalid()
        {
            // Arrange
            _mockService.Setup(s => s.GetAllUsersAsync(0))
                        .ThrowsAsync(new ArgumentException("Invalid page"));

            // Act
            var result = await _controller.GetAllUsers(0);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
