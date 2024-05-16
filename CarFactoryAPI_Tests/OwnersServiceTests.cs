using CarAPI.Entities;
using CarAPI.Models;
using CarAPI.Payment;
using CarAPI.Repositories_DAL;
using CarAPI.Services_BLL;
using CarFactoryAPI.Entities;
using CarFactoryAPI.Repositories_DAL;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace CarFactoryAPI_Tests
{
    public class OwnersServiceTests : IDisposable
    {
        private readonly ITestOutputHelper testOutputHelper;
        // Create Mock Of Dependencies
        Mock<ICarsRepository> carRepoMock;
        Mock<IOwnersRepository> OwnerRepoMock;
        Mock<ICashService> cashMock;

        // use fake object as a dependency
        OwnersService ownersService;

        public OwnersServiceTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
            // test setup
            testOutputHelper.WriteLine("Test setup");
            // Create Mock Of Dependencies
            carRepoMock = new();
            OwnerRepoMock = new();
            cashMock = new();

            // use fake object as a dependency
            ownersService = new OwnersService(
                carRepoMock.Object, OwnerRepoMock.Object, cashMock.Object);
        }
        public void Dispose() 
        {
            // test clean up
            testOutputHelper.WriteLine("test clean up");
        } 

        // -- Owner already has a car -- //
        [Fact]
        [Trait("Author", "Aya")]
        [Trait("Priorty", "3")]
        public void BuyCar_OwenerHasCar_AlreadyHasCar()
        {
            testOutputHelper.WriteLine("Test 01");

            //Build the mock data
            Car car = new Car() { Id = 10, Price = 1000 };
            Owner owner = new Owner { Car = new Car() };

            //Setup called method
            carRepoMock.Setup(o => o.GetCarById(10)).Returns(car);
            OwnerRepoMock.Setup(o => o.GetOwnerById(2)).Returns(owner);
            BuyCarInput carInput = new() { CarId = 10, OwnerId = 2, Amount = 1000 };

            // Act 
            string result = ownersService.BuyCar(carInput);

            //Assert
            Assert.Contains("have car", result);
        }

        // -- Owner doesn't have sufficient funds -- //
        [Fact]
        [Trait("Author", "Ali")]
        [Trait("Priority", "4")]
        public void BuyCar_InsufficientFunds_InsufficientFunds()
        {
            testOutputHelper.WriteLine("Test 02");
            //Arrange

            // Build the mock data
            Car car = new Car() { Id = 10, Price = 1000 };
            Owner owner = new Owner() { Id = 3};

            // Setup called methods
            carRepoMock.Setup(o => o.GetCarById(10)).Returns(car);
            OwnerRepoMock.Setup(o => o.GetOwnerById(3)).Returns(owner);

            BuyCarInput carInput = new() { CarId = 10, OwnerId = 3, Amount = 600 }; 

            // Act
            string result = ownersService.BuyCar(carInput);

            // Assert
            Assert.Contains("Insufficient", result);
        }

        // -- Failed to assign a car to the owner -- //
        [Fact]
        [Trait("Author", "Emad")]
        [Trait("Priority", "5")]
        public void BuyCar_AssignToOwnerFailure_SomethingWrong()
        {
            testOutputHelper.WriteLine("Test 03");

            // Arrange

            //Build the mock Data
            Car car = new Car() { Id = 10, Price = 1000 };
            Owner owner = new Owner() { Id = 4};

            // Setup called methods
            carRepoMock.Setup(o => o.GetCarById(10)).Returns(car);
            OwnerRepoMock.Setup(o => o.GetOwnerById(4)).Returns(owner);
         
            BuyCarInput carInput = new() { CarId = 10, OwnerId = 4, Amount = 1000 };

            // Act
            string result = ownersService.BuyCar(carInput);

            // Assert
            Assert.Contains("wrong", result);
        }

        // -- Assign a car to the owner successfully -- //
        [Fact]
        [Trait("Author", "Sara")]
        [Trait("Priority", "5")]
        public void BuyCar_AssignToOwnerSuccessfully_Successfull()
        {
            testOutputHelper.WriteLine("Test 04");

            // Arrange

            //Build the mock Data
            Car car = new Car() { Id = 10, Price = 1000 };
            Owner owner = new Owner() { Id = 5, Name = "Sara" };

            // Setup called methods
            carRepoMock.Setup(o => o.GetCarById(10)).Returns(car);
            OwnerRepoMock.Setup(o => o.GetOwnerById(5)).Returns(owner);
            cashMock.Setup(o => o.Pay(1000)).Returns("Amount: 1000 is paid through Cash");
            carRepoMock.Setup(r => r.AssignToOwner(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            BuyCarInput carInput = new() { CarId = 10, OwnerId = 5, Amount = 1000 };

            // Act
            string result = ownersService.BuyCar(carInput);

            // Assert
            Assert.Contains("Successful", result);
            
        }


    }
}
