using LabShortestRouteFinder.Helpers;
namespace LabShortestRouteFinder.Tests
{
    public class HelpersTests
    {
        private const double MarginOfError = 15.0;

        [Fact]
        public void GivenTwoPointsWithSameCoorLatLon_ReturnsCorrectDistanceInKm()
        {
            //Arrange
            double lat1 = 59.8696;
            double lon1 = 17.6283;
            double lat2 = 59.8696;
            double lon2 = 17.6283;
            double expectedDistance = 0.0;
            //Act
            double actualDistance = DistanceCalculator.CalculateDistance(lat1, lon1, lat2, lon2);
            //Assert
            Assert.InRange(actualDistance, expectedDistance - 0.0, expectedDistance + 0.0);
        }

        [Fact]
        public void CalculatingEquatorOnTheNEQuadrant_ReturnsTheEarthCinrcunference()
        {
            //Arrange
            double lat1 = 0.0;
            double lon1 = 0.0;
            double lat2 = 0.0;
            double lon2 = 180.0;
            double expectedDistance = 40075;
            double marginOfError = 55.9;
            //Act
            double actualDistance = DistanceCalculator.CalculateDistance(lat1, lon1, lat2, lon2);
            //Assert
            Assert.InRange(actualDistance*2, expectedDistance - marginOfError, expectedDistance + marginOfError);
        }

        [Fact]
        public void CalculatingEquatorOnTheNWQuadrant_ReturnsTheEarthCinrcunference()
        {
            //Arrange
            double lat1 = 0.0;
            double lon1 = 0.0;
            double lat2 = 0.0;
            double lon2 = -180.0;
            double expectedDistance = 40075;
            double marginOfError = 55.9;
            //Act
            double actualDistance = DistanceCalculator.CalculateDistance(lat1, lon1, lat2, lon2);
            //Assert
            Assert.InRange(actualDistance*2, expectedDistance - marginOfError, expectedDistance + marginOfError);
        }

        [Fact]
        public void GivenTwoPointsWithCoorLatLon_ReturnsCorrectDistanceInKm()
        {
            //Arrange
            double lat1 = 59.8696;
            double lon1 = 17.6283;
            double lat2 = 59.3360;
            double lon2 = 18.0788;
            double expectedDistance = 64.5;

            //Act
            double actualDistance = DistanceCalculator.CalculateDistance(lat1, lon1, lat2, lon2);

            //Assert
            Assert.InRange(actualDistance, expectedDistance - MarginOfError, expectedDistance + MarginOfError );

        }

        [Theory]
        [InlineData(40.7128, -74.0060, 34.0522, -118.2437, 3940.0)] // New York City to Los Angeles
        [InlineData(51.5074, -0.1278, 48.8566, 2.3522, 343.6)]      // London to Paris
        [InlineData(35.6895, 139.6917, 37.7749, -122.4194, 8278.0)] // Tokyo to San Francisco
        [InlineData(-33.8688, 151.2093, -37.8136, 144.9631, 713.0)] // Sydney to Melbourne
        [InlineData(59.8696, 17.6283, 59.3360, 18.0788, 64.5)]      // Uppsala to Stockholm
        public void CalculateDistanceBetweenTwoGeoPoints_ShouldReturnCorrectDistance(
        double latitude1, double longitude1, double latitude2, double longitude2, double expectedDistance)
        {
            // Act
            double actualDistance = DistanceCalculator.CalculateDistance(latitude1, longitude1, latitude2, longitude2);

            // Assert
            Assert.InRange(actualDistance, expectedDistance - MarginOfError, expectedDistance + MarginOfError);
        }

        [Theory]
        [InlineData(40.7128, -74.0060, 34.0522, -118.2437, 3935.0)] // New York City to Los Angeles
        [InlineData(51.5074, -0.1278, 48.8566, 2.3522, 335.6)]      // London to Paris
        [InlineData(35.6895, 139.6917, 37.7749, -122.4194, 8280.0)] // Tokyo to San Francisco
        [InlineData(-33.8688, 151.2093, -37.8136, 144.9631, 713.0)] // Sydney to Melbourne
        [InlineData(59.8696, 17.6283, 59.3360, 18.0788, 64.5)]      // Uppsala to Stockholm
        public void CalculateDistanceWithTwoDifferentLambertsFunctions_ShouldReturnValuesWithinAMarginOfError(
double latitude1, double longitude1, double latitude2, double longitude2, double expectedDistance)
        {
            // Act
            double actualDistance = DistanceCalculator.CalculateDistance(latitude1, longitude1, latitude2, longitude2);
            double actualDistanceNew = DistanceCalculator.CalculateDistance2(latitude1, longitude1, latitude2, longitude2);

            // Assert
            Assert.InRange(actualDistanceNew, actualDistance-MarginOfError, actualDistance+actualDistanceNew);
        }

        [Theory]
        [InlineData(40.7128, -74.0060, 34.0522, -118.2437, 3935.7)] // New York City to Los Angeles
        [InlineData(51.5074, -0.1278, 48.8566, 2.3522, 335.66)]      // London to Paris
        [InlineData(35.6895, 139.6917, 37.7749, -122.4194, 8278.0)] // Tokyo to San Francisco
        [InlineData(-33.8688, 151.2093, -37.8136, 144.9631, 713.0)] // Sydney to Melbourne
        [InlineData(59.8696, 17.6283, 59.3360, 18.0788, 64.5)]      // Uppsala to Stockholm
        public void CalculateDistanceBetweenTwoGeoPointsWithWGS84_ShouldReturnCorrectDistance(
        double latitude1, double longitude1, double latitude2, double longitude2, double expectedDistance)
        {
            // Act

            // Use https://www.geodatos.net/en/distances/cities to get test data.
            double actualDistance = WGS84DistanceCalculator.CalculateDistance(latitude1, longitude1, latitude2, longitude2);

            // Assert
            Assert.InRange(actualDistance, expectedDistance - MarginOfError, expectedDistance + MarginOfError);
        }

    }
}