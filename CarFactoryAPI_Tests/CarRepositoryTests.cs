using CarAPI.Entities;
using CarFactoryAPI.Entities;
using CarFactoryAPI.Repositories_DAL;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;


namespace CarFactoryAPI_Tests
{
    public class CarRepositoryTests
    {
        // Create mock of dependencies
        Mock<FactoryContext> contextMock;

        // Use fake object as dependency
        CarRepository carRepository;

        List<Car> cars;
        public CarRepositoryTests()
        {
            // Create Mock of Dependencies
            contextMock = new();

            // use fake object as dependency
            carRepository = new(contextMock.Object);

            cars = new List<Car>() {
                new Car() { Id = 1},
                new Car() { Id = 2},
                new Car() { Id = 3}
            };

        }

        [Fact]
        public void GetAllCars_CarsList()
        {
            // Arrange
           
            // setup called Dbsets
            contextMock.Setup(o => o.Cars).ReturnsDbSet(cars);

            // Act
            var result = carRepository.GetAllCars();

            // Assert
            Assert.Equal(cars, result);
        }

        [Fact]
        public void AddCar_CarToAdd_True()
        {
            // Arrange

            var car = new Car { Id = 100 };

            // setup called Dbsets

            contextMock.Setup(o => o.Cars.Add(car));
            contextMock.Setup(o => o.SaveChanges());

            // Act
            var result = carRepository.AddCar(car);

            // Assert
            Assert.True(result);

        }


        [Fact]
        public void AssignToOwner_CarNotNull_True()
        {
            // Arrange
            Owner owner = new Owner() { Id = 100 };

            // setup called Dbsets
            contextMock.Setup(o => o.Cars).ReturnsDbSet(cars);

            // Act
            Car result = carRepository.GetCarById(1);
            var result2 = carRepository.AssignToOwner(1, 100);

            // Assert
            Assert.NotNull(result);
            Assert.True(result2);

        }

        [Fact]
        public void AssignToOwner_CarIsNull_False()
        {

            // Arrange
            Owner owner = new Owner { Id = 10 };
            contextMock.Setup(o => o.Cars).ReturnsDbSet(cars);
            // Act
            Car? result = carRepository.GetCarById(100);
            var result2 = carRepository.AssignToOwner(100, 10);

            // Assert
            Assert.Null(result);
            Assert.False(result2);
        }


    }
}


