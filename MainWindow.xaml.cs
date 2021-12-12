using System.Windows;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace CourseWork
{
    public partial class MainWindow : Window
    {
        private readonly OS _os;
        private readonly ObservableCollection<ProcessStatistic> _processesSource;
        private readonly ObservableCollection<CPUStatistic> _CPUSource;
        private bool _pause;


        public MainWindow()
        {
            InitializeComponent();
            _os = new OS();
            _pause = true;
            _processesSource = new();
            _CPUSource = new();
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
            _processesSource.Clear();
            foreach (var item in _os.Statistic.Processes)
            {
                _processesSource.Add(item);
            }

            _CPUSource.Clear();
            foreach (var item in _os.Statistic.CPUStatistics)
            {
                _CPUSource.Add(item);
            }
        }
    }
}
