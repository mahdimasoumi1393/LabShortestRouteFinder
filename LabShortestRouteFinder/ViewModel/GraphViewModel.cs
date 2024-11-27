using LabShortestRouteFinder;
using LabShortestRouteFinder.Model;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LabShortestRouteFinder.ViewModel
{
    public class GraphViewModel
    {
        
        public ObservableCollection<CityNode> Cities { get; }
        public ObservableCollection<Route> Routes { get; }
        public MainViewModel MainViewModel { get; }
        private Canvas _canvas;


        public GraphViewModel(MainViewModel mainViewModel) 
        {
            Cities = mainViewModel.Cities;
            Routes = mainViewModel.Routes;
            
        }

        //public GraphViewModel(MainViewModel mainViewModel)
        //{
        //    MainViewModel=mainViewModel;
        //}

        // Ny metod för att rita rutter, inklusive alla waypoints
        // Ny metod för att rita en rutt, inklusive waypoint
        public void DrawRoute(Route route)
        {
            // Samla alla punkter i rutten
            var allPoints = new List<CityNode> { route.Start }; // Startpunkten

            if (route.Waypoint != null) // Ändrat: Kontrollera om waypoint är definierad
            {
                allPoints.Add(route.Waypoint); // Ändrat: Lägg till waypoint
            }

            allPoints.Add(route.Destination); // Lägg till slutdestinationen

            // Rita linjer mellan alla städer (start → waypoint → destination)
            for (int i = 0; i < allPoints.Count - 1; i++)
            {
                DrawLine(allPoints[i], allPoints[i + 1]);
            }
        }

        // Metod för att rita linje mellan två CityNode-objekt
        private void DrawLine(CityNode start, CityNode end)
        {
            // Här ska koden för att rita linjen mellan start och end implementeras
            Line line = new Line
            {
                X1 = start.X, // X-koordinat för startpunkten
                Y1 = start.Y, // Y-koordinat för startpunkten
                X2 = end.X,   // X-koordinat för slutpunkten
                Y2 = end.Y,   // Y-koordinat för slutpunkten
                Stroke = Brushes.Black, // Färg på linjen
                StrokeThickness = 2 // Linjens tjocklek
            };

            _canvas.Children.Add(line); // Lägg till linjen på Canvas
        }
        // Ny metod för att rita alla rutter
        public void RenderGraph()
        {
            _canvas.Children.Clear(); // Rensa Canvas innan ritning

            foreach (var route in Routes)
            {
                DrawRoute(route);
            }
        }
    }
}
