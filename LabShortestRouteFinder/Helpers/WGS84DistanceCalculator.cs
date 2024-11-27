using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabShortestRouteFinder.Helpers
{
    public class WGS84DistanceCalculator
    {
        // WGS84 ellipsoid constants
        private const double SemiMajorAxis = 6378137.0; // meters (equatorial radius)
        private const double Flattening = 1 / 298.257223563; // WGS84 flattening factor
        private const double SemiMinorAxis = SemiMajorAxis * (1 - Flattening); // meters (polar radius)

        /// <summary>
        /// Calculates the geodesic (WGS84) distance between two points using the Vincenty formula.
        /// </summary>
        /// <param name="latitude1">Latitude of the first point in degrees.</param>
        /// <param name="longitude1">Longitude of the first point in degrees.</param>
        /// <param name="latitude2">Latitude of the second point in degrees.</param>
        /// <param name="longitude2">Longitude of the second point in degrees.</param>
        /// <returns>The distance between the two points in meters.</returns>
        public static double CalculateDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            // Convert latitude and longitude from degrees to radians
            double lat1 = ToRadians(latitude1);
            double lon1 = ToRadians(longitude1);
            double lat2 = ToRadians(latitude2);
            double lon2 = ToRadians(longitude2);

            double a = SemiMajorAxis;
            double b = SemiMinorAxis;
            double f = Flattening;

            double L = lon2 - lon1;
            double U1 = Math.Atan((1 - f) * Math.Tan(lat1));
            double U2 = Math.Atan((1 - f) * Math.Tan(lat2));

            double sinU1 = Math.Sin(U1), cosU1 = Math.Cos(U1);
            double sinU2 = Math.Sin(U2), cosU2 = Math.Cos(U2);

            double lambda = L, lambdaP;
            double iterLimit = 100;
            double cosSqAlpha, sinSigma, cos2SigmaM, cosSigma, sigma;

            do
            {
                double sinLambda = Math.Sin(lambda);
                double cosLambda = Math.Cos(lambda);

                sinSigma = Math.Sqrt(
                    (cosU2 * sinLambda) * (cosU2 * sinLambda) +
                    (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda) *
                    (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda)
                );

                if (sinSigma == 0) return 0; // Co-incident points

                cosSigma = sinU1 * sinU2 + cosU1 * cosU2 * cosLambda;
                sigma = Math.Atan2(sinSigma, cosSigma);

                double sinAlpha = cosU1 * cosU2 * sinLambda / sinSigma;
                cosSqAlpha = 1 - sinAlpha * sinAlpha;

                cos2SigmaM = cosSqAlpha == 0 ? 0 : cosSigma - 2 * sinU1 * sinU2 / cosSqAlpha;

                double C = f / 16 * cosSqAlpha * (4 + f * (4 - 3 * cosSqAlpha));

                lambdaP = lambda;
                lambda = L + (1 - C) * f * sinAlpha *
                         (sigma + C * sinSigma *
                         (cos2SigmaM + C * cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM)));

            } while (Math.Abs(lambda - lambdaP) > 1e-12 && --iterLimit > 0);

            if (iterLimit == 0)
                throw new InvalidOperationException("Vincenty formula failed to converge");

            double uSq = cosSqAlpha * (a * a - b * b) / (b * b);
            double A = 1 + uSq / 16384 * (4096 + uSq * (-768 + uSq * (320 - 175 * uSq)));
            double B = uSq / 1024 * (256 + uSq * (-128 + uSq * (74 - 47 * uSq)));
            double deltaSigma = B * sinSigma *
                                (cos2SigmaM + B / 4 *
                                 (cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM) -
                                  B / 6 * cos2SigmaM * (-3 + 4 * sinSigma * sinSigma) *
                                  (-3 + 4 * cos2SigmaM * cos2SigmaM)));

            double s = b * A * (sigma - deltaSigma);

            return s / 1000; // Convert meters to kilometers
        }

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="degrees">Angle in degrees.</param>
        /// <returns>Angle in radians.</returns>
        private static double ToRadians(double degrees)
        {
            return degrees * (Math.PI / 180.0);
        }
    }
}
