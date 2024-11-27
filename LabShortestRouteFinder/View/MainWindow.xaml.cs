using LabShortestRouteFinder.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace LabShortestRouteFinder.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel MainViewModel { get; }

        public RouteViewModel RouteViewModel { get; }
        public GraphViewModel GraphViewModel { get; }
        //private GraphViewModel _graphViewModel;

        public MainWindow()
        {
            InitializeComponent();

            MainViewModel = new MainViewModel();
            RouteViewModel = new RouteViewModel(MainViewModel);
            GraphViewModel = new GraphViewModel(MainViewModel);
            //_graphViewModel = new GraphViewModel(new MainViewModel(), MyCanvas);


            //this.DataContext = _graphViewModel;

            DataContext = MainViewModel;
        }


            private const string RoutesFilePath = "Resources/routes.json";

        private void SaveRouteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Läs data från textfält
                string start = StartTextBox.Text;
                string destination = DestinationTextBox.Text;
                string waypoint = WaypointTextBox.Text;
                if (!double.TryParse(DrivingDistanceTextBox.Text, out double drivingDistance))
                {
                    MessageBox.Show("Driving Distance måste vara ett giltigt tal.");
                    return;
                }
                if (!double.TryParse(StraightLineDistanceTextBox.Text, out double straightLineDistance))
                {
                    MessageBox.Show("Straight Line Distance måste vara ett giltigt tal.");
                    return;
                }
                if (!double.TryParse(CostTextBox.Text, out double cost))
                {
                    MessageBox.Show("Cost måste vara ett giltigt tal.");
                    return;
                }

                // Skapa ett nytt ruttobjekt
                var newRoute = new
                {
                    Start = start,
                    Destination = destination,
                    Waypoint = waypoint,
                    DrivingDistance = drivingDistance,
                    StraightLineDistance = straightLineDistance,
                    Cost = cost
                };

                // Läs befintliga rutter från filen
                List<object> routes;
                if (File.Exists(RoutesFilePath))
                {
                    string existingData = File.ReadAllText(RoutesFilePath);
                    routes = JsonSerializer.Deserialize<List<object>>(existingData) ?? new List<object>();
                }
                else
                {
                    routes = new List<object>();
                }

                // Lägg till den nya rutten
                routes.Add(newRoute);

                // Skriv till filen
                string json = JsonSerializer.Serialize(routes, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(RoutesFilePath, json);

                MessageBox.Show("Rutt sparad!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ett fel inträffade: {ex.Message}");
            }
        }
        


        private void OnNavigationSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NavigationListBox.SelectedItem is ListBoxItem selectedItem)
            {
                string? tabName = selectedItem.Tag as string;

                foreach (TabItem tab in MainTabControl.Items)
                {
                    if (tab.Name == tabName)
                    {
                        MainTabControl.SelectedItem = tab;
                        break;
                    }
                }
            }
        }
        //private void SaveRouteButton_Click(object sender, RoutedEventArgs e)
        //{
        //    // Läser in data från textfälten
        //    string start = StartTextBox.Text;
        //    string destination = DestinationTextBox.Text;
        //    string waypoint = WaypointTextBox.Text;
        //    string drivingDistance = DrivingDistanceTextBox.Text;
        //    string straightLineDistance = StraightLineDistanceTextBox.Text;
        //    string cost = CostTextBox.Text;

        //    // Skapar en meddelanderuta för att visa att datan har sparats (placeholder)
        //    MessageBox.Show($"Route saved:\nStart: {start}\nDestination: {destination}\nWaypoint: {waypoint}\nDriving Distance: {drivingDistance}\nStraight Line Distance: {straightLineDistance}\nCost: {cost}");
        //}

    }
}