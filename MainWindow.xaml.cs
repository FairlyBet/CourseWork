using System.Windows;
using System.Threading;
using System.Threading.Tasks;

namespace CourseWork
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly OS _os;


        public MainWindow()
        {
            InitializeComponent();
            _os = new OS();
        }

        private async void AutoRun_Click(object sender, RoutedEventArgs e)
        {
            _os.SetToAutomaticMode();
            Step.IsEnabled = false;
            AutoRun.IsEnabled = false;
            while (!Step.IsEnabled)
            {
                box.Text = _os.ToString();
                await Task.Delay(500);
            }
        }

        private void ManualRun_Click(object sender, RoutedEventArgs e)
        {
            _os.SetToManualMode();
            Step.IsEnabled = true;
            AutoRun.IsEnabled = true;
        }

        private void Step_Click(object sender, RoutedEventArgs e)
        {
            _os.UpdateSystemState();
            box.Text = _os.ToString();
        }
    }
}
