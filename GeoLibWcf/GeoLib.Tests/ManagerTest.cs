using System;
using GeoLib.Contracts;
using GeoLib.Data;
using GeoLib.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GeoLib.Tests
{
    [TestClass]
    public class ManagerTest
    {
        [TestMethod]
        public void Test_Zip_Code_Retrival()
        {
            Mock<IZipCodeRepository> mockZipCodeRepository = new Mock<IZipCodeRepository>();

            ZipCode zipCode = new ZipCode
            {
                City = "LINCOLN PARK",
                State = new State() { Abbreviation = "NJ" },
                Zip = "07035"
            };

            mockZipCodeRepository.Setup(obj => obj.GetByZip("07035")).Returns(zipCode);

            IGeoService geoService = new GeoManager(mockZipCodeRepository.Object);

            ZipCodeData data = geoService.GetZipInfo("07035");

            bool result = (data.City.ToUpper() == zipCode.City)
                && (data.State == zipCode.State.Abbreviation)
                && (data.ZipCode == zipCode.Zip);

            Assert.IsTrue(result);
        }
    }
}
