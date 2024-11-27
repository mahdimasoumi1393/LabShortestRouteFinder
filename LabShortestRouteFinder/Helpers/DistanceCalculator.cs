using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabShortestRouteFinder.Helpers
{
    public static class DistanceCalculator
    {
        private const double EarthRadiusKm = 6371.0;

        /// <summary>
        /// Calculates the geodesic distance between two points using Haversine-formula.
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon2"></param>
        /// <returns></returns>
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusKm * c;
        }

        /// <summary>
        /// Calculates the geodesic distance between two points using the Lambert formula.
        /// Lambert's formula assumes a spherical Earth.
        /// </summary>
        /// <param name="latitude1"></param>
        /// <param name="longitude1"></param>
        /// <param name="latitude2"></param>
        /// <param name="longitude2"></param>
        /// <returns></returns>
        public static double CalculateDistance2(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            // Convert latitude and longitude from degrees to radians
            double lat1 = ToRadians(latitude1);
            double lon1 = ToRadians(longitude1);
            double lat2 = ToRadians(latitude2);
            double lon2 = ToRadians(longitude2);

            // Calculate Lambert's formula terms
            double deltaLongitude = lon2 - lon1;

            double centralAngle = Math.Acos(
                Math.Sin(lat1) * Math.Sin(lat2) +
                Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(deltaLongitude)
            );

            // Distance is central angle times Earth's radius
            double distance = EarthRadiusKm * centralAngle;

            return distance;
        }

        private static double ToRadians(double angle)
        {
            return angle * Math.PI / 180.0;
        }
    }
}
