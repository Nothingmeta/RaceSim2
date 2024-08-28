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
using System.Windows.Threading;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Data.Initialize();
            ImageCache.Initialize();

            Data.CurrentRace.DriversChanged += OnDriversChanged;
            Data.CurrentRace.NextRace += OnNextRace;

            InitializeComponent();
        }

        public void OnDriversChanged(object sender, EventArgs e)
        {
            DriversChangedEventArgs driverE = (DriversChangedEventArgs)e;
            this.TrackImg.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    this.TrackImg.Source = null;
                    this.TrackImg.Source = Visualisation.DrawTrack(driverE.Track);
                }));
        }

        public void OnNextRace(object sender, EventArgs e)
        {
            ImageCache.ClearCache();
            Data.CurrentRace.CleanupEvents();
            //Data.Competition.ParticipantTimesBroken.Reset();
            //Data.Competition.ParticipantsTimes.Reset();
            Data.NextRace();
            if (Data.CurrentRace != null)
            {
                /*Data.CurrentRace.DriversChanged += DataContextMainWindow.OnDriversChanged;
                if (_currentRaceStats != null)
                    Data.CurrentRace.DriversChanged += _currentRaceStats.DataContextCurrentRace.OnDriversChanged;
                if (_competitionPartStats != null)
                    Data.CurrentRace.DriversChanged += _competitionPartStats.DataContextCompetition.OnDriversChanged;*/
                Data.CurrentRace.DriversChanged += OnDriversChanged;
                Data.CurrentRace.NextRace += OnNextRace;
                Data.CurrentRace.Start();
            }
        }
    }
}