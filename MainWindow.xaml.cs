using System.Windows;
using System.Threading.Tasks;

namespace CourseWork
{
    public partial class MainWindow : Window
    {
        private readonly OS _os;
        private bool _pause;

        public MainWindow()
        {
            InitializeComponent();
            _os = new OS();
        }

        private async void AutoRun_Click(object sender, RoutedEventArgs e)
        {
            _pause = false;
            while (!_pause)
            {
                _os.UpdateSystemState();
                box.Text = _os.ToString();
                await Task.Delay(Config.TickRate);
            }
        }

        private void ManualRun_Click(object sender, RoutedEventArgs e)
        {
            _pause = true;
        }

        private void Step_Click(object sender, RoutedEventArgs e)
        {
            _os.UpdateSystemState();
            box.Text = _os.ToString();
        }
    }
}
