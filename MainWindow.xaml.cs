using System.Windows;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CourseWork
{
    public partial class MainWindow : Window
    {
        private readonly OS _os;
        private readonly ObservableCollection<ProcessStatistic> _processesSource;
        private readonly ObservableCollection<ProcessStatistic> _rejected;
        private readonly ObservableCollection<ProcessStatistic> _terminated;
        private readonly ObservableCollection<CPUStatistic> _CPUSource;
        private  ProcessWindow _creationWindow;
        private  InfoWindow _infoWindowTerminated;
        private  InfoWindow _infoWindowRejected;
        private bool _pause;


        public MainWindow()
        {
            InitializeComponent();
            _os = new OS();
            _pause = true;
            _processesSource = new();
            _CPUSource = new();
            _terminated = new();
            _rejected = new();
            ProcessesList.ItemsSource = _processesSource;
            CPUList.ItemsSource = _CPUSource;
        }

        private async void AutoRun_Click(object sender, RoutedEventArgs e)
        {
            if (!_pause)
            {
                return;
            }
            _pause = false;
            while (!_pause)
            {
                _os.UpdateSystemState();
                UpdateSources();
                await Task.Delay(Config.TickRate);
            }
        }
       
        private void ManualRun_Click(object sender, RoutedEventArgs e)
        {
            _pause = true;
        }

        private void Step_Click(object sender, RoutedEventArgs e)
        {
            if (!_pause)
            {
                return;
            }
            _os.UpdateSystemState();
            UpdateSources();
        }

        private void UpdateSources()
        {
            Fill(_os.Statistic.Processes, _processesSource);
            Fill(_os.Statistic.RejectedProcesses, _rejected);
            Fill(_os.Statistic.TerminatedProcesses, _terminated);
            Fill(_os.Statistic.CPUStatistics, _CPUSource);
        }

        private static void Fill<T>(IEnumerable<T> source, ObservableCollection<T> target)
        {
            target.Clear();
            foreach (var item in source)
            {
                target.Add(item);
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            _creationWindow = new(_os);
            _creationWindow.Owner = this;
            _creationWindow.Show();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            _os.RaiseNewProcess(new(0));
        }

        private void ShowRejected_Click(object sender, RoutedEventArgs e)
        {
            _infoWindowRejected = new(_rejected);
            _infoWindowRejected.Owner = this;
            _infoWindowRejected.Show();
        }

        private void ShowTerminated_Click(object sender, RoutedEventArgs e)
        {
            _infoWindowTerminated = new(_terminated);
            _infoWindowTerminated.Owner = this;
            _infoWindowTerminated.Show();
        }
    }
}
