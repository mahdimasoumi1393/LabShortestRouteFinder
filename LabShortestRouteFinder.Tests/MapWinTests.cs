using System;
using Xunit;
using LabShortestRouteFinder.Model;
using LabShortestRouteFinder.Helpers;

namespace LabShortestRouteFinder.Tests.Model
{
    public class MapWinTests
    {
        [Fact]
        public void GetWindowCoord_CornersAndMiddle_Test()
        {
            // Arrange
            var minGpsCoord = new Tuple<double, double>(0.0, 0.0);
            var maxGpsCoord = new Tuple<double, double>(100.0, 100.0);
            var windowMaxXY = new Tuple<int, int>(1000, 1000);
            var mapWin = new MapWin(minGpsCoord, maxGpsCoord, windowMaxXY);

            // Act & Assert
            // Bottom-left corner
            var bottomLeft = mapWin.GetWindowCoord(0.0, 0.0);
            Assert.Equal(new Tuple<int, int>(0, 0), bottomLeft);

            // Bottom-right corner
            var bottomRight = mapWin.GetWindowCoord(0.0, 100.0);
            Assert.Equal(new Tuple<int, int>(1000, 0), bottomRight);

            // Top-left corner
            var topLeft = mapWin.GetWindowCoord(100.0, 0.0);
            Assert.Equal(new Tuple<int, int>(0, 1000), topLeft);

            // Top-right corner
            var topRight = mapWin.GetWindowCoord(100.0, 100.0);
            Assert.Equal(new Tuple<int, int>(1000, 1000), topRight);

            // Middle
            var middle = mapWin.GetWindowCoord(50.0, 50.0);
            Assert.Equal(new Tuple<int, int>(500, 500), middle);
        }

        [Fact]
        public void GivenTwoCoordinatesWithLatitudesInDifferentEmisphers_ARectangleIsReturned()
        {
            // Arrange

            var nwGpsCoord = new Tuple<double, double>(100.0, 0.0);             // NW (+lat, +lon)
            var neGpsCoord = new Tuple<double, double>(100.0, 100.0);
            var swGpsCoord = new Tuple<double, double>(-100.0, 0.0);            // SW (-lat, +lon)
            var seGpsCoord = new Tuple<double, double>(-100.0, 100.0);

            // Act
            int windowHeightLeft = (int)WGS84DistanceCalculator.CalculateDistance(nwGpsCoord.Item1, nwGpsCoord.Item2, swGpsCoord.Item1, swGpsCoord.Item2);
            int windowHeightRight = (int)WGS84DistanceCalculator.CalculateDistance(neGpsCoord.Item1, neGpsCoord.Item2, seGpsCoord.Item1, seGpsCoord.Item2);

            int windowWidthUp = (int)WGS84DistanceCalculator.CalculateDistance(nwGpsCoord.Item1, nwGpsCoord.Item2, neGpsCoord.Item1, neGpsCoord.Item2);
            int windowWidthDown = (int)WGS84DistanceCalculator.CalculateDistance(swGpsCoord.Item1, swGpsCoord.Item2, seGpsCoord.Item1, seGpsCoord.Item2);

            var windowMaxXY = new Tuple<int, int>(windowWidthUp, windowHeightLeft);
            var mapWin = new MapWin(nwGpsCoord, seGpsCoord, windowMaxXY);

            // Assert
            Assert.Equal(windowWidthUp, windowWidthDown);
            Assert.Equal(windowHeightLeft, windowHeightRight);
        }

        [Fact]
        public void GivenSouthEastAndWestPositions_ReturningWinXYBottonLeftRightPoints()
        {
            // Arrange
            var NorthWest = new Tuple<double, double>(69.0600, 10.9300);
            var NorthEast = new Tuple<double, double>(69.0600, 24.1600);
            var SouthWest = new Tuple<double, double>(55.2000, 10.9300);   
            var SouthEast = new Tuple<double, double>(55.2000, 24.1600);


            int windowWidthTop = (int)WGS84DistanceCalculator.CalculateDistance(NorthWest.Item1, NorthWest.Item2, NorthEast.Item1, NorthEast.Item2);
            int windowWidthBottom = (int)WGS84DistanceCalculator.CalculateDistance(SouthWest.Item1, SouthWest.Item2, SouthEast.Item1, SouthEast.Item2);

            int windowHeightLeft = (int)WGS84DistanceCalculator.CalculateDistance(NorthWest.Item1, NorthWest.Item2, SouthWest.Item1, SouthWest.Item2);
            int windowHeightRight = (int)WGS84DistanceCalculator.CalculateDistance(NorthEast.Item1, NorthEast.Item2, SouthEast.Item1, SouthEast.Item2);

            int windowWidth;
            int windowHeight;
            windowWidth = windowWidthTop > windowWidthBottom ? windowWidthTop : windowWidthBottom;
            windowHeight = windowHeightLeft > windowHeightRight ? windowHeightLeft : windowHeightRight;

            var windowMaxXY = new Tuple<int, int>(windowWidth, windowHeight);
            var mapWin = new MapWin(SouthWest, NorthEast, windowMaxXY);

            // Act
            var resultBL = mapWin.GetWindowCoord(SouthWest.Item1, SouthWest.Item2);
            var resultBR = mapWin.GetWindowCoord(SouthEast.Item1, SouthEast.Item2);

            var resultTL = mapWin.GetWindowCoord(NorthWest.Item1, NorthWest.Item2);
            var resultTR = mapWin.GetWindowCoord(NorthEast.Item1, NorthEast.Item2);

            // Assert
            // Bottom-Width should be gratter than Top-Width on this quadrant
            Assert.Equal(new Tuple<int, int>(0, 0), resultBL);

            Assert.Equal(new Tuple<int, int>(windowWidth, 0), resultBR);

            Assert.Equal(new Tuple<int, int>(0, windowHeight), resultTL);

            Assert.Equal(new Tuple<int, int>(windowWidth, windowHeight), resultTR);


        }
    }
}
