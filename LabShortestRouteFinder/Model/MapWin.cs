using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabShortestRouteFinder.Model
{
    public class MapWin
    {
        public Tuple<int, int> WindowsMaxXY { get; set; }
        public Tuple<double, double> MinGpsCoord { get; set; }
        public Tuple<double, double> MaxGpsCoord { get; set; }

        public MapWin(Tuple<double, double> minGpsCoord, Tuple<double, double> maxGpsCoord, Tuple<int, int> windowMaxXY)
        {
            MinGpsCoord = minGpsCoord;
            MaxGpsCoord = maxGpsCoord;
            WindowsMaxXY = windowMaxXY;
        }

        public Tuple<int, int> GetWindowCoord(double latitude, double longitude)
        {
            if (latitude < MinGpsCoord.Item1 || latitude > MaxGpsCoord.Item1 || longitude < MinGpsCoord.Item2 || longitude > MaxGpsCoord.Item2)
            {
                throw new ArgumentException("Latitude and Longitude must be within the range of MinGpsCoord and MaxGpsCoord");
            }
            int x = (int)((longitude - MinGpsCoord.Item2) / (MaxGpsCoord.Item2 - MinGpsCoord.Item2) * WindowsMaxXY.Item1);
            int y = (int)((latitude - MinGpsCoord.Item1) / (MaxGpsCoord.Item1 - MinGpsCoord.Item1) * WindowsMaxXY.Item2);
            return new Tuple<int, int>(x, y);
        }

        public Tuple<int, int> GetWindowCoord(Tuple<double, double> gpsCoord)
        {
            return GetWindowCoord(gpsCoord.Item1, gpsCoord.Item2);
        }

    }

}
